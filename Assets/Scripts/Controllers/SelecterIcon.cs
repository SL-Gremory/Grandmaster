using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelecterIcon : MonoBehaviour
{
	public static GameObject targetObject;

    static SelecterIcon instance;

    private void Awake()
    {
        if (instance != null)
            Debug.LogError("Multiple SelecterIcons in the scene.", this);
        instance = this;
    }

    void Update()
    {
		if (targetObject != null)
		{
			transform.position = targetObject.transform.position + new Vector3(0f, 1f, 0f);
			transform.Rotate(0f, 0f, -3f);
		}
    }
	
	public static void ResetPosition()
	{
		instance.transform.position = Vector3.zero;
		instance.transform.rotation = Quaternion.identity;
	}
}