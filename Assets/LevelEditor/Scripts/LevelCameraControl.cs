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
	bool wasInside = false; //was mouse inside the window in the previous frame?
	
    void Awake()
    {
        cam = GetComponent<Camera>();
        oldMousePos = Input.mousePosition;
    }
	
	private bool MouseInWindow()
	{
		//is the mouse within the bounds of the window as defined by the camera?
		var view = cam.ScreenToViewportPoint(Input.mousePosition);
		bool isInside = view.x > 0 && view.x < 1 && view.y > 0 && view.y < 1;
		if (!isInside)
		{
			wasInside = false;
		}
		return isInside;
	}
	
	private bool MouseEnteredWindow()
	{
		//is this the frame in which the mouse entered the window?
		bool enteredWindow = !wasInside && MouseInWindow();
		return enteredWindow;
	}
	
    // Update is called once per frame
    void Update()
    {
		if (!MouseInWindow())
		{
			Debug.Log("mouse was not in window this frame");
		}
		else if (MouseEnteredWindow())
		{
			//Mouse must be in window per the previous if statement
			//give the game a frame to store the actual mouse position to prevent "skipping"
			oldMousePos = Input.mousePosition;
			wasInside = true;
		}
		else
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
				var mouseXdelta = (Input.mousePosition.x - oldMousePos.x) / Screen.width;
				transform.RotateAround(transform.position + transform.forward * 20f, Vector3.up, mouseXdelta * rotationSpeed * Time.deltaTime * 365f);
				//transform.Rotate(new Vector3(0, , 0), Space.World);
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
}
