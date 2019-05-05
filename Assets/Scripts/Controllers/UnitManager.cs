using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  This class is attached to each unit game object
 *  This handles unit selection and general unit location in the units grid
 * 
 */


public class UnitManager : Selectable
{
	private BattleNavigate battleNavigate;

	[SerializeField]
	internal bool isAlly; //must define as true or false in editor, on prefab, or when spawning object

    public bool IsAlly { get { return isAlly; } }

    private bool moveIsDone;
	private bool actionIsDone;


    void Start()
    {
		battleNavigate = gameObject.GetComponentInParent<BattleNavigate>();
        RegisterToBattleManager();
        StartUnit();
    }


    void RegisterToBattleManager()
    {
        var battleNavigator = this.GetComponentInParent<BattleNavigate>();
        Int2 unitPosition = battleNavigator.GetUnitPosition();

        BattleManager.Instance.AddUnit(unitPosition, this);
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
			BattleManager.Instance.playerUnitCount++;
			if (BattleManager.Instance.isPlayerTurn)
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
			BattleManager.Instance.enemyUnitCount++;
			if (!BattleManager.Instance.isPlayerTurn)
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
			BattleManager.Instance.unitsDone++;
			Debug.Log("Unit finished acting");
		}	
	}
	
	internal void KillUnit()
	{
		if (isAlly)
		{
			BattleManager.Instance.playerUnitCount--;
		}
		else
		{
			BattleManager.Instance.enemyUnitCount--;
		}
		Destroy(gameObject);
		BattleManager.Instance.CheckWinConditions();
		Debug.Log("Unit has died to death");
	}
}
