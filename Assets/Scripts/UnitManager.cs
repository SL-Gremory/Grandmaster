using System; //for String characterName
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
	//Initialize variables
	private EventManager eventManager;
	private Stats stats;//might need to make public if I understand how this works
	private CharacterData characterData; //consider changing private to static since there only needs to be one instance of this variable
	public String characterName; //must define when spawning object
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
		stats = gameObject.GetComponentInParent<Stats>();
		characterData = GameObject.FindObjectOfType<CharacterData>();
		originalColor = this.GetComponent<SpriteRenderer>().color;
		darkColor1 = originalColor; darkColor1.r -= 0.3f; darkColor1.g -= 0.3f; darkColor1.b -= 0.3f; //excuse my bullshit
		darkColor2 = originalColor; darkColor2.r -= 0.6f; darkColor2.g -= 0.6f; darkColor2.b -= 0.6f; //excuse my bullshit
		
        StartUnit();
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/
	
	void StartUnit()
	{
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
		
		StatTypes[] order = new StatTypes[]
		{
			StatTypes.HP,     // Hit points
			StatTypes.MP,     // "Magic" points
			StatTypes.ATK,    // Physical/magical attack power
			StatTypes.DEF,    // Physical defense
			StatTypes.SPR,    // Magical defense
			StatTypes.SPD,    // Speed
		};
		int[] characterStats = characterData.ReportStats(characterName);
		for(int i = 0; i < order.Length; i++)
		{
			StatTypes currentType = order[i];
			stats.SetValue(currentType,characterStats[i]);
		}
	}
	
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
		eventManager.CheckWinConditions();
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
			//stats.SetValue(StatTypes.HP,0); //sets hp stat to zero
		}
		//hit space to view the unit's info in the console. For testing.
		if (Input.GetKeyDown("space"))
        {
			Debug.Log(characterName);
			Debug.Log("HP = " + stats[StatTypes.HP]);
			Debug.Log("MP = " + stats[StatTypes.MP]);
			Debug.Log("ATK = " + stats[StatTypes.ATK]);
			Debug.Log("DEF = " + stats[StatTypes.DEF]);
			Debug.Log("SPR = " + stats[StatTypes.SPR]);
			Debug.Log("SPD = " + stats[StatTypes.SPD]);
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
