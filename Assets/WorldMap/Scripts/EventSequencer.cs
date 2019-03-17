using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[DisallowMultipleComponent]
public class EventSequencer : MonoBehaviour
{
    public static int CurrentSequence { get; set; }

    [SerializeField]
    UnityEngine.Object[] sequences; //in inspector check whether they have ISequence

    Coroutine coroutine;

    public void Begin() {
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(SequenceCheckingCoroutine());
    }

    IEnumerator SequenceCheckingCoroutine() {
        yield return null;
        while (CurrentSequence > -1 && CurrentSequence < sequences.Length) {
            CurrentSequence++;
            Debug.Log("Beginning sequence " + (CurrentSequence - 1));
            SaveManager.UpdateSaveData();
            (sequences[CurrentSequence-1] as ISequence).BeginSequence(); //this may change scenes and stop execution of the coroutine
            yield return null;
            while (!(sequences[CurrentSequence - 1] as ISequence).HasSequenceEnded()) {
                yield return null;
            }
            Debug.Log("Ending sequence " + (CurrentSequence - 1));
        }
    }

}


#if UNITY_EDITOR
[CustomEditor(typeof(EventSequencer))]
public class EventSequencerEditor : Editor {

    SerializedProperty sequences;

    private void OnEnable()
    {
        sequences = serializedObject.FindProperty("sequences");
    }

    public override void OnInspectorGUI()
    {
        GUI.skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene);
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        for (int i = 0; i < sequences.arraySize; i++)
        {
            var item = sequences.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(item);
            var seq = item.objectReferenceValue;
            if (seq != null && !(seq is ISequence)) {
                if (seq is GameObject)
                {
                    item.objectReferenceValue = (seq as GameObject).GetComponent<ISequence>() as Component;
                    if (item.objectReferenceValue == null)
                        Debug.LogWarning("No component on this GameObject implements ISequence interface.");
                }
                else {
                    item.objectReferenceValue = null;
                    Debug.LogWarning("A Sequence must implement ISequence interface.");
                }
            }
        }
        if (GUILayout.Button("Add Sequence"))
        {
            sequences.InsertArrayElementAtIndex(sequences.arraySize);
        }

        serializedObject.ApplyModifiedProperties();
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Changed sequences.");
        }

        GUI.skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
    }
}
/*
//for checking sequences, sorry Object bro
[CustomPropertyDrawer(typeof(UnityEngine.Object))]
public class ObjectDrawer : PropertyDrawer
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
}*/
#endif
