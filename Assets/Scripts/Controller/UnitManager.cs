using System; //for String characterName
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : Selectable
{
	private EventManager eventManager;
	private Stats stats;//might need to make public if I understand how this works
	private CharacterData characterData; //consider changing private to static since there only needs to be one instance of this variable
	public String characterName; //must define when spawning object
	public bool isAlly; //must define as true or false in editor, on prefab, or when spawning object
	private bool moveIsDone;
	private bool actionIsDone;
	
    void Start()
    {
		eventManager = GameObject.FindObjectOfType<EventManager>();
		stats = gameObject.GetComponentInParent<Stats>();
		characterData = GameObject.FindObjectOfType<CharacterData>();
        StartUnit();
    }
	
	void Update()
	{
		if (isSelected)
		{
			//M to simulate unit moving
			if (Input.GetKeyDown(KeyCode.M) && !moveIsDone)
			{
				DoneMoving();
			}

			//A to simulate unit performing an action
			if (Input.GetKeyDown(KeyCode.A) && !actionIsDone)
			{
				DoneActing();
			}

			//D to kill unit
			if (Input.GetKeyDown(KeyCode.K))
			{
				KillUnit();
				//stats.SetValue(StatTypes.HP,0); //sets hp stat to zero; test unit death detection when it gets implemented
			}
			//SPACE to view the unit's character info in the console
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
	}
	
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
		isActive = true; //in the future, do this only for ally Units. Enemies will not be selectable.
		ChangeColor(0);
	}
	
	public void ExhaustUnit()
	{
		moveIsDone = true;
		actionIsDone = true;
		isActive = false; //in the future, do this only for ally Units. Enemies will not be selectable.
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
}
