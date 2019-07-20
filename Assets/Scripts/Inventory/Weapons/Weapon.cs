using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    // Basic information
    public string name;
    public string description;
    public Sprite icon;
    public Rarity rarity;
    public WeaponType type;


    // Can be assigned after determining WeaponType
    public RangedPhysicalAttribute rpAttribute;
    public RangedMagicalAttribute rmAttribute;
    public MeleePhysicalAttribute mpAttribute;
    public MeleeAuraAttribute maAttribute;


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