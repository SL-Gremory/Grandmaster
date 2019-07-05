using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 *  Must attach this script to each game object
 */

public class UnitData : MonoBehaviour
{

    /*
     *   DEFINING CHARACTERISTICS
     */

    internal string UnitName;
    //public readonly Sprite UnitSprite;
    internal Job UnitJob;
    internal Team UnitTeam;

    [SerializeField]
    private SpriteRenderer UnitRender;

    //public CharacterSkill UnitCharacterSkill;
    //public RaceSkill UnitRaceSkill;

    [SerializeField]
    internal List<Skill> UnitSkills;

    [SerializeField]
    internal Weapon UnitWeapon;

    [SerializeField]
    internal Armor UnitArmor;


    private void Awake()
    {
        
    }

    private void Start()
    {

    }


}
