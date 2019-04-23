using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GridOverlay : MonoBehaviour
{
    [SerializeField]
    Shader overlayShader;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Camera>().SetReplacementShader(overlayShader, "Overlay");
    }

}
