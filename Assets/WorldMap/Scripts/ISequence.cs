using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISequence
{
    void BeginSequence();

    bool HasSequenceEnded();
}
