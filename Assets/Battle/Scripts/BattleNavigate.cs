﻿using Priority_Queue;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleNavigate : MonoBehaviour
{
    //private Unit unit; //VARLER - find unit to interact with
    private GameObject unit;
    UnitData unitData;
    Selectable unitSelectable;
    UnitStateController unitState;
    Parameters unitParameters;


    [Header("Integrate to use real unit stats")]
    [Header("Ronald: Each unit prefab will also have a Unit component attached containing stats ")]

    [SerializeField]
    Int2 currentUnitPosition; // old, used for calculating all paths in area


    Vector3 previousUnitPosition; // used when cancelling movement made

 //   [SerializeField]
    int maxDistance;
    float maxJump;

    [SerializeField]
    GameObject squarePrefab;
    [SerializeField]
    Mesh quadMesh;
    [SerializeField]
    Material gridMat;

    GameObject visualsParent;
    Int2 lastGoal;
    List<Int2> path;
    bool traveling;
	bool pathHasBeenReset; //VARLER - for resetting the path prior to entering Move()
    TerrainData levelTerrain;

    //public static Unit[,] unitsGrid;

    GameObject[,] unitsGrid;

    private void Start()
    {
        levelTerrain = LevelGrid.Instance.GetComponent<Terrain>().terrainData;
        //SpawnVisualGrid(new GameObject("Visual Grid Parent").transform, quadMesh, levelTerrain, gridMat);


        unit = gameObject;
        unitsGrid = BattleState.unitsGrid;


        if (unitsGrid == null)
        {
            var mapSize = LevelGrid.Instance.GetMapSize();
            //unitsGrid = new Unit[mapSize.x, mapSize.y];
            unitsGrid = new GameObject[mapSize.x, mapSize.y];
        }

        unitData = unit.GetComponent<UnitData>();
        unitSelectable = unit.GetComponent<Selectable>();
        unitState = unit.GetComponent<UnitStateController>();
        unitParameters = unit.GetComponent<Parameters>();
        AddUnit(new Int2((int)transform.position.x, (int)transform.position.z), gameObject);

    }

    private void Awake()
    {

    }

    internal void Move()
    {
        maxDistance = unitParameters.MOV;
        maxJump = unitParameters.JMP;

        //Debug.Log("called Move(). Traveling: " + traveling + ". Path: " + path +".");
        //VARLER - if statement ensures path has been reset before re-entering Move()
        if (!pathHasBeenReset)
		{
			path = null;
			pathHasBeenReset = true;
		}

        if (!traveling && Input.GetMouseButtonDown(0) && path != null)
        {
            previousUnitPosition = transform.position;
            StartCoroutine(Travel(path));
			pathHasBeenReset = false; //VARLER - allow path to be reset again
			return; //VARLER - failsafe for cutting out of function once move is complete
        }
        if (!traveling && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 1000f))
        {
			//Debug.Log("Looking at paths");
            var goal = new Int2((int)hit.point.x, (int)hit.point.z);
            if (goal == lastGoal)
                return;
            lastGoal = goal;
            path = CalculatePath(new Int2((int)transform.position.x, (int)transform.position.z), goal);
        }
    }

    internal void Return()
    {
        if(previousUnitPosition != null)
            transform.position = previousUnitPosition;

        pathHasBeenReset = true;
    }

    IEnumerator Travel(List<Int2> path)
    {
        //unit.isActive = false; //VARLER - prevent interaction with moving unit
        unitSelectable.isActive = false;


		traveling = true;
        RemoveUnit(new Int2((int)transform.position.x, (int)transform.position.z));
        while (path.Count > 0)
        {
            var cell = path[path.Count - 1];
            var target = new Vector3(cell.x + 0.5f, LevelGrid.Instance.GetHeight(cell.x, cell.y), cell.y + 0.5f);
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 10f);
            if (Vector3.SqrMagnitude(transform.position - target) < 0.01)
                path.RemoveAt(path.Count - 1);
            yield return null;
        }
        if (visualsParent != null)
            Destroy(visualsParent);
        AddUnit(new Int2((int)transform.position.x, (int)transform.position.z), unit);
        traveling = false;

		unitSelectable.isActive = true; //VARLER - allow interaction once move is complete
        unitState.DoneMoving(); //VARLER - execute code for unit done moving upon completion of coroutine
    }


    List<Int2> CalculatePath(Int2 start, Int2 goal)
    {
        if (visualsParent != null)
            Destroy(visualsParent);
        visualsParent = new GameObject("Walking visuals");
        if (start.x < 0 || start.y < 0 || start.x >= LevelGrid.Instance.GetMapSize().x || start.y >= LevelGrid.Instance.GetMapSize().y)
            return null;
        if (goal.x < 0 || goal.y < 0 || goal.x >= LevelGrid.Instance.GetMapSize().x || goal.y >= LevelGrid.Instance.GetMapSize().y)
            return null;
        if (IsUnitAt(goal))
            return null;

        //SpawnVisualGridAround(visualsParent.transform, quadMesh, levelTerrain, gridMat, start, maxDistance);
        var path = Astar(start, goal, maxDistance, maxJump);
        var go = Instantiate(squarePrefab, new Vector3(goal.x + 0.5f, LevelGrid.Instance.GetHeight(goal.x, goal.y) + 0.1f, goal.y + 0.5f), squarePrefab.transform.rotation, visualsParent.transform);
        if (path != null)
        {
            go.GetComponent<Renderer>().sharedMaterial.SetColor("_TintColor", Color.green);
            return path;
        }
        go.GetComponent<Renderer>().sharedMaterial.SetColor("_TintColor", Color.red);
        return null;
    }

    static void SpawnVisualGrid(Transform holder, Mesh quad, TerrainData data, Material mat)
    {

        for (int x = 0; x < LevelGrid.Instance.GetMapSize().x; x++)
        {
            for (int y = 0; y < LevelGrid.Instance.GetMapSize().y; y++)
            {
                var go = new GameObject();
                go.transform.SetParent(holder, false);
                go.transform.position = new Vector3(x + 0.5f, LevelGrid.Instance.GetHeight(x, y) + 0.1f, y + 0.5f);
                var filter = go.AddComponent<MeshFilter>();
                go.AddComponent<MeshRenderer>().sharedMaterial = mat;
                var mesh = new Mesh();
                mesh.vertices = new Vector3[] {
                    new Vector3(-0.5f, 0,-0.5f),
                    new Vector3(0.5f, 0,0.5f),
                    new Vector3(0.5f, 0,-0.5f),
                    new Vector3(-0.5f, 0,0.5f),
                };
                mesh.triangles = quad.triangles;
                mesh.uv = quad.uv;
                filter.sharedMesh = mesh;
            }
        }

    }

    static void SpawnVisualGridAround(Transform holder, Mesh quad, TerrainData data, Material mat, Int2 center, int maxDistance)
    {

        for (int x = -maxDistance; x <= maxDistance; x++)
        {
            for (int y = -maxDistance; y <= maxDistance; y++)
            {
                if (center.x + x < 0 || center.y + y < 0 || center.x + x >= LevelGrid.Instance.GetMapSize().x || center.y + y >= LevelGrid.Instance.GetMapSize().y)
                    continue;
                if (Mathf.Abs(x) + Mathf.Abs(y) > maxDistance)
                    continue;
                var go = new GameObject();
                go.transform.SetParent(holder, false);
                go.transform.position = new Vector3(center.x + x + 0.5f, 0, center.y + y + 0.5f);
                var filter = go.AddComponent<MeshFilter>();
                go.AddComponent<MeshRenderer>().sharedMaterial = mat;
                var mesh = new Mesh();
                mesh.vertices = new Vector3[] {
                    new Vector3(-0.5f, data.GetHeight(center.x + x, center.y + y) + 0.1f,-0.5f),
                    new Vector3(0.5f, data.GetHeight(center.x + x+1, center.y + y+1) + 0.1f,0.5f),
                    new Vector3(0.5f, data.GetHeight(center.x + x+1, center.y + y) + 0.1f,-0.5f),
                    new Vector3(-0.5f, data.GetHeight(center.x + x, center.y + y+1) + 0.1f,0.5f),
                };
                mesh.triangles = quad.triangles;
                mesh.uv = quad.uv;
                filter.sharedMesh = mesh;

                //Debug.Log(verts[0] + ", " + verts[1] + ", " + verts[2]);
            }
        }
    }

    void CalculateTraversability()
    {
        if (visualsParent != null)
            Destroy(visualsParent);
        visualsParent = new GameObject("Walking visuals");
        for (int x = currentUnitPosition.x - maxDistance; x < currentUnitPosition.x + maxDistance + 1; x++)
        {
            for (int z = currentUnitPosition.y - maxDistance; z < currentUnitPosition.y + maxDistance + 1; z++)
            {
                if (Astar(currentUnitPosition, new Int2(x, z), maxDistance, maxJump) != null)
                    Instantiate(squarePrefab, new Vector3(x + 0.5f, LevelGrid.Instance.GetHeight(x, z) + 0.25f, z + 0.5f), squarePrefab.transform.rotation, visualsParent.transform);
            }
        }
    }

    List<Int2> Astar(Int2 start, Int2 goal, int maxDistance, float maxJump)
    {
        HashSet<Int2> closedSet = new HashSet<Int2>();
        var openSet = new SimplePriorityQueue<Int2>();

        Dictionary<Int2, Int2> cameFrom = new Dictionary<Int2, Int2>();

        Dictionary<Int2, float> gScore = new Dictionary<Int2, float>();
        //have float.Infinity as a default
        gScore[start] = 0;

        Dictionary<Int2, float> fScore = new Dictionary<Int2, float>();
        fScore[start] = Heuristic(start, goal);
        openSet.Enqueue(start, fScore[start]);

        while (openSet.Count > 0)
        {
            var current = openSet.Dequeue();
            if (current == goal)
                return ReconstructPath(cameFrom, current, maxDistance);
            var currHeight = LevelGrid.Instance.GetHeight(current.x, current.y);
            closedSet.Add(current);
            var ns = new Int2[] { current + Int2.right, current - Int2.right, current + Int2.up, current - Int2.up };
            foreach (var neighbor in ns)
            {
                if (closedSet.Contains(neighbor))
                    continue;
                if (neighbor.x < 0 || neighbor.y < 0 || neighbor.x >= LevelGrid.Instance.GetMapSize().x || neighbor.y >= LevelGrid.Instance.GetMapSize().y)
                {
                    closedSet.Add(neighbor);
                    continue;
                }
                var neighHeight = LevelGrid.Instance.GetHeight(neighbor.x, neighbor.y);
                if (Mathf.Abs(currHeight - neighHeight) > maxJump) //cannot cross the height difference
                    continue;
                if (!LevelGrid.Instance.IsWalkable(neighbor.x, neighbor.y)) //cell not walkable
                    continue;
                if (IsNonAllyAt(neighbor)) // cell is occupied by a different team unit
                    continue;

                var tentativegScore = gScore[current] + 1;
                gScore[neighbor] = tentativegScore;
                fScore[neighbor] = tentativegScore + Heuristic(neighbor, goal);
                if (!openSet.Contains(neighbor))
                {
                    openSet.Enqueue(neighbor, fScore[neighbor]);
                }
                else if (tentativegScore >= gScore[neighbor])
                    continue;

                cameFrom[neighbor] = current;
            }
        }
        return null;
    }


    static List<Int2> ReconstructPath(Dictionary<Int2, Int2> cameFrom, Int2 current, int maxDistance)
    {
        var path = new List<Int2>();
        path.Add(current);
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
            maxDistance--;
        }
        if (maxDistance < 0)
            return null;
        return path;
    }

    public void ResetNavigator()
    {
        if (visualsParent != null)
            Destroy(visualsParent);

        traveling = false;
    }

    static float Heuristic(Int2 start, Int2 goal)
    {
        return Int2.Distance(start, goal); // manhattan distance(per-axis)
    }

    public Int2 GetUnitPosition()
    {
        return new Int2((int)transform.position.x, (int)transform.position.y);
    }

    public bool IsNonAllyAt(Int2 pos)
    {
        //return unitsGrid[pos.x, pos.y] != null && unitsGrid[pos.x, pos.y].unitAffiliation != unit.unitAffiliation;
        return unitsGrid[pos.x, pos.y] != null && unitsGrid[pos.x, pos.y].GetComponent<UnitData>().UnitTeam != unitData.UnitTeam;
    }

    public bool IsUnitAt(Int2 pos)
    {
        return unitsGrid[pos.x, pos.y] != null;
    }


    /*
    public void AddUnit(Int2 pos, Unit unit)
    {
        if (unitsGrid[pos.x, pos.y] != null)
            Debug.LogError("Logic error, trying to place one unit on top of another. " + unitsGrid[pos.x, pos.y].GetUnitName() + ", " + unit.GetUnitName(), this);
        unitsGrid[pos.x, pos.y] = unit;
    }
    */

    public void AddUnit(Int2 pos, GameObject unit)
    {
        if (unitsGrid[pos.x, pos.y] != null)
            Debug.LogError("Logic error, trying to place one unit on top of another. " + unitsGrid[pos.x, pos.y].GetComponent<UnitData>().UnitName, this);
        unitsGrid[pos.x, pos.y] = unit;

    }
     

    public void RemoveUnit(Int2 pos)
    {
        if (unitsGrid[pos.x, pos.y] == null)
            Debug.LogWarning("Trying to remove a unit from empty position, probably an error. " + pos, this);
        unitsGrid[pos.x, pos.y] = null;
    }
}
