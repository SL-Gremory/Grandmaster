using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropDummy : MonoBehaviour, IProp
{
    [SerializeField]
    Walkability terrainType;
	[SerializeField, Tooltip("Ignore this object for creating heightmap")]
	bool onlyWalkability;

    public Walkability GetWalkability()
    {
        return terrainType;
    }

	public bool OnlyWalkability()
	{
		return onlyWalkability;
	}
}
