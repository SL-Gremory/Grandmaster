using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraToggle : MonoBehaviour
{
    public Camera PerspectiveCamera;
    public Camera TopDownCamera;

    public void ShowTopDown()
    {
        PerspectiveCamera.enabled = false;
        TopDownCamera.enabled = true;
    }

    public void ShowPerspective()
    {
        PerspectiveCamera.enabled = true;
        TopDownCamera.enabled = false;
    }

    private void Start()
    {
        // Top-down view is default

        TopDownCamera.enabled = true;
        PerspectiveCamera.enabled = false;

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
