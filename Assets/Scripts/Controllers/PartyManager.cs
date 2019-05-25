using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PartyManager : MonoBehaviour
{

    //public static List<CharacterData> CurrentParty;
    public static List<Unit> CurrentParty;

    List<Unit> _unitsInScene;

    private void Start()
    {
        if (CurrentParty == null)
            CurrentParty = new List<Unit>();


        // This is where I should load from save file the player's party

        // Find potentially new player units in the scene
        _unitsInScene = GameObject.FindObjectsOfType<Unit>().ToList();
        foreach (Unit u in _unitsInScene)
        {
            if (!CurrentParty.Contains(u) && u.GetUnitAffiliation () == Team.HERO)
                AddToParty(u);
        }

    }

    private void Update()
    {
        // JUST USED FOR TESTING
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("Here we go lads");
            SaveManager.Save();
        }
    }


    public static void AddToParty(Unit u)
    {
        //CurrentParty.Add(new CharacterData(u, Guid.NewGuid()));
        CurrentParty.Add(u);
    }



    public static void UpdateParty()
    {

    }


}
