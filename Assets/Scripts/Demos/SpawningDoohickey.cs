using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningDoohickey : MonoBehaviour
{
    void Start()
    {
        GameObject unitPrefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
        unitPrefab.AddComponent<Stats>();
        unitPrefab.AddComponent<Rank>();
        unitPrefab.AddComponent<Job>();
        unitPrefab.AddComponent<UnitClass>();
    }
}

