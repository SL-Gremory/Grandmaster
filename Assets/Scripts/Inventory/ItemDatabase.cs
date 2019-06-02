using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemDatabase", menuName = "ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<Weapon> WeaponsDB;
    public List<Consumable> ConsumablesDB;

    public virtual void Verify()
    {
        if (WeaponsDB == null)
            WeaponsDB = new List<Weapon>();
        if (ConsumablesDB == null)
            ConsumablesDB = new List<Consumable>();
    }



}
