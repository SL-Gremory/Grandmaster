using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ValueModifier : Modifier
{
    public ValueModifier(int order) : base(order)
    {
    }
    public abstract float Modify(float value);
}
