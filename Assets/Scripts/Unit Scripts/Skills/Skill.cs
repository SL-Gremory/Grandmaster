using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{

    public string Name;
    public string Description;
    public SkillRarity skillRarity;
    public SkillType sType;
    public StatModType mType;

    //public Sprite SkillSprite;



    public Skill()
    {

    }


    // Override method to specify what skill should do  
    public virtual List<ModApplication> Activate()
    {
        return null;
    }

    public virtual void Deactivate() { }
}
