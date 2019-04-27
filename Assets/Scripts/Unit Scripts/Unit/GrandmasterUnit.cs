using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


// Add this script as a component to an object then attach a Job using one of the files in the Units/Job folder

public class GrandmasterUnit : MonoBehaviour
{
    [SerializeField]
    string unitName;

    [SerializeField]
    private Job unitJob;

    [SerializeField]
    private int[] unitStats = new int[(int)StatTypes.Count];

    [SerializeField]
    private Team unitAffiliation;

    private Rank unitRank;

   
    #region Manager-related functions

    // Indicator that indicates if unit is done or not done with a turn which is done using an indification variable indicating that it is done or not done with a turn
    bool turnStatus;

    public bool GetTurnStatus()
    {
        return turnStatus;
    }

    public void SetTurnStatus(bool status)
    {
        turnStatus = status;
    }

    public void toggleStatus()
    {
        turnStatus = !turnStatus;
    }

    #endregion


    void Start()
    {
        turnStatus = true;
        SetBaseStats();


        SingletonManagerTest.herosOnTheField.Add(this);
        Debug.Log("Added self to manager");
        /*
        if (unitAffiliation == Team.HERO)
        {
            SingletonManagerTest.herosOnTheField.Add(this);
        } else if (unitAffiliation == Team.ENEMY)
        {
            SingletonManagerTest.enemiesOnTheField.Add(this);   
        }
        */
    }

    private void Update()
    {

    }

    public string GetUnitName()
    {
        return unitName;
    }

 
    public Team GetUnitAffiliation()
    {
        return unitAffiliation;
    }

    #region Parameter fields

    // Level
    public int LVL
    {
        get { return GetStat(StatTypes.LVL); }
        set { SetStat(StatTypes.LVL, value); }
    }

    // Experience
    public int EXP
    {
        get { return GetStat(StatTypes.EXP); }
        set { SetStat(StatTypes.EXP, value); }
    }

    // Current Hit Points
    public int CHP
    {
        get { return GetStat(StatTypes.CHP); }
        set { SetStat(StatTypes.CHP, value); }
    }

    // Max Hit Points
    public int MHP
    {
        get { return GetStat(StatTypes.MHP); }
        set { SetStat(StatTypes.MHP, value); }
    }

    // Current Magic Points
    public int CMP
    {
        get { return GetStat(StatTypes.CMP); }
        set { SetStat(StatTypes.CMP, value); }
    }

    // Max Magic Points
    public int MMP
    {
        get { return GetStat(StatTypes.MMP); }
        set { SetStat(StatTypes.MMP, value); }
    }

    // Attack
    public int ATK
    {
        get { return GetStat(StatTypes.ATK); }
        set { SetStat(StatTypes.ATK, value); }
    }

    // Defense
    public int DEF
    {
        get { return GetStat(StatTypes.DEF); }
        set { SetStat(StatTypes.DEF, value); }
    }

    // Spirit
    public int SPR
    {
        get { return GetStat(StatTypes.SPR); }
        set { SetStat(StatTypes.SPR, value); }
    }

    // Speed
    public int SPD
    {
        get { return GetStat(StatTypes.SPD); }
        set { SetStat(StatTypes.SPD, value); }
    }

    // Movement
    public int MOV
    {
        get { return GetStat(StatTypes.MOV); }
        set { SetStat(StatTypes.MOV, value); }
    }

    // Jump
    public int JMP
    {
        get { return GetStat(StatTypes.JMP); }
        set { SetStat(StatTypes.JMP, value); }
    }

    #endregion

    public void SetStat(StatTypes parameter, int value)
    {
        int typeIndex = (int)parameter;
        unitStats[typeIndex] = value;
    }

    public int GetStat(StatTypes parameter)
    {
        int typeIndex = (int)parameter;
        return unitStats[typeIndex];
    }

    public void SetBaseStats()
    {
        LVL = 1;
        CHP = MHP;
        CMP = MMP;

        for (int i = 0; i < Job.statOrder.Length; ++i)
        {
            StatTypes parameter = Job.statOrder[i];
            SetStat(parameter, unitJob.GetBaseStat(parameter));
        }
    }

    public void LevelUp()
    {
        LVL++;

        for (int i = 0; i < Job.statOrder.Length; ++i)
        {
            StatTypes parameter = Job.statOrder[i];

            int currentStat = GetStat(parameter);
            int growthStat = unitJob.GetGrowthStat(parameter);
            int newStat = currentStat + growthStat;

            SetStat(parameter, currentStat + growthStat);
        }

        CHP = MHP;
        CMP = MMP;
    }


    // Semi-random level-up (WIP)
    public void variableLevelUp()
    {

        LVL++;

        for (int i = 0; i < Job.statOrder.Length; ++i)
        {
            StatTypes parameter = Job.statOrder[i];

            int currentStat = GetStat(parameter);
            int growthStat = unitJob.GetGrowthStat(parameter);
            int newStat = currentStat + growthStat;

            SetStat(parameter, currentStat + growthStat);
        }

        CHP = MHP;
        CMP = MMP;
    }




    void printStats()
    {
        Debug.Log(string.Format("HP:{0}  MP:{1}  ATK:{2}  DEF:{3}  SPR:{4}  SPD:{5}",
            GetStat(Job.statOrder[0]),
            GetStat(Job.statOrder[1]),
            GetStat(Job.statOrder[2]),
            GetStat(Job.statOrder[3]),
            GetStat(Job.statOrder[4]),
            GetStat(Job.statOrder[5])
        ));
    }
}
