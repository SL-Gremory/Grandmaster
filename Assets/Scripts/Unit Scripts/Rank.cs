using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// The experience model being used here is based
// on Fire Emblem Heroes's model. Needs testing

public class Rank : MonoBehaviour
{
    #region Consts

    public const int minLevel = 1;
    public const int maxLevel = 40;         // THIS MIGHT CHANGE
    public const int maxTotalExp = 40127;   // THIS MIGHT CHANGE

    #endregion

    #region Fields & Properties
    public int LVL
    {
        get { return stats[StatTypes.LVL]; }
    }

    public int EXP
    {
        get { return stats[StatTypes.EXP]; }
        set { stats[StatTypes.EXP] = value;  }
    }

    public float LevelPercent
    {
        get { return (float)(LVL - minLevel) / (float)(maxLevel - minLevel); }
    }

    Stats stats;
    #endregion

    #region MonoBehaviour
    void Awake()
    {
        stats = GetComponent<Stats>();
    }
    #endregion

    #region Event Handlers
    void OnExpChanged(object sender, object args)
    {
        stats.SetValue(StatTypes.LVL, LevelForExperience(EXP));
    }
    #endregion

    #region Public

    // This returns the level a unit would be if they had some total of accumulated experience
    public static int LevelForExperience(int exp)
    {
        int level = maxLevel;
        
        while(level >= minLevel)
        {
            if (exp >= ExperienceForLevel(level))
                break;
            --level;
        }
        return level;
    }

    // This returns the grand total number of accumulated experience required to reach a certain level
    public static int ExperienceForLevel(int level)
    {
        float levelPercent = Mathf.Clamp01((float)(level - minLevel) / (float)(maxLevel - minLevel));
        int totalExperience = (int)EasingEquations.EaseInQuad(0, maxTotalExp, levelPercent);
        return totalExperience;
    }

    public void Init(int level)
    {
        stats.SetValue(StatTypes.LVL, level);
        stats.SetValue(StatTypes.EXP, ExperienceForLevel(level));
    }
    #endregion
}
