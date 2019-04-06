using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Map.Node))]
public class NodeEditor : Editor
{
    SerializedProperty nodes;

    private void OnEnable()
    {
        nodes = serializedObject.FindProperty("travelNodes");
    }

    public override void OnInspectorGUI()
    {
        GUI.skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene);
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        for (int i = 0; i < nodes.arraySize; i++)
        {
            EditorGUILayout.PropertyField(nodes.GetArrayElementAtIndex(i));
        }
        /*
        for (int i = 0; i < nodes.arraySize; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Node:", GUILayout.MaxWidth(48));
            EditorGUILayout.PropertyField(nodes.GetArrayElementAtIndex(i).FindPropertyRelative("node"), GUIContent.none, GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField("Flag:", GUILayout.MaxWidth(30));
            EditorGUILayout.PropertyField(nodes.GetArrayElementAtIndex(i).FindPropertyRelative("flag"), GUIContent.none, GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();
        }*/
        if (GUILayout.Button("Add Destination"))
        {
            nodes.InsertArrayElementAtIndex(nodes.arraySize);
        }

        serializedObject.ApplyModifiedProperties();
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Changed destination nodes.");
        }

        GUI.skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
    }
}
