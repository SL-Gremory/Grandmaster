using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Must attach this script to each game object
 */

public class Unit : Selectable
{
    #region Declarations

    [SerializeField]
    string unitName;

    [SerializeField]
    private Job unitJob;

    [SerializeField]
    private int[] unitStats = new int[(int)StatTypes.Count];

    [SerializeField]
    internal Team unitAffiliation; //private

    private Rank unitRank;

    private BattleManager battleManager;
    private BattleNavigate battleNavigate;
    private bool moveIsDone;
    private bool actionIsDone;
    private bool isAttacking;
    Int2 currentUnitPosition = new Int2();


    #endregion

    #region Initialization

    protected override void Awake()
    {
        base.Awake();
        SetBaseStats();     // Temporary thing, should load stats from a file
    }

    void Start()
    {
        battleManager = BattleManager.Instance;
        battleNavigate = gameObject.GetComponentInParent<BattleNavigate>();
        //SetBaseStats();
        StartUnit();
    }

    void StartUnit()
    {
        if (unitAffiliation == Team.HERO)
        {
            BattleManager.Instance.playerUnitCount++;
            if (BattleManager.Instance.isPlayerTurn)
            {
                ReadyUnit();
            }
            else
            {
                ExhaustUnit();
            }
        }
        else if (unitAffiliation == Team.ENEMY)
        {
            BattleManager.Instance.enemyUnitCount++;
            if (!BattleManager.Instance.isPlayerTurn)
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

        if(isAttacking)
        {
            if(Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;


                if(Physics.Raycast(ray, out hit))
                {
                    Unit foundUnit = hit.transform.gameObject.GetComponent<Unit>();

                    if (foundUnit != null)
                    {
                        PrepareAttackOn(foundUnit);
                    }
                }
            }

            if(Input.GetKeyDown(KeyCode.X))
            {
                AttackingPhase();
            }

        }
        else if (isSelected)
        {
            if (!moveIsDone)
            {
                if (battleManager.isPlayerTurn && unitAffiliation == Team.HERO || !battleManager.isPlayerTurn && unitAffiliation == Team.ENEMY)
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

            /*
            //K to kill unit
            if (Input.GetKeyDown(KeyCode.K))
            {
                KillUnit();
            }
            */

            if (Input.GetKeyDown(KeyCode.Z))
            {
                AttackingPhase();
            }


            //Simulate a unit getting damaged
            if (Input.GetKeyDown(KeyCode.F))
            {
              //set CHP to 5 less
              CHP = CHP -5;
              Debug.Log(CHP);
              if (CHP <= 0)
              {
                KillUnit();
              }
            }
          }
    }


    private void AttackingPhase()
    {
        if (!isAttacking)
            Debug.Log("Unit is attacking. Click on a unit to attack or type 'x' to cancel attack");
        else
            Debug.Log("Unit is not attacking");

        isAttacking = !isAttacking;
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
            BattleManager.Instance.unitsDone++;
            Debug.Log("Unit finished acting");
        }
    }

    internal void KillUnit()
    {
        if (unitAffiliation == Team.HERO)
        {
            BattleManager.Instance.playerUnitCount--;
        }
        else if (unitAffiliation == Team.ENEMY)
        {
            BattleManager.Instance.enemyUnitCount--;
        }
		SelectThis(false);
        Destroy(gameObject);
        BattleManager.Instance.CheckWinConditions();
        Debug.Log("Unit has died to death");
    }

    #endregion

    #region Miscellaneous

    private void PrepareAttackOn(Unit defender)
    {
        Int2 aPos = new Int2((int)this.transform.position.x, (int)this.transform.position.y);
        Int2 dPos = new Int2((int)defender.transform.position.x, (int)defender.transform.position.y);

        // Check defender affiliation
        if (this.GetUnitAffiliation() == defender.GetUnitAffiliation())
        {
            Debug.Log("Cannot attack an ally!");
            return;
        }


        if (Int2.Distance(aPos, dPos) > 1)
        {
            Debug.Log("Cannot attack a unit that is out of range!");
            return;
        }
        else
        {
            Debug.Log("Unit is attacking this unit for " + Attack.CalculateProjectedDamage(this, defender));
            Debug.Log("Unit will take " + Attack.CalculateProjectedDamage(defender, this) + " in the process.");

            Attack.CommenceBattle(this, defender);
            Debug.Log(this.GetUnitName() + " is now at " + this.CHP + " HP and " + defender.GetUnitName() + " now has " + defender.CHP);
        }

    }


    public void PrintStats()
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

    #endregion
}
