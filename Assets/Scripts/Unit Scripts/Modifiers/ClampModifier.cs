using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampModifier : ValueModifier
{
    public readonly float min;
    public readonly float max;

    public ClampModifier(int order, float min, float max) : base(order)
    {
        this.min = min;
        this.max = max;
    }

    public override float Modify(float value)
    {
        float newValue = Mathf.Clamp(value, min, max);
        return newValue;
    }
}
