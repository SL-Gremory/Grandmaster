using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropDummy : MonoBehaviour, IProp
{
    [SerializeField]
    bool isWalkable;

    public bool IsWalkable()
    {
        return isWalkable;
    }
}
