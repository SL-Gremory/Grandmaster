using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackButtonHandler : MonoBehaviour
{
    UnitStateController usc;


    public void TestFunction()
    {
        Debug.Log("Attacking");
        usc = Selectable.currentSelected.GetComponent<UnitStateController>();
        usc.Attacking();
        
    }
}
