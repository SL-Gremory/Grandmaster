using Priority_Queue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleControl : MonoBehaviour
{

    [Header("Integrate to use real unit stats")]
    [SerializeField]
    Int2 currentUnitPosition;
    [SerializeField]
    int maxDistance;
    [SerializeField]
    float maxJump;
    [SerializeField]
    GameObject squarePrefab;
    [SerializeField]
    bool recalculate;

    GameObject visualsParent;

    private void Update()
    {
        if (recalculate)
        {
            recalculate = false;
            CalculateTraversability();
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
                    Instantiate(squarePrefab, new Vector3(x, LevelGrid.Instance.GetHeight(x, z) + 0.25f, z), squarePrefab.transform.rotation, visualsParent.transform);
            }
        }
    }

    static List<Int2> Astar(Int2 start, Int2 goal, int maxDistance, float maxJump)
    {
        if (start.x < 0 || start.y < 0 || start.x >= LevelGrid.Instance.GetMapSize().x || start.y >= LevelGrid.Instance.GetMapSize().y)
            return null;
        if (goal.x < 0 || goal.y < 0 || goal.x >= LevelGrid.Instance.GetMapSize().x || goal.y >= LevelGrid.Instance.GetMapSize().y)
            return null;
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
                if (Mathf.Abs(currHeight - neighHeight) > maxJump)
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

    static float Heuristic(Int2 start, Int2 goal)
    {
        return Int2.Distance(start, goal); // manhattan distance(per-axis)
    }

}




