using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnhance1 : Skill
{
   
    public AttackEnhance1()
    {
        Name = "Attack Enhance 1";
        Description = "Adds 3 to ATK";
        skillRarity = SkillRarity.COMMON;
        sType = SkillType.PASSIVE;
        mType = StatModType.Flat;
    }

    public override List<ModApplication> Activate()
    {
        List<ModApplication> mods = null;

        // List mods down here
        mods.Add(new ModApplication(mType, StatTypes.ATK, 3f));

        return mods;
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }
}
