using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Job : MonoBehaviour
{

    #region Fields / Properties
    public static readonly StatTypes[] statOrder = new StatTypes[]
    {
        // StatTypes.LVL,    // Level
        // StatTypes.EXP,    // Accumulated experience
        StatTypes.HP,     // Hit points
        StatTypes.MP,     // "Magic" points
        StatTypes.ATK,    // Physical/magical attack power
        StatTypes.DEF,    // Physical defense
        StatTypes.SPR,    // Magical defense
        StatTypes.SPD     // Speed
        //StatTypes.MOV,    // Move count
    };

    public int[] baseStats = new int[statOrder.Length];
    public float[] growStats = new float[statOrder.Length];
    Stats stats;
    #endregion

    #region Public
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

    public void LoadDefaultStats()
    {
        for (int i = 0; i < statOrder.Length; ++i)
        {
            StatTypes type = statOrder[i];
            stats.SetValue(type, baseStats[i]);
        }
        stats.SetValue(StatTypes.HP, stats[StatTypes.HP]);
        stats.SetValue(StatTypes.MP, stats[StatTypes.MP]);
    }

    #endregion

    #region Event Handlers

    protected virtual void OnLvlChangeNotification(object sender, object args)
    {
        int oldValue = (int)args;
        int newValue = stats[StatTypes.LVL];

        for (int i = oldValue; i < newValue; ++i)
        {
            LevelUp();
        }
    }

    #endregion

    #region Private

    private void LevelUp()
    {
        // Stat growth junk goes here
        // Still not sure about the "scale" of things concerning the stats
        // i.e. will they be in the tens? hundreds? thousands?

        for (int i = 0; i < statOrder.Length; ++i)
        {
            StatTypes type = statOrder[i];
   
            int whole = Mathf.FloorToInt(growStats[i]);
            float fraction = growStats[i] - whole;

            int value = stats[type];
            value += whole;

            if (UnityEngine.Random.value > (1f - fraction))
                value++;


            stats.SetValue(type, value);
        }

        stats.SetValue(StatTypes.HP, stats[StatTypes.HP]);
        stats.SetValue(StatTypes.MP, stats[StatTypes.MP]);












    }

    #endregion
}
