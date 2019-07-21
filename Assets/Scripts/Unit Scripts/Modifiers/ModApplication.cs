using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ModApplication
{
    public StatModType mType;
    public StatTypes sType;
    public float Value;


    // The order for adding mdos will go:
    //      Mod type (e.g. flat or percentage),
    //      Affected stat (e.g. ATK, DEF, HP), then
    //      Value of the mod being added (e.g. 3f, 0.12f)


    public ModApplication(StatModType modType, StatTypes statType, float value)
    {
        mType = modType;
        sType = statType;
        Value = value;
    }

}
