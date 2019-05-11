using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Must attach this script to each game object
 */

public class Unit2 : Selectable
{
    #region Declarations

    [SerializeField]
    string unitName;

    [SerializeField]
    private Job unitJob;

    [SerializeField]
    private int[] unitStats = new int[(int)StatTypes.Count];

    [SerializeField]
    private Team unitAffiliation;

    private Rank unitRank;

    private TurnManager turnManager;
    private BattleNavigate battleNavigate;

    [SerializeField] internal bool isAlly; //must define as true or false in editor, on prefab, or when spawning object

    public bool IsAlly { get { return isAlly; } }

    private bool moveIsDone;
    private bool actionIsDone;
    Int2 currentUnitPosition = new Int2();


    #endregion

    #region Initialization
    private void Awake()
    {
        SetBaseStats();     // Temporary thing, should load stats from a file

    }

    void Start()
    {
        turnManager = TurnManager.Instance;
        battleNavigate = gameObject.GetComponentInParent<BattleNavigate>();
        StartUnit();
    }

    void StartUnit()
    {
        if (isAlly)
        {
            TurnManager.Instance.playerUnitCount++;
            if (TurnManager.Instance.isPlayerTurn)
            {
                ReadyUnit();
            }
            else
            {
                ExhaustUnit();
            }
        }
        else
        {
            TurnManager.Instance.enemyUnitCount++;
            if (!TurnManager.Instance.isPlayerTurn)
            {
                ReadyUnit();
            }
            else
            {
                ExhaustUnit();
            }
        }
    }


    #endregion

    #region Parameters

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

    #region Setters and Getters

    public string GetUnitName()
    {
        return unitName;
    }

    public Team GetUnitAffiliation()
    {
        return unitAffiliation;
    }

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
        for (int i = 0; i < Job.statOrder.Length; ++i)
        {
            StatTypes parameter = Job.statOrder[i];
            SetStat(parameter, unitJob.GetBaseStat(parameter));
        }

        LVL = 1;
        CHP = MHP;
        CMP = MMP;
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


    #endregion

    #region Unit State Controls

    void Update()
    {
        if (isSelected)
        {
            //M to simulate unit moving
            //if (Input.GetKeyDown(KeyCode.M) && !moveIsDone)
            if (!moveIsDone)
            {
                if (turnManager.isPlayerTurn && isAlly || !turnManager.isPlayerTurn && !isAlly)
                {
                    battleNavigate.Move();
                }
            }

            //ESCAPE to deselect unit
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SelectThis(false);
                Debug.Log("Deselected a selectable via esc");
            }

            //SPACE to simulate unit performing an action
            if (Input.GetKeyDown(KeyCode.Space) && !actionIsDone)
            {
                DoneActing(); // Call everytime at the very end after attacking as well
            }

            //K to kill unit
            if (Input.GetKeyDown(KeyCode.K))
            {
                KillUnit();
            }
        }
    }



    internal void ReadyUnit()
    {
        moveIsDone = false;
        actionIsDone = false;
        ChangeColor(0);
    }

    internal void ExhaustUnit()
    {
        moveIsDone = true;
        actionIsDone = true;
        ChangeColor(2);
    }

    internal void DoneMoving()
    {
        if (!moveIsDone)
        {
            moveIsDone = true;
            ChangeColor(1);
            Debug.Log("Unit finished moving");
        }
    }

    internal void DoneActing()
    {
        if (!actionIsDone)
        {
            ExhaustUnit();
            TurnManager.Instance.unitsDone++;
            Debug.Log("Unit finished acting");
        }
    }

    internal void KillUnit()
    {
        if (isAlly)
        {
            TurnManager.Instance.playerUnitCount--;
        }
        else
        {
            TurnManager.Instance.enemyUnitCount--;
        }
        Destroy(gameObject);
        TurnManager.Instance.CheckWinConditions();
        Debug.Log("Unit has died to death");
    }

    #endregion


    #region Miscellaneous

    /*
    private void PrepareAttackOn(Unit defender)
    {

        Int2 aPos = new Int2(
                    (int)Mathf.Floor(this.transform.position.x),
                    (int)Mathf.Floor(this.transform.position.z));

        Int2 dPos = new Int2(
                    (int)Mathf.Floor(defender.transform.position.x),
                    (int)Mathf.Floor(defender.transform.position.z));

        if (this.unitAffiliation == defender.unitAffiliation)
        {
            Debug.Log("Cannot attack an ally!");
            return;
        }

        if (Int2.Distance(aPos, dPos) > 1)
        {
            Debug.Log("Cannot attack a unit that is out of range!");
            return;
        }

        Debug.Log("Unit is attacking this unit for " + Attack.CalculateProjectedDamage(this, defender) + " damage but will take " +
            Attack.CalculateProjectedDamage(defender, this) + " damage.");

        Attack.CommenceBattle(this, defender);
        Debug.Log(this.unitName + " is now at " + this.CHP + " HP and " + defender.unitName + " now has " + defender.CHP);
    }*/

    #endregion

}

