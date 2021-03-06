﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// set as scriptable object?
// might be easier than creating whole classes

[CreateAssetMenu(menuName = "Grandmaster Job")]
public class Job : ScriptableObject
{
    [SerializeField]
    internal List<WeaponType> weaponPermissions;

    public static readonly StatTypes[] statOrder = new StatTypes[]
    {
        StatTypes.MHP,     // Hit points
        StatTypes.MMP,     // "Magic" points
        StatTypes.ATK,    // Physical/magical attack power
        StatTypes.DEF,    // Physical defense
        StatTypes.SPR,    // Magical defense
        StatTypes.SPD,     // Speed
        StatTypes.MOV,    // Move count
        StatTypes.JMP
    };

    [Header("Base Stats")]
    public int[] baseStats = new int[statOrder.Length];

    [Header("Stat Growths")]
    public int[] statGrowths = new int[statOrder.Length];

    // NEED TO CREATE EDITOR FOR 2D ARRAY
    public int[,] statMilestones = new int[statOrder.Length, 10];



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

}
