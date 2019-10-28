using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Terrain Types", menuName = "Create Terrain Types")]
public class TerrainTypesSO : ScriptableObject
{
	[System.Serializable]
	public struct LayerAndWalkability {
		public TerrainLayer layer;
		public Walkability walkability;
	}
	[SerializeField]
	LayerAndWalkability[] layers;

	public Walkability FindWalkability(TerrainLayer layer) {
		foreach (var item in layers)
		{
			if (item.layer == layer)
				return item.walkability;
		}
		Debug.LogWarning("WARNING: Layer " + layer.name + " not found in this terrain types SO.", this);
		return Walkability.Unwalkable;
	}
}
