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
	[SerializeField]
	float terrainHeight = 0f; //enter average height of terrain in the Y direction >> camera rotates at this height
	[SerializeField]
	float[] terrainBounds = {0f,100f,0f,100f}; //X min, X max, Z min, Z max
	
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
			//Zooming code? idk what this does
			zoomAmount += Input.mouseScrollDelta.y;
			zoomAmount = Mathf.Clamp(zoomAmount, 1f, 12f);
			var movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
			movement = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * movement;
			movement = transform.InverseTransformDirection(movement);
			transform.Translate(movement * movementSpeed * Time.deltaTime, Space.Self);
			var scale = Mathf.Max(1f,Mathf.Round(zoomAmount * (Screen.height / referenceHeight)));
			var halfHeight = Screen.height / (1f * pixelsPerUnit * scale); //was originally 2f, but varler wanted a further zoomed out view
			cam.orthographicSize = halfHeight;
			
			//Rotation code
			var viewDistanceToTerrain = (transform.position.y-terrainHeight)/Mathf.Sin(transform.eulerAngles.x*Mathf.PI/180f);
			var pivotPoint = transform.position + transform.forward * viewDistanceToTerrain;
			if (Input.GetMouseButton(1))
			{
				var mouseXdelta = (Input.mousePosition.x - oldMousePos.x) / Screen.width;
				transform.RotateAround(pivotPoint, Vector3.up, mouseXdelta * rotationSpeed * Time.deltaTime * 365f);
			}
			
			//Translation code
			if (Input.GetMouseButton(2))
			{	
				var mouseXdelta = (Input.mousePosition.x - oldMousePos.x) / Screen.width;
				var mouseYdelta = (Input.mousePosition.y - oldMousePos.y) / Screen.height;
				var xMoveAmount = -40f*mouseXdelta/(Mathf.Pow(zoomAmount,0.5f));
				var yMoveAmount = -60f*mouseYdelta*Mathf.Sin(transform.eulerAngles.x*Mathf.PI/180f)/(Mathf.Pow(zoomAmount,0.5f));
				var zMoveAmount = -60f*mouseYdelta*Mathf.Cos(transform.eulerAngles.x*Mathf.PI/180f)/(Mathf.Pow(zoomAmount,0.5f));
				transform.Translate(xMoveAmount, yMoveAmount, zMoveAmount);
				pivotPoint = transform.position + transform.forward * viewDistanceToTerrain; //reload pivotPoint position to prevent skipping
				if (pivotPoint.x < terrainBounds[0]) //minimum X
				{
					var target = new Vector3(transform.position.x+terrainBounds[0]-pivotPoint.x, transform.position.y, transform.position.z);
					transform.position = target;
				}
				else if (pivotPoint.x > terrainBounds[1]) //maximum X
				{
					var target = new Vector3(transform.position.x+terrainBounds[1]-pivotPoint.x, transform.position.y, transform.position.z);
					transform.position = target;
				}
				if (pivotPoint.z < terrainBounds[2]) //minimum Z
				{
					var target = new Vector3(transform.position.x, transform.position.y, transform.position.z+terrainBounds[2]-pivotPoint.z);
					transform.position = target;
				}
				else if (pivotPoint.z > terrainBounds[3]) //maximum Z
				{
					var target = new Vector3(transform.position.x, transform.position.y, transform.position.z+terrainBounds[3]-pivotPoint.z);
					transform.position = target;
				}
			}
			
			//Set current mouse position as old position for next loop
			oldMousePos = Input.mousePosition;
		}
    }
}
