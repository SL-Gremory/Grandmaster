using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this gameObject is a prop
// use this on level props together with colliders
// terrain height and walkability will take this into account
public interface IProp
{
    bool IsWalkable();
}
