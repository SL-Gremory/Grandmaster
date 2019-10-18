using UnityEngine;
using UnityEditor;
using System;


enum StatNameOrder {
    LVL,
    EXP,
    CHP,
    MHP,
    CMP,
    MMP,
    ATK,
    DEF,
    SPR,
    SPD,
    MOV,
    JMP
};


[CustomEditor(typeof(Parameters))]
public class ParametersEditor : Editor
{

    int count = (int)StatTypes.Count;

    public void ShowArrayProperty(SerializedProperty stats, string header)
    {
        EditorGUILayout.LabelField(header, EditorStyles.boldLabel);
        EditorGUI.indentLevel += 1;
        if (stats.arraySize == 0)
        {
            return;
        }

        for (int i = 0; i < count; i++)
        {
            try
            {
                EditorGUILayout.PropertyField(stats.GetArrayElementAtIndex(i), new GUIContent(Enum.GetName(typeof(StatNameOrder), i)));
            }
            catch(Exception e)
            {

            }
        }
        EditorGUI.indentLevel -= 1;
    }

    public override void OnInspectorGUI()
    {

        //ShowArrayProperty(serializedObject.FindProperty("baseStats"), "Base Stats");
        ShowArrayProperty(serializedObject.FindProperty("realStats"), "Real Stats");
    }
}