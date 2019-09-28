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

        if(Input.GetKeyDown(KeyCode.R))
        {
            Selectable.zoomed = false;
            BattleState.radialMenu.SetActive(false);
        }


        if (!Selectable.zoomed)
        {
           // if(cam.transform.position != origCameraPos) cam.transform.rotation = origCameraRot;
     

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
            var scale = Mathf.Max(1f, Mathf.Round(zoomAmount * (Screen.height / referenceHeight)));
            var halfHeight = Screen.height / (2f * pixelsPerUnit * scale);
            cam.orthographicSize = halfHeight;

            oldMousePos = Input.mousePosition;

        } else
        {

            GameObject focusedUnit = Selectable.currentSelected;
            //cam.transform.position = Vector3.Lerp(cam.transform.position, focusedUnit.transform.position, Time.deltaTime * movementSpeed);
            //cam.transform.position = Vector3.MoveTowards(cam.transform.position, focusedUnit.transform.position, Time.deltaTime * movementSpeed * 5);
            //Vector3 targetPos = new Vector3(focusedUnit.transform.localPosition.x - offset, cam.transform.localPosition.y, focusedUnit.transform.localPosition.z - offset);
            //cam.transform.position = Vector3.MoveTowards(cam.transform.localPosition, targetPos, Time.deltaTime * movementSpeed * 4);




            //cam.transform.LookAt(focusedUnit.transform);
            Quaternion toRotation = Quaternion.LookRotation(focusedUnit.transform.position - cam.transform.position);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, toRotation, Time.deltaTime * rotationSpeed);

            // Do not touch very sensitive Kapp
            //menuControl = StartActivate(Time.deltaTime * rotationSpeed * 1.4f);
            //StartCoroutine(menuControl);

            if (Input.GetMouseButton(1))
            {
                Selectable.currentSelected = null;
                Selectable.zoomed = false;
            }

        }

    }

    private IEnumerator StartActivate(float v)
    {
        yield return new WaitForSeconds(v);
        //BattleState.radialMenu.SetActive(true);

    }
}
