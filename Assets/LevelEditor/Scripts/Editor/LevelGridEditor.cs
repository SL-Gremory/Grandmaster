using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelGrid))]
public class LevelGridEditor : Editor
{
    SerializedProperty terrainTypes;
    SerializedProperty angle;
	SerializedProperty useTerrain;
    SerializedProperty useProps;
    SerializedProperty usePrefabs;

    private void OnEnable()
    {
		terrainTypes = serializedObject.FindProperty("terrainTypes");
        angle = serializedObject.FindProperty("setUnwalkableAngle");
		useTerrain = serializedObject.FindProperty("useTerrain");
        useProps = serializedObject.FindProperty("useProps");
        usePrefabs = serializedObject.FindProperty("usePrefabs");

    }

    public override void OnInspectorGUI()
    {
        GUI.skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene);

        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
		EditorGUILayout.PropertyField(terrainTypes);
        EditorGUILayout.LabelField("Use in edit mode to keep changes.");
        EditorGUILayout.IntSlider(angle, 0, 90);
        EditorGUILayout.BeginHorizontal();
        useTerrain.boolValue = GUILayout.Toggle(useTerrain.boolValue, "Use Terrain", "Button");
        useProps.boolValue = GUILayout.Toggle(useProps.boolValue, "Use Props", "Button");
        usePrefabs.boolValue = GUILayout.Toggle(usePrefabs.boolValue, "Use Prefabs", "Button");
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Sample Terrain"))
        {
            (target as LevelGrid).SampleHeights();
        }


        serializedObject.ApplyModifiedProperties();
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Resampled heights or changed angle");
        }

        GUI.skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
    }
}
