using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipContainer : MonoBehaviour
{
    Parameters statsReference;
    Weapon weapon;
    Armor armor;

    private void Start()
    {
        statsReference = gameObject.GetComponent<Parameters>();
    }


    void Equip(Weapon w)
    {

    }
    

    void Equip(Armor a)
    {

    }

    void Unequip(Weapon w)
    {

    }

    void Unequip(Armor a)
    {

    }
}
