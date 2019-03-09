using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GameSave
{
    // player stats etc..
    public string[] flags;
    public string currentNode; // must be name of GameObject in world map with Node component
    public int currentSequence;
}

public static class SaveManager
{
    static string saveName = "default"; 

    public static void SetSaveFileName(string name)
    {
        saveName = name;
    }

    public static void Save()
    {
        var save = new GameSave();

        save.flags = PlayerFlags.GetFlags();
        save.currentNode = Map.WorldMap.Instance.CurrentNodeName();
        save.currentSequence = EventSequencer.CurrentSequence;


        var json = JsonUtility.ToJson(save, true);
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/saves/");
        System.IO.File.WriteAllText(Application.persistentDataPath + "/saves/" + saveName, json);
    }

    public static void Load()
    {
        if (!System.IO.File.Exists(Application.persistentDataPath + "/saves/" + saveName))
            return;
        var json = System.IO.File.ReadAllText(Application.persistentDataPath + "/saves/" + saveName);
        var save = JsonUtility.FromJson<GameSave>(json);

        PlayerFlags.LoadFlags(save.flags);
        Map.WorldMap.Instance.SetCurrentNode(GameObject.Find(save.currentNode).GetComponent<Map.Node>());
        EventSequencer.CurrentSequence = save.currentSequence;

    }
}
