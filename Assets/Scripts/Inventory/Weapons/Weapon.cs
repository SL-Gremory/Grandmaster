﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    // Basic information
    public string item_name;
    public string item_description;
    public Sprite item_icon;
    public Rarity rarity;
    WeaponType type;

 
    // Weapon's attack range
    public int range;

    // Stat modification
    public int hp_mod;
    public StatModType hp_mod_type;

    public int mp_mod;
    public StatModType mp_mod_type;

    public int atk_mod;
    public StatModType atk_mod_type;

    public int def_mod;
    public StatModType def_mod_type;

    public int spr_mod;
    public StatModType spr_mod_type;

    public int spd_mod;
    public StatModType spd_mod_type;

    public int mov_mod;
    public StatModType mov_mod_type;

    public int jmp_mod;
    public StatModType jmp_mod_type;



}