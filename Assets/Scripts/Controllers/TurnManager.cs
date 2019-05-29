using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    [SerializeField]
	private DoneButton doneButton;
    [SerializeField]
	private TurnCountText turnCountText;
	[SerializeField]
	private bool playerFirst; //must define as true or false in editor, on prefab, or when spawning object

	internal bool turnIsDone = false;
	internal bool isPlayerTurn;
	internal int playerUnitCount = 0;
	internal int enemyUnitCount = 0;
	internal int unitsDone = 0;
	internal int turnCounter = 1;

    public static TurnManager Instance { get; private set; }

	void Start()
	{
        if (Instance != null)
            Debug.LogError("There can't be multiple BattleManagers in the scene.");
        Instance = this;
        //var mapSize = LevelGrid.Instance.GetMapSize();
        //unitsGrid = new Unit[mapSize.x, mapSize.y];

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
		Unit[] units = FindObjectsOfType(typeof(Unit)) as Unit[];

        foreach (Unit unit in units)
		//foreach (Unit unit in unitsGrid)
		{
			Debug.Log("finded a unit");
            var unitInfo = unit.GetComponentInParent<Unit>();
			if (isPlayerTurn)
			{
				if (unitInfo.GetUnitAffiliation() == Team.HERO)
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
				if (unitInfo.GetUnitAffiliation() == Team.ENEMY)
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

    // I already have Win/Lose conditions in BattleControl, we should merge them  <--------------------------
    internal void CheckWinConditions()
	{
		if (enemyUnitCount <= 0)
		{
			Debug.Log("player wins");
            foreach (Unit unit in BattleNavigate.unitsGrid)
			{
				unit.ExhaustUnit();
			}
			doneButton.DeactivateButton();
		}
		else if (playerUnitCount <= 0)
		{
			//loss
			Debug.Log("enemy wins");
			foreach (Unit unit in BattleNavigate.unitsGrid)
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
