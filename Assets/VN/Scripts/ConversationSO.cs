﻿using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public static class JsonHelper
{

    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = UnityEngine.JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return UnityEngine.JsonUtility.ToJson(wrapper, true);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}

[System.Serializable]
public class Speech
{
    public string speaker;
    public string expression = "base";
    public string speech;
}

[System.Serializable]
public class NameAndCharacter
{
    public string name;
    public CharacterSO character;
}

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue <--", order = 1)]
public class ConversationSO : ScriptableObject, ISequence
{
    [SerializeField]
    NameAndCharacter[] actors;
	[SerializeField]
	AudioClip[] voiceActing;
	public AudioClip[] VoiceActing { get => voiceActing; }

    public bool Loaded { get; private set; }
    public NameAndCharacter[] Actors { get => actors; }
    [SerializeField]
    Speech[] conversation;
    public Speech[] Conversation { get { return conversation; } }

    public void Load()
    {
#if UNITY_EDITOR
        var relPath = AssetDatabase.GetAssetPath(this);
        if (relPath == null || relPath.Length == 0)
            return;
        var jsonPath = Application.dataPath.Remove(Application.dataPath.Length - 6) + relPath.Remove(relPath.Length - 6) + ".json";
        if (!File.Exists(jsonPath))
        {
            conversation = new Speech[] { new Speech { speaker = "Untitled", speech = "New speech. Hello World!" } };
            var json = JsonHelper.ToJson(conversation);
            File.WriteAllText(jsonPath, json);
            AssetDatabase.Refresh();
            Loaded = true;
        }
        else
        {
            var json = File.ReadAllText(jsonPath);
            conversation = JsonHelper.FromJson<Speech>(json);
            AssetDatabase.Refresh();
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            Loaded = true;
        }
#endif
    }

    private void OnDisable()
    {
        Loaded = false;
    }

    public CharacterSO GetCharacterForName(string name)
    {
        var ind = System.Array.FindIndex(actors, item => item.name.Equals(name));
        if (ind == -1)
            return null;
        return actors[ind].character;
    }

    public void BeginSequence()
    {
        DialogueManager.Instance.StartConversation(this);
    }

    public bool HasSequenceEnded()
    {
        return !DialogueManager.Instance.IsInDialogue;
    }
}
