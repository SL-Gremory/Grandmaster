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

    public bool IsAlly { get { return isAlly; } }
	
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
				if (battleManager.isPlayerTurn && isAlly || !battleManager.isPlayerTurn && !isAlly)
				{
					battleNavigate.Move();
				}
			}
			
			//ESCAPE to deselect unit
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				SelectThis(false);
				Debug.Log("Deselected a selectable via esc");
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
		ChangeColor(0);
	}
	
	internal void ExhaustUnit()
	{
		moveIsDone = true;
		actionIsDone = true;
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
