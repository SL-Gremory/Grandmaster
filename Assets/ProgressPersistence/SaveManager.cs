using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSave
{
    // player stats etc..
    public string[] flags;
    public string currentNode; // must be name of GameObject in world map with Node component
    public int currentSequence;
    public List<Unit> currentParty;
    //public Inventory[] inventory;    // Will be added later

}

public static class SaveManager
{
    static string saveName = "save.dat";

    static GameSave currSave = null;

    public static void SetSaveFileName(string name)
    {
        saveName = name;
    }

    public static void UpdateSaveData()
    {
        currSave = currSave ?? new GameSave();

        currSave.flags = PlayerFlags.GetFlags();
        //currSave.currentNode = Map.WorldMap.Instance.CurrentNodeName();
        currSave.currentSequence = EventSequencer.CurrentSequence;
        currSave.currentParty = PartyManager.CurrentParty;
    }

    public static void Save()
    {
        UpdateSaveData();

        var json = JsonUtility.ToJson(currSave, true);
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/saves/");
        System.IO.File.WriteAllText(Application.persistentDataPath + "/saves/" + saveName, json);
        Debug.Log("Saving to " + Application.persistentDataPath + "/saves/" + saveName);
    }

    public static bool IsSaveLoaded() {
        return currSave != null;
    }

    //reload save data into game from the currently open save
    public static void RestoreFromOpenSave()
    {
        if (!IsSaveLoaded())
            return;
        PlayerFlags.LoadFlags(currSave.flags);
        Map.WorldMap.Instance.SetCurrentNode(GameObject.Find(currSave.currentNode).GetComponent<Map.Node>());
        EventSequencer.CurrentSequence = currSave.currentSequence;
        PartyManager.CurrentParty = currSave.currentParty;
    }

    //overrides any data in current save not saved to disk
    public static void LoadFromDisk()
    {
        if (!System.IO.File.Exists(Application.persistentDataPath + "/saves/" + saveName))
            return;
        var json = System.IO.File.ReadAllText(Application.persistentDataPath + "/saves/" + saveName);
        currSave = JsonUtility.FromJson<GameSave>(json);

        RestoreFromOpenSave();

    }
}
