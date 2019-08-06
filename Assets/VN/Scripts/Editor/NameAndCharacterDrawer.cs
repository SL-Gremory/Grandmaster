using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(NameAndCharacter))]
public class NameAndCharacterDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);
        position.width *= 0.5f;
        //EditorGUI.indentLevel = 0;
        EditorGUIUtility.labelWidth = 60;
        EditorGUI.PropertyField(position, property.FindPropertyRelative("name"), new GUIContent("Speaker:"));
        position.x += position.width;
        EditorGUIUtility.labelWidth = 20;
        EditorGUI.PropertyField(position, property.FindPropertyRelative("character"), new GUIContent(" is"));
        EditorGUI.EndProperty();
    }
}
