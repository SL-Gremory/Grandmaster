using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCameraControl : MonoBehaviour
{

    [SerializeField]
    float referenceHeight = 1080;
    [SerializeField]
    float pixelsPerUnit = 32f;
    [SerializeField]
    float rotationSpeed = 1f;
    [SerializeField]
    float movementSpeed = 1f;



    Camera cam;
    float zoomAmount = 1;
    Vector2 oldMousePos;
    Vector3 origCameraPos;
    Quaternion origCameraRot;
    bool zoomedIn;

    private IEnumerator menuControl;

    void Awake()
    {
        cam = GetComponent<Camera>();
        oldMousePos = Input.mousePosition;
        origCameraPos = cam.transform.position;
        origCameraRot = cam.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
		    //Rotation Code
        zoomAmount += Input.mouseScrollDelta.y;
        zoomAmount = Mathf.Clamp(zoomAmount, 1f, 12f);
        var movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        movement = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * movement;
        movement = transform.InverseTransformDirection(movement);
        transform.Translate(movement * movementSpeed * Time.deltaTime, Space.Self);

        if (Input.GetMouseButton(1))
        {
            Selectable.zoomed = false;
            BattleState.radialMenu.SetActive(false);
        }
        var scale = Mathf.Max(1f,Mathf.Round(zoomAmount * (Screen.height / referenceHeight)));
        var halfHeight = Screen.height / (2f * pixelsPerUnit * scale);
        cam.orthographicSize = halfHeight;

    		//Translation code
    		if (Input.GetMouseButton(2))
    		{
    			var mouseXdelta = (Input.mousePosition.x - oldMousePos.x) / Screen.width;
    			var mouseYdelta = (Input.mousePosition.y - oldMousePos.y) / Screen.height;
    			transform.Translate(-mouseXdelta*40f/(zoomAmount), -mouseYdelta*40f/(zoomAmount), 0f);
    		}

        oldMousePos = Input.mousePosition;
    }
}
