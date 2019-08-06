using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PixelartScaler : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.CanvasScaler scaler;

    [SerializeField]
    int baseScale = 2;
    [SerializeField]
    float baseHorizontalResolution = 1920;
    [SerializeField][Range(-0.5f, 0.5f)]
    float edgeOffset;

    private void Awake()
    {
        if (scaler == null)
            scaler = GetComponent<UnityEngine.UI.CanvasScaler>();
    }

    void Update()
    {
        Scale();
    }


    void Scale()
    {
        scaler.scaleFactor = Mathf.Max(1f, Mathf.Round(baseScale * (edgeOffset + Screen.width / baseHorizontalResolution)));
    }
}
