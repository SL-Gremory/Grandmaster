using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 

public class CameraControl : MonoBehaviour
{

    [SerializeField] GameObject playerObject;
    [SerializeField] [Range(20f, 200f)] float smoothCamera = 50f;

    Transform target;
    Vector3 offset;

    private void Start()
    {
        target = playerObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        SetCameraToTarget();
        RotateCamera();
        ZoomCamera();
    }

    void RotateCamera()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            transform.RotateAround(target.transform.position, new Vector3(0.0f, -45.0f, 0.0f), smoothCamera * Time.deltaTime);
            //Vector3 rotatedPosition = target.position + offset;
            //Vector3 smoothedCamera = Vector3.Lerp(transform.position, rotatedPosition, smoothCamera);
            //transform.position = smoothedCamera;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            transform.RotateAround(target.transform.position, new Vector3(0.0f, 45.0f, 0.0f), smoothCamera * Time.deltaTime);
            //transform.Rotate(new Vector​3(0.0f, -90.0f, 0.0f));
            //Vector3 targetPosition = playerObject.transform.position;
            //Vector3 cameraPosition = Camera.main.transform.position;
            //Vector3 rotatedPosition = target.position + offset;
            //Vector3 smoothedCamera = Vector3.Lerp(transform.position, rotatedPosition, smoothCamera);
            //transform.position = smoothedCamera;
        }
    }

    void ZoomCamera()
    {
        if (Input.GetKey(KeyCode.Z))
        {

        }
        else if (Input.GetKey(KeyCode.C))
        {

        }
    }

    //Update camera everytime button is pressed.
    void SetCameraToTarget()
    {
        //transform.LookAt(target);
    }

    //Allow user to change target
    /*void ChangeTarget()
    {

    }*/
}
