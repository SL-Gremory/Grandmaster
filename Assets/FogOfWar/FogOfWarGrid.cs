using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(LevelGrid))]
public class FogOfWarGrid : MonoBehaviour
{
    [SerializeField]
    FogTypesSO fogTypes;
    LevelGrid grid;
    bool[,] visibilityMap;
    Texture2D visibilityTex; //R: hue, G: noise scale, B: terrain height, A: saturation
    bool dirty = true;

    readonly float MAX_MAP_HEIGHT = 20; // TODO change later to reflect reality pls

    private void Awake()
    {
        grid = GetComponent<LevelGrid>();
        visibilityMap = new bool[grid.GetMapSize().x, grid.GetMapSize().y];
        visibilityTex = new Texture2D(grid.GetMapSize().x, grid.GetMapSize().y, TextureFormat.ARGB32, false);
        visibilityTex.wrapMode = TextureWrapMode.Clamp;
        visibilityTex.filterMode = FilterMode.Trilinear;
        for (int x = 0; x < grid.GetMapSize().x; x++)
        {
            for (int y = 0; y < grid.GetMapSize().y; y++)
            {
                visibilityTex.SetPixel(x, y, Color.clear);
            }
        }

        Shader.SetGlobalVector("_LevelSize", new Vector4(grid.GetMapSize().x, MAX_MAP_HEIGHT, grid.GetMapSize().y, 0));
        Shader.SetGlobalTexture("_VisHeightMap", visibilityTex);

        //TEST
        for (int x = 10; x < 25; x++)
        {
            for (int y = 10; y < 25; y++)
            {
                SetVisible(new Int2(x, y));
            }
        }
        for (int x = 25; x < 60; x++)
        {
            for (int y = 25; y < 50; y++)
            {
                SetVisible(new Int2(x, y));
            }
        }

        for (int x = 20; x < 50; x++)
        {
            for (int y = 20; y < 50; y++)
            {
                if ((x - 35) * (x - 35) + (y - 35) * (y - 35) < 50)
                    SetFog(new Int2(x, y), 1);
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
        return new Color(hue, fogTypes.FogTypes[fogTypeIndex].noiseScale, (grid.GetHeight(pos.x, pos.y) + 5) / (MAX_MAP_HEIGHT + 5), sat);
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
