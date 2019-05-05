using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{

    public int this[StatTypes s]
    {
        get
        {
            return _data[(int)s];
        }
        set
        {
            SetValue(s, value);
        }
    }

    public void SetValue(StatTypes s, int value)
    {
        int oldStat = this[s];

        // No change to stat, return 
        if (oldStat == value)
            return;

        // Otherwise overwrite to new stat value
        _data[(int)s] = value;
    }

    int[] _data = new int[(int)StatTypes.Count];
}
