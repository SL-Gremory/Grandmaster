using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LevelGrid))]
public class FogOfWarGrid : MonoBehaviour
{
    LevelGrid grid;
    bool[,] visibilityMap;
    Texture2D visibilityTex;
    bool dirty = true;

    readonly float MAX_MAP_HEIGHT = 20; // TODO change later to reflect reality pls

    private void Awake()
    {
        grid = GetComponent<LevelGrid>();
        visibilityMap = new bool[grid.GetMapSize().x, grid.GetMapSize().y];
        visibilityTex = new Texture2D(grid.GetMapSize().x, grid.GetMapSize().y);
        visibilityTex.wrapMode = TextureWrapMode.Clamp;
        visibilityTex.filterMode = FilterMode.Trilinear;
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
        SetVisible(new Int2(25, 25));
        SetVisible(new Int2(26, 26));
        SetVisible(new Int2(27, 27));
        SetVisible(new Int2(28, 28));
        SetVisible(new Int2(29, 29));
        SetVisible(new Int2(30, 30));
        SetVisible(new Int2(31, 31));


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

    private void LateUpdate()
    {
        if (!dirty)
            return;
        dirty = false;
        Color[] cols = new Color[visibilityMap.GetLength(0) * visibilityMap.GetLength(1)];
        for (int x = 0; x < visibilityMap.GetLength(0); x++)
        {
            for (int y = 0; y < visibilityMap.GetLength(1); y++)
            {
                cols[x + y * visibilityMap.GetLength(0)] = visibilityMap[x, y] ? Color.black : Color.red;
                cols[x + y * visibilityMap.GetLength(0)].g = grid.GetHeight(x, y)/MAX_MAP_HEIGHT; // can it overflow?
            }
        }
        visibilityTex.SetPixels(cols);
        visibilityTex.Apply();
        Shader.SetGlobalTexture("_VisHeightMap", visibilityTex);
    }
}
