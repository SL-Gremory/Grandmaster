using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAssets : MonoBehaviour
{
    public static SceneAssets Instance { get; set; }

    [SerializeField]
    public Transform DamagePopupPrefab;
}
