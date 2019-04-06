using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Map.NodeAndFlag))]
public class NodeAndFlagDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);
        position.width *= 0.5f;
        //EditorGUI.indentLevel = 0;
        EditorGUIUtility.labelWidth = 38;
        EditorGUI.PropertyField(position, property.FindPropertyRelative("node"), new GUIContent("Node:", "Node accessible from this node."));
        position.x += position.width;
        EditorGUI.PropertyField(position, property.FindPropertyRelative("flag"), new GUIContent("Flag:", "Flag needed to travel there."));
        EditorGUI.EndProperty();
    }
}
