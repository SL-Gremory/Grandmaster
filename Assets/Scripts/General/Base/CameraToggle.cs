using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraToggle : MonoBehaviour
{
    public Camera PerspectiveCamera;
    public Camera TopDownCamera;
/*
    public void ShowTopDown()
    {
        PerspectiveCamera.enabled = true;
        TopDownCamera.enabled = false;
    }

    public void ShowPerspective()
    {
        PerspectiveCamera.enabled = false;
        TopDownCamera.enabled = true;
    }
*/
    private void Start()
    {
        // Perspective view is default

        TopDownCamera.enabled = false;
        PerspectiveCamera.enabled = true;

    }

    private void Update()
    {
        // Toggles camera view when a key is clicked
        if(Input.GetKeyDown(KeyCode.M))
        {
            TopDownCamera.enabled = !TopDownCamera.enabled;
            PerspectiveCamera.enabled = !PerspectiveCamera.enabled;
        }
    }
}
