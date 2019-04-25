using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
	private DoneButton doneButton;
	private TurnCountText turnCountText;
	[SerializeField]
	private bool playerFirst; //must define as true or false in editor, on prefab, or when spawning object
	internal bool turnIsDone = false;
	internal bool isPlayerTurn;
	internal int playerUnitCount = 0;
	internal int enemyUnitCount = 0;
	internal int unitsDone = 0;
	internal int turnCounter = 1;
	
	void Awake()
	{
		doneButton = GameObject.FindObjectOfType<DoneButton>();
		turnCountText = GameObject.FindObjectOfType<TurnCountText>();
		if (playerFirst)
		{
			isPlayerTurn = true;
		}
		else
		{
			isPlayerTurn = false;
		}
	}
	
    void Update()
    {
        if (turnIsDone)
		{
			Debug.Log("Changing turns");
			ChangeTurns();
			turnIsDone = false;
		}
		
		if (isPlayerTurn && playerUnitCount == unitsDone)
		{
			doneButton.BrightenButton();
			unitsDone++; //stupid, hacky, but efficient solution that prevents brightening the button in every frame even after it has already been brightened
		}
		else if (!isPlayerTurn && enemyUnitCount == unitsDone)
		{
			turnIsDone = true;
		}
    }
	
	void ChangeTurns()
	{	
		isPlayerTurn = !isPlayerTurn; //current turn is done, at this point forward it is the other side's turn
		unitsDone = 0;
		doneButton.ResetButton(isPlayerTurn);
		if (isPlayerTurn)
		{
			turnCounter++; //only increments when it is becoming the player's turn
			turnCountText.DisplayNewTurn();
		}
		
		//goes through every object of type Unit and readies/exhausts allies or enemies appropriately
		UnitManager[] units = FindObjectsOfType(typeof(UnitManager)) as UnitManager[];
		foreach (UnitManager unit in units)
		{
			if (isPlayerTurn)
			{
				if (unit.isAlly)
				{
					unit.ReadyUnit();
					Debug.Log("ally is woke");
				}
				else
				{
					unit.ExhaustUnit();
					Debug.Log("enemy is exhausted");
				}
			}
			else
			{
				if (!unit.isAlly)
				{
					unit.ReadyUnit();
					Debug.Log("enemy is woke");
				}
				else
				{
					unit.ExhaustUnit();
					Debug.Log("ally is exhausted");
				}
			}
		}
	}
	
	internal void CheckWinConditions()
	{
		
		if (enemyUnitCount <= 0)
		{
			//win
			Debug.Log("player wins");
			//exhaust units and disable done button
			UnitManager[] units = FindObjectsOfType(typeof(UnitManager)) as UnitManager[];
			foreach (UnitManager unit in units)
			{
				unit.ExhaustUnit();
			}
			doneButton.DeactivateButton();
		}
		else if (playerUnitCount <= 0)
		{
			//loss
			Debug.Log("enemy wins");
			//exhaust units and disable done button
			UnitManager[] units = FindObjectsOfType(typeof(UnitManager)) as UnitManager[];
			foreach (UnitManager unit in units)
			{
				unit.ExhaustUnit();
			}
			doneButton.DeactivateButton();
		}
		else
		{
			//carry on my wayward son
		}
	}
}
