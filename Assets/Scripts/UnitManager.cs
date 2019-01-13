using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
	//Initialize variables
	private EventManager eventManager;
	private Color originalColor;
	private Color darkColor1;
	private Color darkColor2;
	private bool moveIsDone;
	private bool actionIsDone;
	public bool isAlly; //must define as true or false in editor, on prefab, or when spawning object
	
    // Start is called before the first frame update
    void Start()
    {
		eventManager = GameObject.FindObjectOfType<EventManager>();
		originalColor = this.GetComponent<SpriteRenderer>().color;
		darkColor1 = originalColor; darkColor1.r -= 0.3f; darkColor1.g -= 0.3f; darkColor1.b -= 0.3f; //excuse my bullshit
		darkColor2 = originalColor; darkColor2.r -= 0.6f; darkColor2.g -= 0.6f; darkColor2.b -= 0.6f; //excuse my bullshit
		
        if (isAlly)
		{
			eventManager.playerUnitCount++;
			if (eventManager.playerFirst)
			{
				ReadyUnit();
			}
			else
			{
				ExhaustUnit();
			}
		}
		else
		{
			eventManager.enemyUnitCount++;
			if (!eventManager.playerFirst)
			{
				ReadyUnit();
			}
			else
			{
				ExhaustUnit();
			}
		}
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/
	
	public void ReadyUnit()
	{
		moveIsDone = false;
		actionIsDone = false;
		ChangeColor(0);
	}
	
	public void ExhaustUnit()
	{
		moveIsDone = true;
		actionIsDone = true;
		ChangeColor(2);
	}
	
	void DoneMoving()
	{
		if (!moveIsDone)
		{
			moveIsDone = true;
			ChangeColor(1);
			Debug.Log("Unit finished moving");
		}
	}
	
	void DoneActing()
	{
		if (!actionIsDone)
		{
			ExhaustUnit();
			eventManager.unitsDone++;
			Debug.Log("Unit finished acting");
		}	
	}
	
	void KillUnit()
	{
		if (isAlly)
		{
			eventManager.playerUnitCount--;
		}
		else
		{
			eventManager.enemyUnitCount--;
		}
		Destroy(gameObject);
		Debug.Log("Unit has died to death");
	}
	
	void OnMouseEnter()
	{
		if (!actionIsDone)
		{
			Highlight(true);
		}
	}
	
	void OnMouseExit()
	{
		Highlight(false);
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
			KillUnit();
		}
	}
	
	void Highlight(bool highlighted)
	{
		//not a true highlight, just changes the transparency
		Color tmp = this.GetComponent<SpriteRenderer>().color;
		if(highlighted)
		{
			tmp.a = 0.7f;
		}
		else
		{
			tmp.a = 1f;
		}
		this.GetComponent<SpriteRenderer>().color = tmp;
	}
	
	void ChangeColor(int choice)
	{
		if (choice == 0)
		{
			this.GetComponent<SpriteRenderer>().color = originalColor;
		}
		else if (choice == 1)
		{
			this.GetComponent<SpriteRenderer>().color = darkColor1;
		}
		else if (choice == 2)
		{
			this.GetComponent<SpriteRenderer>().color = darkColor2;
		}
	}
}
