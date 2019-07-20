using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Armor", menuName = "Armor")]
public class Armor : ScriptableObject
{
    // Basic information
    public string name;
    public string description;
    public Sprite icon;
    public Rarity rarity;

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