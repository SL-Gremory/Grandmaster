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
    
    [SerializeField]
    internal readonly string UnitName;
    //public readonly Sprite UnitSprite;

    [SerializeField]
    internal Job UnitJob;

    [SerializeField]
    internal Team UnitTeam;

    //[SerializeField]
    //internal SpriteRenderer UnitRender;

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
