using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinModifier : ValueModifier
{
    public float min;
    
    public MinModifier(int order, float min) : base (order)
    {
        this.min = min;
    }

    public override float Modify (float value)
    {
        float newValue = Mathf.Min(min, value);
        return newValue;
    }
}
