using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplyModifier : ValueModifier
{
    public readonly float multiplyingModifier;

    public MultiplyModifier(int order, float multiplyingModifier) : base(order)
    {
        this.multiplyingModifier = multiplyingModifier;
    }

    public override float Modify(float value)
    {
        float newValue = value * multiplyingModifier;
        return newValue;
    }
}
