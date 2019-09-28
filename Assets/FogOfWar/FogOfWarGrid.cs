using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(LevelGrid))]
[RequireComponent(typeof(Terrain))]
public class FogOfWarGrid : MonoBehaviour
{
	[SerializeField]
	FogTypesSO fogTypes;
	LevelGrid grid;
	bool[,] visibilityMap;
	Texture2D visibilityTex; //R: hue, G: noise scale, B: terrain height, A: saturation
	bool dirty = true;

	float maxMapHeight; // TODO change later to reflect reality pls

	private void Awake()
	{
		grid = GetComponent<LevelGrid>();
		maxMapHeight = GetComponent<Terrain>().terrainData.size.y;
		Debug.Log(maxMapHeight);
		visibilityMap = new bool[grid.GetMapSize().x, grid.GetMapSize().y];
		visibilityTex = new Texture2D(grid.GetMapSize().x, grid.GetMapSize().y, TextureFormat.RGBAFloat, false);
		visibilityTex.wrapMode = TextureWrapMode.Clamp;
		visibilityTex.filterMode = FilterMode.Trilinear;
		for (int x = 0; x < grid.GetMapSize().x; x++)
		{
			for (int y = 0; y < grid.GetMapSize().y; y++)
			{
				visibilityTex.SetPixel(x, y, Color.clear);
			}
		}

		Shader.SetGlobalVector("_LevelSize", new Vector4(grid.GetMapSize().x, maxMapHeight, grid.GetMapSize().y, 0));
		Shader.SetGlobalTexture("_VisHeightMap", visibilityTex);

		//TEST

		for (int x = 0; x < 64; x++)
		{
			for (int y = 0; y < 64; y++)
			{
				SetVisible(new Int2(x, y), true);
				SetFog(new Int2(x, y), 1);
			}
		}
		for (int x = 0; x < 32; x++)
		{
			for (int y = 0; y < 64; y++)
			{
				//ClearFog(new Int2(x, y));
				SetFog(new Int2(x, y), 0);
			}
		}
		for (int x = 0; x < 48; x++)
		{
			for (int y = 0; y < 64; y++)
			{
				ClearFog(new Int2(x, y));
				//SetFog(new Int2(x, y), 1);
			}
		}
	}

	public void SetVisible(Int2 pos, bool visible = true)
	{
		if (pos.x < 0 || pos.y < 0 || pos.x >= grid.GetMapSize().x || pos.y >= grid.GetMapSize().y)
		{
			Debug.LogError("Fog of war out of bounds at " + pos);
			return;
		}
		visibilityMap[pos.x, pos.y] = visible;
		dirty = true;
	}

	public void SetFog(Int2 pos, int fogTypeIndex)
	{
		visibilityTex.SetPixel(pos.x, pos.y, visTexColor(pos, fogTypeIndex));
		dirty = true;
	}

	public void ClearFog(Int2 pos)
	{
		visibilityTex.SetPixel(pos.x, pos.y, Color.clear);
		dirty = true;
	}

	Color visTexColor(Int2 pos, int fogTypeIndex)
	{
		Color.RGBToHSV(fogTypes.FogTypes[fogTypeIndex].hue, out float hue, out float sat, out _);
		var col = new Color(hue, fogTypes.FogTypes[fogTypeIndex].noiseScale, (grid.GetHeight(pos.x, pos.y)) / (maxMapHeight), sat);
		return col;
	}

	private void LateUpdate()
	{
		if (!dirty)
			return;
		Color[] cols = visibilityTex.GetPixels();
		for (int x = 0; x < visibilityMap.GetLength(0); x++)
		{
			for (int y = 0; y < visibilityMap.GetLength(1); y++)
			{
				if (!visibilityMap[x, y])
					cols[x + y * visibilityMap.GetLength(0)] = visTexColor(new Int2(x, y), 0);
			}
		}
		dirty = false;
		visibilityTex.SetPixels(cols);
		visibilityTex.Apply();
		Shader.SetGlobalTexture("_VisHeightMap", visibilityTex);
	}
}
