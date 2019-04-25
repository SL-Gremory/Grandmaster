using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelecterIcon : MonoBehaviour
{
	internal GameObject targetObject;
	
    void Update()
    {
		if (targetObject != null)
		{
			transform.position = targetObject.transform.position + new Vector3(0f, 1f, 0f);
			transform.Rotate(0f, 0f, -3f);
		}
    }
	
	internal void ResetPosition()
	{
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
	}
}