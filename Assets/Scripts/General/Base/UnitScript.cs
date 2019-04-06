using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : Unit
{
    public Color LeadingColor;
    public Camera PerspectiveCamera;
    public Camera TopDownCamera;

    public override void Initialize()
    {
        base.Initialize();
        transform.position += new Vector3(0, 1, 0);
        GetComponent<Renderer>().material.color = LeadingColor + Color.black;
    }

    public override void MarkAsAttacking(Unit other)
    {
        throw new System.NotImplementedException();
    }

    public override void MarkAsDefending(Unit other)
    {
        throw new System.NotImplementedException();
    }

    public override void MarkAsDestroyed()
    {
        throw new System.NotImplementedException();
    }

    public override void MarkAsFinished()
    {
        GetComponent<Renderer>().material.color = LeadingColor + Color.gray;
    }

    public override void MarkAsFriendly()
    {
        GetComponent<Renderer>().material.color = LeadingColor + new
        Color(0.8f, 1, 0.8f);
    }
    public override void MarkAsReachableEnemy()
    {
        GetComponent<Renderer>().material.color = LeadingColor + Color.red;
    }
    public override void MarkAsSelected()
    {
        GetComponent<Renderer>().material.color = LeadingColor + Color.green;
    }
    public override void UnMark()
    {
        GetComponent<Renderer>().material.color = LeadingColor;
    }

    private void Update()
    {
        // This makes it so that the unit is always looking at the used camera (WIP)
        if (PerspectiveCamera.enabled) {
            transform.LookAt(transform.position + PerspectiveCamera.transform.rotation * Vector3.forward,
                PerspectiveCamera.transform.rotation * Vector3.up);
        } else
        {
            transform.LookAt(transform.position + TopDownCamera.transform.rotation * Vector3.forward,
                TopDownCamera.transform.rotation * Vector3.up);
        }
    }
}
