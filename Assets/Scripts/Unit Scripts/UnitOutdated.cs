﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 *  Must attach this script to each game object
 */
 /*
public class UnitOutdated : Selectable
{

    #region Declarations

    private TurnManager turnManager;
    private BattleNavigate battleNavigate;

    [SerializeField]
    private string unitName;
    [SerializeField]
    private Job unitJob;
    [SerializeField]
    private List<int> unitStats = new List<int>((int)StatTypes.Count);
    //private int[] unitStats = new int[(int)StatTypes.Count];

    

    private List<Modifier>[] statModifiers = new List<Modifier>[(int)StatTypes.Count];

    [SerializeField]
    internal Team unitAffiliation; //private

    // UI-related things
    SpriteRenderer renderer;
    UnitInfoUI unitInfo;

    // Booleans
    private bool moveIsDone;
    private bool actionIsDone;
    private bool isAttacking;


    // Rocky Hp bar code stuff
    private HPScript hpBar;

    // List of weapons the character is allowed to wield
    [SerializeField]
    internal List<WeaponType> weaponPermissions;

    // Equipment & Skills
    Weapon weapon;
    List<Skill> skills = new List<Skill>();



    #endregion

    #region Initialization

    // Create a new unit using attached GrandmasterJob asset
    public Unit()
    {
        //SetBaseStats();
    }

    // Copy existing unit
    public Unit(Unit u)
    {
        unitName = u.unitName;
        unitAffiliation = u.unitAffiliation;
        LVL = u.LVL;
        EXP = u.EXP;
        CHP = u.CHP;
        MHP = u.MHP;
        CMP = u.CMP;
        MMP = u.MMP;
        ATK = u.ATK;
        DEF = u.DEF;
        SPR = u.SPR;
        SPD = u.SPD;
        MOV = u.MOV;
        JMP = u.JMP;
    }
    
    public override void Awake()
    {
        base.Awake();
        SetBaseStats();     // Temporary thing, should load stats from a file
        hpBar = GetComponentInChildren<HPScript>(); // Rocky HP bar stuff
    }

    void Start()
    {
        // Ronald: This is a bad way of finding a GO in a different scene. Should change this later
        unitInfo = GameObject.Find("UnitInfoUIText").GetComponent<UnitInfoUI>();
        renderer = GetComponent<SpriteRenderer>();
        turnManager = TurnManager.Instance;
        battleNavigate = gameObject.GetComponentInParent<BattleNavigate>();

        StartUnit();
        hpBar.Start();
    }

    void StartUnit()
    {
        if (unitAffiliation == Team.HERO)
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
        else if (unitAffiliation == Team.ENEMY)
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

    // Current Experience
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

    // Probably shouldn't calculate modded values each time
    public int GetStat(StatTypes parameter)
    {
        int typeIndex = (int)parameter;
        float modValue = unitStats[typeIndex];

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

        return (int)Math.Floor(modValue);
    }

    public void AddModifier(ModApplication mod)
    {
        int typeIndex = (int)mod.sType;

        statModifiers[typeIndex].Add(new Modifier(mod.Value, mod.mType));
        statModifiers[typeIndex].Sort(ModifierOrder);
    }

    public void RemoveModifier(Modifier mod)
    {
        int typeIndex = (int)mod.Type;

        // FIX THIS
        statModifiers[typeIndex].Remove(mod);
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


    public void SetBaseStats()
    {

        EXP = 0;
        LVL++;

        for (int i = 0; i < Job.statOrder.Length; ++i)
        {
            StatTypes parameter = Job.statOrder[i];
            SetStat(parameter, unitJob.GetBaseStat(parameter));
        }

        CHP = MHP;
        CMP = MMP;
    }

    public void LevelUp()
    {
        EXP = EXP + Experience.ExperienceForLevel(LVL) - Experience.ExperienceForLevel(LVL + 1);
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
        // HP bar UI functions
        if (hpBar._localScale.x > (float)CHP / (float)MHP)
        {
            hpBar.ChangeLocalScale((float)CHP / (float)MHP);
        }

        // Unit is in attacking phase
        if (isAttacking)
        {
            Unit foundUnit = null;

            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 1000f))
                {
                    foundUnit = hit.transform.gameObject.GetComponent<Unit>();


                    // If attack is impossible, deselect the found unit
                    // and put selector back to this unit
                    if (foundUnit != null)
                    {
                        PrepareAttackOn(foundUnit);
                        foundUnit.DeselectUnit();
                        this.SelectThis(true);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log("Deselected " + this.unitName + " via attack cancel");

                if(foundUnit != null)
                {
                    foundUnit.DeselectUnit();
                }

                isAttacking = false;
            }

        }
        else if (isSelected)
        {
            unitInfo.DisplayStats(this);
            if (!moveIsDone)
            {
                if (turnManager.isPlayerTurn && unitAffiliation == Team.HERO || !turnManager.isPlayerTurn && unitAffiliation == Team.ENEMY)
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
                DoneActing();
            }

            //Simulate a unit getting damaged
            if (Input.GetKeyDown(KeyCode.F))
            {
                CHP -= 10;
                Debug.Log(CHP);
                if (CHP <= 0)
                {
                    KillUnit();
                }
            }

            //Hit Z to simulated a selected unit to be in attacking phase
            if (Input.GetKeyDown(KeyCode.Z) && !actionIsDone)
            {
                Debug.Log(this.unitName + " is atacc");
                isAttacking = true;
            }

            if(Input.GetKeyDown(KeyCode.B) && moveIsDone && !actionIsDone)
            {
                battleNavigate.Return();
                moveIsDone = false;
            }

        }
    }


    // Called when cancelling a unit's selection via attack toggle, etc
    internal void DeselectUnit()
    {
        isSelected = false;
        SelectThis(false);
        isAttacking = false;
    }


    // Can move and attack
    internal void ReadyUnit()
    {
        moveIsDone = false;
        actionIsDone = false;
        ChangeColor(0);
    }

    // Cannot move or attack
    internal void ExhaustUnit()
    {
        SelectThis(false);
        moveIsDone = true;
        actionIsDone = true;
        ChangeColor(2);
    }

    // Unit cannot move or attack but selector is still active (used for death animation)
    internal void FinishUnit()
    {
        moveIsDone = true;
        actionIsDone = true;
        ChangeColor(2);
    }

    // Cannot move but can attack
    internal void DoneMoving()
    {
        if (!moveIsDone)
        {
            moveIsDone = true;
            ChangeColor(1);
            //Debug.Log("Unit finished moving");
        }
    }

    // Cannot move and attack
    internal void DoneActing()
    {
        SelectThis(false);
        battleNavigate.ResetNavigator();
        if (!actionIsDone)
        {
            isAttacking = false;
            ExhaustUnit();
            TurnManager.Instance.unitsDone++;
            //Debug.Log("Unit finished acting");
        }
    }

    internal void KillUnit()
    {
        FinishUnit();
        battleNavigate.ResetNavigator();
        if (unitAffiliation == Team.HERO)
        {
            TurnManager.Instance.playerUnitCount--;
        }
        else if (unitAffiliation == Team.ENEMY)
        {
            TurnManager.Instance.enemyUnitCount--;
        }  
        TurnManager.Instance.CheckWinConditions();
        Debug.Log(string.Format("{0} has died to death", this.unitName));
        StartCoroutine("DeathAnimation");
    }

    #endregion

    #region Miscellaneous

    IEnumerator DeathAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        for (float f = 1f; f >= -0.05; f -= 0.10f)
        {
            Color c = renderer.material.color;
            c.a = f;
            renderer.material.color = c;
            yield return new WaitForSeconds(0.05f);
        }
        SelectThis(false);
        Destroy(gameObject);
    }

    
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
    }

    
    void AddSkill(Skill skill)
    {
        if (skills.Count >= 3)
            return;

        skills.Add(skill);

        List<ModApplication> toApply = skills[skills.Count - 1].Activate();
        
        foreach (ModApplication mod in toApply)
        {
            AddModifier(mod);
        }

    }

    void RemoveSkill(Skill skill)
    {
        if (skills.Contains(skill))
        {
            int removeIndex = skills.IndexOf(skill);
            skills[removeIndex].Deactivate();
            skills.Remove(skill);
        }
    }

    void Equip(Weapon item)
    {
        if (weapon != null)
            Unequip(weapon);
        weapon = item;

        if (MHP == CHP)
            CHP += item.hp_mod;
        MHP += item.hp_mod;

        if (MMP == CMP)
            CMP += item.hp_mod;
        MMP += item.mp_mod;

        ATK += item.atk_mod;
        DEF += item.def_mod;
        SPR += item.spr_mod;
        SPD += item.spd_mod;
        MOV += item.mov_mod;

    }

    void Unequip(Weapon weapon)
    {

        // Should filter this through a stat modifier script
        MHP -= weapon.hp_mod;
        MMP -= weapon.mp_mod;
        ATK -= weapon.atk_mod;
        DEF -= weapon.def_mod;
        SPR -= weapon.spr_mod;
        SPD -= weapon.spd_mod;
        MOV -= weapon.mov_mod;

        if (MHP < CHP)
            CHP = MHP;
        if (MMP < CMP)
            CMP = MMP;

        weapon = null;
    }

    #endregion
}

*/