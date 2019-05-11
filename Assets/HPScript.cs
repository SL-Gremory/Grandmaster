
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPScript : MonoBehaviour
{
    public Vector3 localScale;
    // Start is called before the first frame update
    public void Start()
    {
        localScale = transform.localScale;
    }

    // Update is called once per frame
    public void Update()
    {
        transform.localScale = localScale;
    }
}
