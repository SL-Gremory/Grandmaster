using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// set as scriptable object?
// might be easier than creating whole prefabs for each job

[CreateAssetMenu(menuName = "Grandmaster Job")]
public class Job : ScriptableObject
{

    public static readonly StatTypes[] statOrder = new StatTypes[]
    {
        // StatTypes.LVL,    // Level
        // StatTypes.EXP,    // Accumulated experience
        StatTypes.MHP,     // Hit points
        StatTypes.MMP,     // "Magic" points
        StatTypes.ATK,    // Physical/magical attack power
        StatTypes.DEF,    // Physical defense
        StatTypes.SPR,    // Magical defense
        StatTypes.SPD,     // Speed
        StatTypes.MOV,    // Move count
        StatTypes.JMP
        //StatTypes.Count
    };

    public int[] baseStats = new int[statOrder.Length];
    public int[] statGrowths = new int[statOrder.Length];

    Stats stats;


    public int GetBaseStat(StatTypes parameter)
    {
        int statIndex = IndexForStat(parameter);

        if(statIndex != -1)
        {
            return baseStats[statIndex];
        } else
        {
            return 0;
        }
    }

    public int GetGrowthStat(StatTypes parameter)
    {
        int statIndex = IndexForStat(parameter);

        if(statIndex != -1)
        {
            return statGrowths[statIndex];
        } else
        {
            return 0;
        }
    }

    public void SetBaseStat(StatTypes parameter, int value)
    {
        int statIndex = IndexForStat(parameter);

        if (statIndex != -1)
        {
            baseStats[statIndex] = value;
        }
    }

    public void SetStatGrowth(StatTypes parameter, int value)
    {
        int statIndex = IndexForStat(parameter);

        if (statIndex != -1)
        {
            statGrowths[statIndex] = value;
        }
    }

    private int IndexForStat(StatTypes parameter)
    {
        /*
        if(type == StatTypes.HP)
        {
            return 0;
        }
        */

        switch(parameter)
        {
            case StatTypes.MHP:
                return 0;
            case StatTypes.MMP:
                return 1;
            case StatTypes.ATK:
                return 2;
            case StatTypes.DEF:
                return 3;
            case StatTypes.SPR:
                return 4;
            case StatTypes.SPD:
                return 5;
            case StatTypes.MOV:
                return 6;
            case StatTypes.JMP:
                return 7;
        }
        return -1;
    }


    /*
    public void Employ()
    {
        stats = gameObject.GetComponentInParent<Stats>();
        Feature[] features = GetComponentsInChildren<Feature>();
        for (int i = 0; i < features.Length; ++i)
        {
            features[i].Activate(gameObject);
        }
    }

    public void UnEmploy()
    {
        Feature[] features = GetComponentsInChildren<Feature>();
        for (int i = 0; i < features.Length; ++i)
        {
            features[i].Deactivate();
        }
        stats = null;
    }
    */


}
