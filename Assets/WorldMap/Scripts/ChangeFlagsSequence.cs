using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFlagsSequence : MonoBehaviour, ISequence
{
    [SerializeField]
    string addFlag;
    [SerializeField]
    string removeFlag;

    public void BeginSequence()
    {
        PlayerFlags.AddFlag(addFlag);
        PlayerFlags.RemoveFlag(removeFlag);
    }

    public bool HasSequenceEnded()
    {
        return true;
    }
}
