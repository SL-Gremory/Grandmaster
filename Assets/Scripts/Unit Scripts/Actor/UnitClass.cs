using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitClass : MonoBehaviour
{
    #region Fields / Properties
    public static readonly StatTypes[] statOrder = new StatTypes[]
    {
        // StatTypes.LVL,
        StatTypes.HP,
        // StatTypes.EXP,
        StatTypes.MP,
        StatTypes.ATK,
        StatTypes.DEF,
        StatTypes.SPR,
        StatTypes.SPD
        // StatTypes.MOV
    
    };

    public int[] baseStats = new int[statOrder.Length];
    public float[] growStats = new float[statOrder.Length];
    Stats stats;
    #endregion

    #region Public
    public void Employ()
    {
        stats = gameObject.GetComponentInParent<Stats>();
       // Feature[] features
    }

    #endregion

}
