using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This will assign the grand order at which a stat is being modified

public abstract class Modifier
{
    public readonly int order;

    public Modifier(int order)
    {
        this.order = order;
    }
}
