using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : Selectable
{
	private BattleManager battleManager;
	private BattleNavigate battleNavigate;
	[SerializeField]
	internal bool isAlly; //must define as true or false in editor, on prefab, or when spawning object
	private bool moveIsDone;
	private bool actionIsDone;
	
    void Start()
    {
		battleManager = GameObject.FindObjectOfType<BattleManager>();
		battleNavigate = gameObject.GetComponentInParent<BattleNavigate>();
        StartUnit();
    }
	
	void Update()
	{
		if (isSelected)
		{
			//M to simulate unit moving
			//if (Input.GetKeyDown(KeyCode.M) && !moveIsDone)
			if (!moveIsDone)
			{
				battleNavigate.Move();
			}

			//SPACE to simulate unit performing an action
			if (Input.GetKeyDown("space") && !actionIsDone)
			{
				DoneActing();
			}

			//D to kill unit
			if (Input.GetKeyDown(KeyCode.K))
			{
				KillUnit();
			}
		}
	}
	
	void StartUnit()
	{
		if (isAlly)
		{
			battleManager.playerUnitCount++;
			if (battleManager.isPlayerTurn)
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
			battleManager.enemyUnitCount++;
			if (!battleManager.isPlayerTurn)
			{
				ReadyUnit();
			}
			else
			{
				ExhaustUnit();
			}
		}
	}
	
	internal void ReadyUnit()
	{
		moveIsDone = false;
		actionIsDone = false;
		isActive = true; //in the future, do this only for ally Units. Enemies will not be selectable.
		ChangeColor(0);
	}
	
	internal void ExhaustUnit()
	{
		moveIsDone = true;
		actionIsDone = true;
		isActive = false; //in the future, do this only for ally Units. Enemies will not be selectable.
		ChangeColor(2);
	}
	
	internal void DoneMoving()
	{
		if (!moveIsDone)
		{
			moveIsDone = true;
			ChangeColor(1);
			Debug.Log("Unit finished moving");
		}
	}
	
	internal void DoneActing()
	{
		if (!actionIsDone)
		{
			ExhaustUnit();
			battleManager.unitsDone++;
			Debug.Log("Unit finished acting");
		}	
	}
	
	internal void KillUnit()
	{
		if (isAlly)
		{
			battleManager.playerUnitCount--;
		}
		else
		{
			battleManager.enemyUnitCount--;
		}
		Destroy(gameObject);
		battleManager.CheckWinConditions();
		Debug.Log("Unit has died to death");
	}
}
