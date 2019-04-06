using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Map.NodeEventTrigger))]
public class NodeEventTriggerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GUI.skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene);
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("destination"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("hasFlag"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("onTravelTo"));

        serializedObject.ApplyModifiedProperties();
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Changed node event trigger.");
        }

        GUI.skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
    }
}
