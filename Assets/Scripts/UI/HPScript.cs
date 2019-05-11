
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPScript : MonoBehaviour
{
    internal Vector3 _localScale;

    // Start is called before the first frame update
    public void Start()
    {
        _localScale = transform.localScale;
    }

    // Makes it so that HP bar goes down gradually
    public void ChangeLocalScale(float ratio)
    {
        // Rate of change * framerate coefficient
        _localScale.x -= ((_localScale.x - ratio) / 69f + 0.00420f) * (60f * Time.deltaTime);
    }

    // Update is called once per frame
    public void Update()
    {
        transform.localScale = _localScale;
    }
}
