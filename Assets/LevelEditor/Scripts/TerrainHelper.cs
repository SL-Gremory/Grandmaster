using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Terrain))]
[RequireComponent(typeof(LevelGrid))]
public class TerrainHelper : MonoBehaviour
{
    [SerializeField][Tooltip("How many tiles to draw around cursor.")]
    int gridDistance = 3;
    [SerializeField]
    bool showTerrainLines;
    [SerializeField][Tooltip("Show whether a tile is walkable?")]
    bool showWalkability;
    [SerializeField]
    bool showPrefabs;
    [SerializeField]
    [Tooltip("Change cell walkability with left-click.")]
    bool editWalkability;
    [SerializeField]
    [Tooltip("Set cell walkability type.")]
    Walkability walkableType;
    [SerializeField]
    bool paintPrefabs;
    [SerializeField]
    GameObject paintedPrefab;
    [SerializeField]
    bool prefabAdd;
    [SerializeField]
    bool prefabReplace;
    [SerializeField]
    bool prefabRemove;
}
