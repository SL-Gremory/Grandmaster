using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerrainHelper))]
public class TerrainHelperEditor : Editor
{
    SerializedProperty gridDist;
    SerializedProperty showTerrainLines;
    SerializedProperty showWalkability;
    SerializedProperty showPrefabs;
    SerializedProperty editWalkability;
    SerializedProperty setUnwalkable;
    SerializedProperty paintPrefabs;
    SerializedProperty paintedPrefab;
    SerializedProperty prefabAdd;
    SerializedProperty prefabReplace;
    SerializedProperty prefabRemove;

    GUIStyle textStyle = new GUIStyle();
    LevelGrid grid;
    TerrainData terrainData;

    [MenuItem("GameObject/Create Game Level", false, 0)]
    static void SpawnTerrain()
    {
        var data = new TerrainData();
        data.size = new Vector3(32, 20, 32);
        data.heightmapResolution = 65;
        data.alphamapResolution = 64;
        var go = Terrain.CreateTerrainGameObject(data);
        go.GetComponent<Terrain>().materialType = Terrain.MaterialType.Custom;
        go.GetComponent<Terrain>().materialTemplate = Resources.Load<Material>("PixelartTerrainMat");
        go.AddComponent<TerrainHelper>();
        go.name = "Level";
    }

    private void OnEnable()
    {
        grid = (target as TerrainHelper).GetComponent<LevelGrid>();
        terrainData = (target as TerrainHelper).GetComponent<Terrain>().terrainData;
        textStyle.fontStyle = FontStyle.Bold;
        textStyle.padding = new RectOffset(4, 4, 2, 2);
        textStyle.fontSize = 14;
        textStyle.border = new RectOffset(4, 4, 2, 2);
        gridDist = serializedObject.FindProperty("gridDistance");
        showTerrainLines = serializedObject.FindProperty("showTerrainLines");
        showWalkability = serializedObject.FindProperty("showWalkability");
        showPrefabs = serializedObject.FindProperty("showPrefabs");
        editWalkability = serializedObject.FindProperty("editWalkability");
        setUnwalkable = serializedObject.FindProperty("setUnwalkable");
        paintedPrefab = serializedObject.FindProperty("paintedPrefab");
        paintPrefabs = serializedObject.FindProperty("paintPrefabs");
        prefabAdd = serializedObject.FindProperty("prefabAdd");
        prefabReplace = serializedObject.FindProperty("prefabReplace");
        prefabRemove = serializedObject.FindProperty("prefabRemove");
    }

    public override void OnInspectorGUI()
    {
        GUI.skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene);
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();

        var tSize = EditorGUILayout.IntField("Terrain size:", Mathf.RoundToInt(terrainData.size.x));
        if (tSize != Mathf.RoundToInt(terrainData.size.x))
        {
            if (tSize > 31 && tSize < 20480)
            {
                terrainData.heightmapResolution = tSize + 1;
                terrainData.size = new Vector3(tSize, terrainData.size.y, tSize);
                terrainData.alphamapResolution = tSize;
                grid.SampleHeights();
                Undo.RecordObjects(new Object[] { grid, terrainData }, "Changed terrain size");

            }
        }
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Visualize:");
        EditorGUILayout.BeginHorizontal();
        showTerrainLines.boolValue = GUILayout.Toggle(showTerrainLines.boolValue, "Terrain", "Button");
        showWalkability.boolValue = GUILayout.Toggle(showWalkability.boolValue, "Walkability", "Button");
        showPrefabs.boolValue = GUILayout.Toggle(showPrefabs.boolValue, "Prefabs", "Button");
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.IntSlider(gridDist, 0, 25);
        if (!showPrefabs.boolValue)
            grid.HidePrefabsAll();
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Terrain Painting:");
        editWalkability.boolValue = GUILayout.Toggle(editWalkability.boolValue, "Paint Walkability", "Button");
        if (editWalkability.boolValue)
        {
            setUnwalkable.boolValue = GUILayout.Toggle(setUnwalkable.boolValue, setUnwalkable.boolValue ? "Set Unwalkable" : "Set Walkable", "Button");
            paintPrefabs.boolValue = false;
        }
        paintPrefabs.boolValue = GUILayout.Toggle(paintPrefabs.boolValue, "Paint Prefabs", "Button");
        if (paintPrefabs.boolValue)
        {
            editWalkability.boolValue = false;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.ObjectField(paintedPrefab, GUILayout.ExpandWidth(false));
            prefabAdd.boolValue = GUILayout.Toggle(prefabAdd.boolValue, "Add", "Button");
            if (prefabAdd.boolValue)
            {
                prefabReplace.boolValue = false;
                prefabRemove.boolValue = false;
            }
            else if (!prefabReplace.boolValue && !prefabRemove.boolValue)
                prefabAdd.boolValue = true;
            prefabReplace.boolValue = GUILayout.Toggle(prefabReplace.boolValue, "Replace", "Button");
            if (prefabReplace.boolValue)
            {
                prefabAdd.boolValue = false;
                prefabRemove.boolValue = false;
            }
            prefabRemove.boolValue = GUILayout.Toggle(prefabRemove.boolValue, "Remove", "Button");
            if (prefabRemove.boolValue)
            {
                prefabAdd.boolValue = false;
                prefabReplace.boolValue = false;
            }
            EditorGUILayout.EndHorizontal();
        }

        serializedObject.ApplyModifiedProperties();
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Changed terrain helper settings.");
        }

        GUI.skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
    }

    int mouseX, mouseZ;

    private void OnSceneGUI()
    {/*
        TerrainHelper t = target as TerrainHelper;
        if (!t)
            return;
        var terrainData = t.GetComponent<Terrain>().terrainData;
        var col = t.GetComponent<TerrainCollider>();*/
        if (Physics.Raycast(HandleUtility.GUIPointToWorldRay(Event.current.mousePosition), out RaycastHit hit, 10000f))
        {
            var posInTerrain = hit.point - grid.transform.position;
            var posX = Mathf.FloorToInt(posInTerrain.x);
            var posZ = Mathf.FloorToInt(posInTerrain.z);
            if (posX < 0 || posZ < 0 || posX >= terrainData.heightmapWidth - 1 || posZ >= terrainData.heightmapHeight - 1)
                return;
            if (editWalkability.boolValue)
                DrawWalkability(posX, posZ);
            if (paintPrefabs.boolValue)
                PaintPrefab(posX, posZ);
            //var height = terrainData.GetInterpolatedHeight((posX + 0.5f) / terrainData.heightmapWidth, (posZ + 0.5f) / terrainData.heightmapHeight);
            //var height = grid.GetHeight(posX, posZ);
            var height = posInTerrain.y;
            if (Event.current.type == EventType.Repaint)
            {
                DrawLines(posX, posZ, terrainData, grid.transform.position);
                Handles.BeginGUI();
                var prefabs = grid.GetPrefabsAt(posX, posZ);
                var prefabCount = prefabs == null ? 0 : prefabs.Count;
                GUI.Label(new Rect(10, 10, 100, 20), "[" + posX + "," + posZ + "] Height: " + height.ToString("n2") + " Prefabs: " + prefabCount, textStyle);
                Handles.EndGUI();
            }
            if (Event.current.type == EventType.MouseMove || Event.current.type == EventType.MouseDrag)
                SceneView.RepaintAll();
            if (posX != mouseX || posZ != mouseZ)
            {
            }
            mouseX = posX;
            mouseZ = posZ;
        }


    }

    void DrawWalkability(int posX, int posZ)
    {
        Tools.current = Tool.None;
        var control = GUIUtility.GetControlID(GetHashCode(), FocusType.Passive);
        switch (Event.current.type)
        {
            case EventType.MouseDrag:
            case EventType.MouseDown:
            case EventType.MouseUp:
                if (Event.current.button == 0)
                    grid.SetWalkable(posX, posZ, !setUnwalkable.boolValue);
                break;
            case EventType.MouseMove:
                break;
            case EventType.Layout:
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(GetHashCode(), FocusType.Passive));
                break;
            default:
                break;
        }
    }

    void PaintPrefab(int posX, int posZ)
    {
        Tools.current = Tool.None;
        var control = GUIUtility.GetControlID(GetHashCode(), FocusType.Passive);
        switch (Event.current.type)
        {
            case EventType.MouseDrag:
            case EventType.MouseDown:
            case EventType.MouseUp:
                if (Event.current.button == 0)
                {
                    if (prefabAdd.boolValue)
                    {
                        if (paintedPrefab.objectReferenceValue as GameObject == null)
                        {
                            Debug.LogWarning("You must assign a prefab to paint.");
                        }
                        else
                            grid.AddPrefab(posX, posZ, paintedPrefab.objectReferenceValue as GameObject);
                    }
                    else if (prefabReplace.boolValue)
                    {
                        grid.AddPrefab(posX, posZ, paintedPrefab.objectReferenceValue as GameObject, true);
                    }
                    else if (prefabRemove.boolValue)
                    {
                        grid.RemovePrefab(posX, posZ, paintedPrefab.objectReferenceValue as GameObject);
                    }
                }
                break;
            case EventType.MouseMove:
                break;
            case EventType.Layout:
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(GetHashCode(), FocusType.Passive));
                break;
            default:
                break;
        }
    }

    void DrawLines(int posX, int posZ, TerrainData data, Vector3 offset)
    {
        var dist = gridDist.intValue;
        Handles.DrawSolidDisc(new Vector3(posX + 0.5f, grid.GetHeight(posX, posZ), posZ + 0.5f), Vector3.up, 0.15f);
        for (int x = -dist + 1; x < dist + 1; x++)
        {
            for (int z = -dist + 1; z < dist + 1; z++)
            {
                if (posX + x < 0 || posZ + z < 0 || posX + x + 1 >= data.heightmapWidth || posZ + z + 1 >= data.heightmapHeight)
                    continue;
                if (showTerrainLines.boolValue)
                {
                    Handles.color = Color.black;
                    if (x < dist)
                        Handles.DrawDottedLine(new Vector3(posX + x, data.GetHeight(posX + x, posZ + z), posZ + z) + offset,
                            new Vector3(posX + x + 1, data.GetHeight(posX + x + 1, posZ + z), posZ + z) + offset, 2);
                    if (z < dist)
                        Handles.DrawDottedLine(new Vector3(posX + x, data.GetHeight(posX + x, posZ + z), posZ + z) + offset,
                            new Vector3(posX + x, data.GetHeight(posX + x, posZ + z + 1), posZ + z + 1) + offset, 2);
                    Handles.color = Color.white;
                }
                if (x < dist && z < dist && showWalkability.boolValue)
                {

                    var walkable = grid.IsWalkable(posX + x, posZ + z);
                    Handles.color = walkable ? Color.green : Color.red;
                    var middlePos = new Vector3(posX + x + 0.5f, grid.GetHeight(posX + x, posZ + z), posZ + z + 0.5f);
                    var size = walkable ? 1f : 0.95f;
                    Handles.DrawWireCube(middlePos, new Vector3(size, 0, size));
                    /*
                    var middlePos = new Vector2((posX + x + 0.5f) / data.heightmapWidth, (posZ + z + 0.5f) / data.heightmapHeight);
                    var normal = respectProps.boolValue ? Vector3.up : data.GetInterpolatedNormal(middlePos.x, middlePos.y);
                    var pos = respectProps.boolValue ? grid.GetHeight(posX + x, posZ + z) : data.GetInterpolatedHeight(middlePos.x, middlePos.y);
                    Handles.DrawWireArc(new Vector3(posX + x + 0.5f + offset.x, pos + offset.y, posZ + z + 0.5f + offset.z),
                        normal, Vector3.Cross(Vector3.one, normal), 360, 0.45f);*/
                    Handles.color = Color.white;
                }
                if (x < dist && z < dist && showPrefabs.boolValue)
                {
                    var num = grid.GetPrefabsAt(posX + x, posZ + z).Count;
                    if (num > 0)
                    {
                        Handles.color = Color.Lerp(Color.blue, Color.cyan + Color.green, (num - 1) / 4f);
                        Handles.DrawSolidDisc(new Vector3(posX + x + 0.5f, grid.GetHeight(posX + x, posZ + z), posZ + z + 0.5f), Vector3.up, 0.2f);
                        var parent = grid.ShowPrefabsAt(posX + x, posZ + z);
                        foreach (var col in parent.GetComponentsInChildren<Collider>())
                        {
                            col.enabled = false;
                        }
                        Handles.color = Color.white;
                    }
                }
            }
        }
    }
}
