using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public virtual void Enter()
    {
        AddListeners();
    }

    public virtual void Exit()
    {
        RemoveListeners();
    }

    protected virtual void OnDestroy()
    {
        RemoveListeners();
    }

    public void AddListeners()
    {
        throw new NotImplementedException();
    }

    private void RemoveListeners()
    {
        throw new NotImplementedException();
    }
}
