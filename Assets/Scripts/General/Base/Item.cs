using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public UnitStat Strength;
}


public class Item
{
    StatModifier mod1, mod2;

    public string name;
    public void Equip(Character c)
    {
        // Modifiers need to be stored in variables fists before adding them to the stat
        mod1 = new StatModifier(10, StatModType.Flat);
        mod2 = new StatModifier(0.1f, StatModType.PercentMult);
        c.Strength.AddModifier(mod1);
        c.Strength.AddModifier(mod2);
    }

    public void Unequip(Character c)
    {
        c.Strength.RemoveModifier(mod1);
        c.Strength.RemoveModifier(mod2);
    }
}
