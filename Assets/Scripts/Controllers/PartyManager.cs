using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Temporary thing. Should probably have a generic class tracking a battle's active units instead
using Party = System.Collections.Generic.List<UnityEngine.GameObject>;

// Should be created at the start of the game.
// This class keeps track of the players available units, their experience, equipment, etc.
public class PartyManager : MonoBehaviour
{

    public static PartyManager Instance { get; private set; }
    public static List<Unit> CurrentParty;
    int slotCount = 3;
    const float minLevelBonus = 1.5f;
    const float maxLevelBonus = 0.5f;
    List<Unit> _unitsInScene;

    private PartyManager()
    {

    }

    private void Start()
    {
        if (CurrentParty == null)
            CurrentParty = new List<Unit>();


        // This is where I should load from save file the player's party

        // Find potentially new player units in the scene
        _unitsInScene = GameObject.FindObjectsOfType<Unit>().ToList();
        foreach (Unit u in _unitsInScene)
        {
            if (!CurrentParty.Contains(u) && u.GetUnitAffiliation() == Team.HERO)
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
        CurrentParty.Add(u);
    }

    public static void RemoveFromParty(Unit u)
    {
        CurrentParty.Remove(u);
    }


    public static void RewardExperience(int prize, Party activeParty)
    {
        List<Unit> partyMembers = new List<Unit>(activeParty.Count);

        // Obtain active party members
        for (int i = 0; i < activeParty.Count; i++)
        {
            Unit u = activeParty[i].GetComponent<Unit>();

            if (u != null)
                partyMembers.Add(u);
        }

        int min = Consts.GAME_MIN_LEVEL;
        int max = Consts.GAME_MAX_LEVEL;

        // Determine the range of levels in party
        for (int i = partyMembers.Count - 1; i >= 0; i--)
        {
            min = Mathf.Min(partyMembers[i].LVL, min);
            max = Mathf.Max(partyMembers[i].LVL, max);
        }

        // Weight awarded exp based on individual level
        float[] weights = new float[activeParty.Count];
        float summed = 0;
        for (int i = partyMembers.Count; i >= 0; i--)
        {
            float percent = (float)(partyMembers[i].LVL - min) / (float)(max - min);
            weights[i] = Mathf.Lerp(minLevelBonus, maxLevelBonus, percent);
            summed += weights[i];
        }

        // Distribute weighted experience
        for (int i = partyMembers.Count; i >= 0; i--)
        {
            if (partyMembers[i].LVL != Consts.GAME_MAX_LEVEL)
            {
                partyMembers[i].EXP += Mathf.FloorToInt((weights[i] / summed) * prize);

                // Check if distributed experience causes unit to level up
                int totalAccumulatedExp = partyMembers[i].EXP + Experience.ExperienceForLevel(partyMembers[i].LVL);
                if (totalAccumulatedExp >= Experience.ExperienceForLevel(partyMembers[i].EXP + 1))
                {
                    partyMembers[i].LevelUp();
                }
            }
        }
    }
}
     