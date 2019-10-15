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
    internal string UnitName;
    
  //  [SerializeField]
 //   public Sprite UnitSprite;

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

    [SerializeField]
    internal int BirthYear;

    [SerializeField]
    internal int BirthMonth;

    [SerializeField]
    internal int BirthDay;

    public DateTime UnitBirthDate;
    public Zodiac UnitZodiacSign;

    private void Awake()
    {
        if (UnitBirthDate == null)
            UnitBirthDate = new DateTime(BirthYear, BirthMonth, BirthDay);

        UnitZodiacSign = Calendar.GetZodiacByDate(UnitBirthDate);
    }

    private void Start()
    {

    }


}
