using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* For reference, the stat order for units is
 * 
 *       LVL,    // Level
 *       EXP,   // Current experience
 *       CHP,    // Current Hit points
 *       MHP,    // Max Hit points
 *       CMP,    // Current "Magic" points
 *       MMP,    // Max "Magic" points
 *       ATK,    // Physical/magical attack power
 *       DEF,    // Physical defense
 *       SPR,    // Magical defense
 *       SPD,    // Speed
 *       MOV,    // Move count
 *       JMP,    // Max amount able to change height,
 *       
 */



public class Parameters : MonoBehaviour
{

    [Header("Be sure to assign a job in the Unit component before using")]
    [Header("Base stats should NEVER be affected by anything other than base character growth e.g. level ups")]
    [Header("Only real stats should be affected by equips, skills, etc.")]

    // Base stats and growths are based on current job
    private Job currentJob;

    // Base (unmodified) stats
    [SerializeField]
    private int[] baseStats;
    //    private List<int> baseStats = new List<int>((int)StatTypes.Count);



    // Real (modified) stats
    [SerializeField]
    private int[] realStats;
    //private List<int> realStats = new List<int>((int)StatTypes.Count);

    private List<Modifier>[] statModifiers;

    bool created = false;


    public void Awake()
    {

        // USE PLAYERPREFS FOR LOADING UNIT DATA

        baseStats = new int[(int)StatTypes.Count];
        realStats = new int[(int)StatTypes.Count];
        currentJob = gameObject.GetComponent<UnitData>().UnitJob;

        statModifiers = new List<Modifier>[(int)StatTypes.Count];


        // This means this is a new unit, so it needs new stats
        Initialize();
    }


    public void Start()
    {

    }


    #region Setters and Getters


    // Set modified stat
    public void SetStat(StatTypes parameter, int value)
    {
        int typeIndex = (int)parameter;
        realStats[typeIndex] = value;
    }


    // Set unmodified stat
    public void SetBaseStat(StatTypes parameter, int value)
    {
        int typeIndex = (int)parameter;
        baseStats[typeIndex] = value;
    }


    // Return modified stat
    public int GetStat(StatTypes parameter)
    {
        int typeIndex = (int)parameter;
        return realStats[typeIndex];
    }

    // Return unmodified stat
    public int GetBaseStat(StatTypes parameter)
    {
        int typeIndex = (int)parameter;
        return baseStats[typeIndex];
    }



    // Set stats for a brand new unit
    public void Initialize()
    {
        EXPb = 0;
        EXP = 0;
        LVLb = 1;
        LVL = 1;
        
        
        for (int i = 0; i < Job.statOrder.Length; i++)
        {
            StatTypes parameter = Job.statOrder[i];
            SetBaseStat(parameter, currentJob.GetBaseStat(parameter));
            SetStat(parameter, currentJob.GetBaseStat(parameter));
        }

        CHP = MHP;
        CHPb = MHPb;
        CMP = MMP;
        CMPb = MMPb;
        
    }



    // Should be semi-random, but for now stats grow linearly
    public void LevelUp()
    {
        EXP = EXP + Experience.ExperienceForLevel(LVL) - Experience.ExperienceForLevel(LVL + 1);
        LVL += 1;

        for (int i = 0; i < Job.statOrder.Length; ++i)
        {
            StatTypes parameter = Job.statOrder[i];

            int currentStat = GetBaseStat(parameter);
            int growthStat = currentJob.GetGrowthStat(parameter);
            int newStat = currentStat + growthStat;

            SetStat(parameter, currentStat + growthStat);
            // UpdateRealStat();
        }

        CHP = MHP;
        CMP = MMP;
    }


    public void AddModifier(ModApplication mod)
    {
        int typeIndex = (int)mod.sType;

        statModifiers[typeIndex].Add(new Modifier(mod.Value, mod.mType));
        statModifiers[typeIndex].Sort(ModifierOrder);
    }

    public void RemoveModifier(ModApplication mod)
    {
        int typeIndex = (int)mod.sType;
        Modifier toRemove = new Modifier(mod.Value, mod.mType, (int)mod.mType);

        if(statModifiers[typeIndex].Contains(toRemove))
            statModifiers[typeIndex].Remove(toRemove);

        statModifiers[typeIndex].Sort(ModifierOrder);
    }

    private int ModifierOrder(Modifier a, Modifier b)
    {
        if (a.Order < b.Order)
            return -1;
        else if (a.Order > b.Order)
            return 1;
        else
            return 0;
    }

    // Applies all mods to real stat parameter
    public void ApplyModifiers(StatTypes parameter)
    {
        int typeIndex = (int)parameter;
        float modValue = realStats[typeIndex];

        // Add together any modifiers for that stat
        for (int i = 0; i < statModifiers[typeIndex].Count; i++)
        {
            Modifier mod = statModifiers[typeIndex][i];

            if (mod.Type == StatModType.Flat)
            {
                modValue += statModifiers[typeIndex][i].Value;
            }
            else if (mod.Type == StatModType.Percent)
            {
                modValue *= 1 + mod.Value;
            }
        }
    }


    #endregion


    #region Parameters

    /*
     *      BASE (unmodified) PARAMETERS
     */

    public int LVLb
    {
        get { return GetBaseStat(StatTypes.LVL); }
        set { SetBaseStat(StatTypes.LVL, value); }
    }

    public int EXPb
    {
        get { return GetBaseStat(StatTypes.EXP); }
        set { SetBaseStat(StatTypes.EXP, value); }
    }

    public int CHPb
    {
        get { return GetBaseStat(StatTypes.CHP); }
        set { SetBaseStat(StatTypes.CHP, value); }
    }

    public int MHPb
    {
        get { return GetBaseStat(StatTypes.MHP); }
        set { SetBaseStat(StatTypes.MHP, value); }
    }

    public int CMPb
    {
        get { return GetBaseStat(StatTypes.CMP); }
        set { SetBaseStat(StatTypes.CMP, value); }
    }

    public int MMPb
    {
        get { return GetBaseStat(StatTypes.MMP); }
        set { SetBaseStat(StatTypes.MMP, value); }
    }

    public int ATKb
    {
        get { return GetBaseStat(StatTypes.ATK); }
        set { SetBaseStat(StatTypes.ATK, value); }
    }

    public int DEFb
    {
        get { return GetBaseStat(StatTypes.DEF); }
        set { SetBaseStat(StatTypes.DEF, value); }
    }

    public int SPRb
    {
        get { return GetBaseStat(StatTypes.SPR); }
        set { SetBaseStat(StatTypes.SPR, value); }
    }

    public int SPDb
    {
        get { return GetBaseStat(StatTypes.SPD); }
        set { SetBaseStat(StatTypes.SPD, value); }
    }

    public int MOVb
    {
        get { return GetBaseStat(StatTypes.MOV); }
        set { SetBaseStat(StatTypes.MOV, value); }
    }

    public int JMPb
    {
        get { return GetBaseStat(StatTypes.JMP); }
        set { SetBaseStat(StatTypes.JMP, value); }
    }


    /*
     *      REAL (modified) PARAMETERS
     */

    public int LVL
    {
        get { return GetBaseStat(StatTypes.LVL); }
        set { SetStat(StatTypes.LVL, value); }
    }

    public int EXP
    {
        get { return GetBaseStat(StatTypes.EXP); }
        set { SetStat(StatTypes.EXP, value); }
    }

    public int CHP
    {
        get { return GetStat(StatTypes.CHP); }
        set { SetStat(StatTypes.CHP, value); }
    }

    public int MHP
    {
        get { return GetStat(StatTypes.MHP); }
        set { SetStat(StatTypes.MHP, value); }
    }

    public int CMP
    {
        get { return GetStat(StatTypes.CMP); }
        set { SetStat(StatTypes.CMP, value); }
    }

    public int MMP
    {
        get { return GetStat(StatTypes.MMP); }
        set { SetStat(StatTypes.MMP, value); }
    }

    public int ATK
    {
        get { return GetStat(StatTypes.ATK); }
        set { SetStat(StatTypes.ATK, value); }
    }

    public int DEF
    {
        get { return GetStat(StatTypes.DEF); }
        set { SetStat(StatTypes.DEF, value); }
    }

    public int SPR
    {
        get { return GetStat(StatTypes.SPR); }
        set { SetStat(StatTypes.SPR, value); }
    }

    public int SPD
    {
        get { return GetStat(StatTypes.SPD); }
        set { SetStat(StatTypes.SPD, value); }
    }

    public int MOV
    {
        get { return GetStat(StatTypes.MOV); }
        set { SetStat(StatTypes.MOV, value); }
    }

    public int JMP
    {
        get { return GetStat(StatTypes.JMP); }
        set { SetStat(StatTypes.JMP, value); }
    }



    #endregion
}
