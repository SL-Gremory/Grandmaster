using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ConversationSO))]
public class ConversationSOEditor : Editor
{
    private void OnEnable()
    {
        (target as ConversationSO).Load();
    }

    public override void OnInspectorGUI()
    {
        GUI.skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene);
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        EditorStyles.label.wordWrap = true;
        var t = target as ConversationSO;
        if (GUILayout.Button("Refresh"))
            t.Load();
        if (!t.Loaded && Event.current.type != EventType.Layout)
        {
            t.Load();
        }
        else if (t.Loaded)
        {
            var actors = serializedObject.FindProperty("actors");
			var voiceActing = serializedObject.FindProperty("voiceActing");
            for (int i = 0; i < actors.arraySize; i++)
            {
                EditorGUILayout.PropertyField(actors.GetArrayElementAtIndex(i));
            }
            if (GUILayout.Button("Add Actor"))
                actors.InsertArrayElementAtIndex(actors.arraySize);
            for (int i = 0; i < t.Conversation.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(t.Conversation[i].speaker + ":");
                EditorGUILayout.LabelField("(" + t.Conversation[i].expression + ")", GUILayout.MaxWidth(50));
				if (voiceActing.arraySize - 1 < i)
					voiceActing.InsertArrayElementAtIndex(i);
				EditorGUILayout.PropertyField(voiceActing.GetArrayElementAtIndex(i),new GUIContent(), GUILayout.MaxWidth(100));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.LabelField(t.Conversation[i].speech);
                GUILayout.Space(10);
            }
        }

        serializedObject.ApplyModifiedProperties();
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Changed conversation.");
        }

        GUI.skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
    }
}
