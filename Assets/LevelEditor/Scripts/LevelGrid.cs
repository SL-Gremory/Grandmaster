using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Terrain))]
[RequireComponent(typeof(TerrainCollider))]
[ExecuteInEditMode]
public class LevelGrid : MonoBehaviour
{
    static LevelGrid instance;
    public static LevelGrid Instance { get { return instance; } }
    [SerializeField]
    [Tooltip("Sets tiles as unwalkable using steepness of terrain. (0°-90°)")]
    int setUnwalkableAngle;
    [SerializeField]
    bool useTerrain = true;
    [SerializeField]
    bool useProps = true;
    [SerializeField]
    bool usePrefabs = true;
    [SerializeField]
    [HideInInspector]
    bool[] walkables;
    [SerializeField]
    [HideInInspector]
    float[] heights;
    [SerializeField]
    PrefabHolder[] prefabMap;
    [SerializeField]
    [HideInInspector]
    int sizeX;
    [SerializeField]
    [HideInInspector]
    int sizeZ;

    public bool IsInitialized { get; private set; }
    Dictionary<Int2, GameObject> spawnedPrefabs = new Dictionary<Int2, GameObject>();
    GameObject prefabHolderHolder; //THANKS A LOT UNITY SERIALIZATION, workaround to get rid of all prefabs when entering play mode

    [System.Serializable] // thanks a lot, unity serialization
    class PrefabHolder
    {
        public List<GameObject> prefabs;

        public GameObject this[int i]
        {
            get { return prefabs[i]; }
            set { prefabs[i] = value; }
        }
    }

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple LevelGrid instances, this is not supported.");
            return;
        }
        instance = this;
        if (walkables == null || walkables.Length == 0 || heights == null || heights.Length == 0 || prefabMap == null || prefabMap.Length == 0)
            SampleHeights();
        else
            IsInitialized = true;

        prefabHolderHolder = GameObject.Find("PrefabHolderHolderTempShit4674");
        if (prefabHolderHolder != null)
            DestroyImmediate(prefabHolderHolder);
        prefabHolderHolder = new GameObject("PrefabHolderHolderTempShit4674");
    }

    private void OnEnable()
    {
        
    }
    
    private void OnDisable()
    {
        HidePrefabsAll();
    }

    private void OnDestroy()
    {
        instance = null;
    }

    public void SampleHeights()
    { //TODO get real height including other objects like walls and bridges using raycasts
        ProcessTerrain(GetComponent<Terrain>().terrainData);
        IsInitialized = true;
    }


    void ProcessTerrain(TerrainData data)
    {
        sizeX = data.heightmapWidth - 1;
        sizeZ = data.heightmapHeight - 1;
        if (walkables == null || walkables.Length != sizeX * sizeZ)
            walkables = new bool[sizeX * sizeZ];
        if (heights == null || heights.Length != sizeX * sizeZ)
            heights = new float[sizeX * sizeZ];
        if (prefabMap == null || prefabMap.Length != sizeX * sizeZ)
            prefabMap = new PrefabHolder[sizeX * sizeZ];
        for (int x = 0; x < sizeX; x++)
        {
            for (int z = 0; z < sizeZ; z++)
            {
                if (!HasPropThere(x, z, data))
                {
                    if (!useTerrain)
                        continue;
                    heights[x + sizeX * z] = data.GetInterpolatedHeight((x + 0.5f) / (float)sizeX, (z + 0.5f) / (float)sizeZ);

                    if (setUnwalkableAngle < 0.001f)
                        walkables[x + sizeX * z] = true;
                    else
                    {
                        walkables[x + sizeX * z] = data.GetSteepness((x + 0.5f) / (float)sizeX, (z + 0.5f) / (float)sizeZ) < setUnwalkableAngle;
                    }
                }
            }
        }
    }

    public void AddPrefab(int posX, int posZ, GameObject prefab, bool removeOther = false)
    {
        HidePrefabsAt(posX, posZ);
        var prefabHolder = prefabMap[posX + sizeX * posZ];
        if (prefab == null && removeOther)
        {
            prefabHolder.prefabs.Clear();
        }
        else if (prefabHolder.prefabs.Count == 0 || removeOther)
        {
            prefabHolder.prefabs.Clear();
            prefabHolder.prefabs.Add(prefab);
        }
        else
        {
            var ind = prefabHolder.prefabs.IndexOf(prefab);
            if (ind == -1)
            {
                prefabHolder.prefabs.Add(prefab);
            }
        }
    }

    public void RemovePrefab(int posX, int posZ, GameObject prefab)
    {
        HidePrefabsAt(posX, posZ);
        if (prefab == null)
        {
            prefabMap[posX + sizeX * posZ].prefabs.Clear();
            return;
        }

        var prefabs = prefabMap[posX + sizeX * posZ];
        if (prefabs.prefabs.Count == 0 || prefabs.prefabs.IndexOf(prefab) == -1)
            return;
        prefabs.prefabs.Remove(prefab);
    }
    /// <summary>
    /// Returns prefabs at the position
    /// </summary>
    /// <param name="posX"></param>
    /// <param name="posZ"></param>
    /// <returns></returns>
    public List<GameObject> GetPrefabsAt(int posX, int posZ)
    {
        return prefabMap[posX + sizeX * posZ]?.prefabs;
    }

    bool HasPropThere(int posX, int posZ, TerrainData data)
    {
        if (!useProps)
            return false;
        var realPos = transform.position + new Vector3(posX + 0.5f, data.size.y + 100, posZ + 0.5f);
        var hits = Physics.BoxCastAll(realPos, Vector3.one / 4f, Vector3.down, Quaternion.identity, realPos.y);
        System.Array.Sort(hits, delegate (RaycastHit hit1, RaycastHit hit2)
        {
            return hit2.point.y.CompareTo(hit1.point.y);
        });
        for (int i = 0; i < hits.Length; i++)
        {
            var hit = hits[i];
            if (hit.collider.gameObject == gameObject)
                return false;
            var prop = hit.collider.GetComponent<IProp>();
            if (prop != null)
            {
                SetWalkable(posX, posZ, prop.IsWalkable());
                SetHeight(posX, posZ, hit.point.y - transform.position.y);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Gets integer map size, map coordinates range is [0,size)
    /// </summary>
    /// <returns></returns>
    public Int2 GetMapSize() {
        return new Int2(sizeX, sizeZ);
    }

    /// <summary>
    /// Returns level height at position
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public float GetHeight(int x, int z)
    {
        Debug.Assert(x >= 0 && z >= 0 && x < sizeX && z < sizeZ);
        return heights[x + sizeX * z];
    }

    /// <summary>
    /// Sets level height at position
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <param name="height"></param>
    public void SetHeight(int x, int z, float height)
    {
        Debug.Assert(x >= 0 && z >= 0 && x < sizeX && z < sizeZ);
        heights[x + sizeX * z] = height;
    }

    /// <summary>
    /// Returns whether cell at position is walkable
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public bool IsWalkable(int x, int z)
    {
        Debug.Assert(x >= 0 && z >= 0 && x < sizeX && z < sizeZ);
        return walkables[x + sizeX * z];
    }

    /// <summary>
    /// Sets walkability at position
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <param name="value"></param>
    public void SetWalkable(int x, int z, bool value)
    {
        Debug.Assert(x >= 0 && z >= 0 && x < sizeX && z < sizeZ);
        walkables[x + sizeX * z] = value;
    }
    
    /// <summary>
    /// Instantiates prefabs at the position, returns a Parent GameObject which holds them, null if no prefabs here
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public GameObject ShowPrefabsAt(int x, int z)
    {
        var prefabs = GetPrefabsAt(x, z);
        var pos = new Int2(x, z);
        if (prefabs.Count == 0)
            return null;
        if (spawnedPrefabs.TryGetValue(pos, out GameObject holder)) {
            return holder;
        }
        holder = new GameObject();
        holder.transform.SetParent(prefabHolderHolder.transform, true);
        spawnedPrefabs[pos] = holder;
        holder.transform.position = new Vector3(pos.x + 0.5f, GetHeight(x, z), pos.y + 0.5f);
        for (int i = 0; i < prefabs.Count; i++)
        {
            Instantiate(prefabs[i], holder.transform, false);
        }
        return holder;
    }

    /// <summary>
    /// Destroys prefabs at position, if they were spawned
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    public void HidePrefabsAt(int x, int z) {
        if (spawnedPrefabs.TryGetValue(new Int2(x, z), out GameObject holder)) {
            spawnedPrefabs.Remove(new Int2(x, z));
            if (Application.isPlaying)
                Destroy(holder);
            else
                DestroyImmediate(holder);
        }
    }

    /// <summary>
    /// Destroys prefabs at all positions
    /// </summary>
    public void HidePrefabsAll() {
        foreach (var holder in spawnedPrefabs.Values)
        {
            if (Application.isPlaying)
                Destroy(holder);
            else
                DestroyImmediate(holder);
        }
        spawnedPrefabs.Clear();
    }
}
