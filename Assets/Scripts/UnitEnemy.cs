using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEnemy : MonoBehaviour
{
    //Initialize variables
	private EventManager eventManager;
	public bool moveIsDone = true;
	public bool actionIsDone = true;
	
	// Start is called before the first frame update
    void Start()
    {
        eventManager = GameObject.FindObjectOfType<EventManager>();
		eventManager.enemyUnitCount++;
		Darken();//DELETE THIS LINE FOR THE REAL THING
		Darken();//DELETE THIS LINE FOR THE REAL THING
    }

    // Update is called once per frame
    /*void Update()
    {
		
    }*/
	
	void OnDeath()
	{
		eventManager.enemyUnitCount--;
		//remove sprite, etc.
	}
	
	void DoneMoving()
	{
		moveIsDone = true;
		Darken();//DELETE THIS LINE FOR THE REAL THING
		Debug.Log("Unit finished moving");
		//prevent further movement
	}
	
	void DoneActing()
	{
		if(!moveIsDone){DoneMoving();}
		actionIsDone = true;
		eventManager.unitsDone++;
		Darken();//DELETE THIS LINE FOR THE REAL THING
		Debug.Log("Unit finished acting");
		//prevent further actions
	}
	
	//////////DELETE THIS SECTION FOR THE REAL THING//////////
	void OnMouseEnter()
	{
		if (!actionIsDone)
		{
			Color tmp = this.GetComponent<SpriteRenderer>().color;
			tmp.a = 0.7f;
			this.GetComponent<SpriteRenderer>().color = tmp;
		}
	}
	void OnMouseExit()
	{
		Color tmp = this.GetComponent<SpriteRenderer>().color;
		tmp.a = 1f;
		this.GetComponent<SpriteRenderer>().color = tmp;
	}
	void OnMouseOver()
	{
		//Left click to simulate unit moving
		if (Input.GetMouseButtonDown(0) && !moveIsDone)
		{
			DoneMoving();
		}

		//Right click to simulate unit performing an action
		if (Input.GetMouseButtonDown(1) && !actionIsDone)
		{
			DoneActing();
		}

		//Middle click to simulate death, so spooky
		if (Input.GetMouseButtonDown(2))
		{
			OnDeath();
			Destroy(gameObject);
			Debug.Log("Unit has died to death");
		}
	}
	void Darken()
	{
		Color tmp = this.GetComponent<SpriteRenderer>().color;
		tmp.r -= 0.3f;
		tmp.g -= 0.3f;
		tmp.b -= 0.3f;
		this.GetComponent<SpriteRenderer>().color = tmp;
	}
	public void RevertColor()
	{
		Color tmp = this.GetComponent<SpriteRenderer>().color;
		tmp.r += 0.6f;
		tmp.g += 0.6f;
		tmp.b += 0.6f;
		this.GetComponent<SpriteRenderer>().color = tmp;
	}
	//////////DELETE THIS SECTION FOR THE REAL THING//////////
}
