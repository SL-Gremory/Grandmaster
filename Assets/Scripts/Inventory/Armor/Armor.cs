using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Armor", menuName = "Armor")]
public class Armor : ScriptableObject
{
    // Basic information
    public string item_name;
    public string item_description;
    public Sprite item_icon;
    public Rarity rarity;

    // Stat modification
    public int exp_mod;
    public int hp_mod;
    public int mp_mod;
    public int atk_mod;
    public int def_mod;
    public int spr_mod;
    public int spd_mod;
    public int mov_mod;

}