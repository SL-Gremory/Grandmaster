using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Each item/equipment can have one or multiple features
     
    Use cases:
    1. Feature can be activated for some time and then deactivated,
        e.g. equipping and unequiping a sword will change the attack stat
    2. Feature can be permanent, e.g. health potions
 */

public abstract class Feature : MonoBehaviour
{
    #region Fields / Properties
    protected GameObject _target { get; private set; }
    #endregion

    #region Public
    public void Activate(GameObject target)
    {
        if(_target == null)
        {
            _target = target;
            OnApply();
        }
    }

    public void Apply(GameObject target)
    {
        _target = target;
        OnApply();
        _target = null;
    }

    public void Deactivate()
    {
        if(_target != null)
        {
            OnRemove();
            _target = null;
        }
    }


    #endregion


    #region Private
    protected abstract void OnApply();
    protected virtual void OnRemove() { }
    #endregion
}
