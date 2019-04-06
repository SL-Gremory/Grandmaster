using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct FogProperties
{
    public Color hue; //only hue and saturation of the color is used
    public float noiseScale; // 0 to 1
}

[CreateAssetMenu(fileName = "Fog Types", menuName ="Fog Types <--", order = 3)]
public class FogTypesSO : ScriptableObject
{
    [SerializeField]
    [Header("Kinds of fog, first must be regular fog.")]
    FogProperties[] fogTypes;

    public FogProperties[] FogTypes { get => fogTypes; }
}
