using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddModifier : ValueModifier
{
    public readonly float addingValue;

    public AddModifier(float addingValue, int order) : base(order)
    {
        this.addingValue = addingValue;
    }

    public override float Modify(float value)
    {
        float newValue = value + addingValue;
        return newValue;
    }
}
