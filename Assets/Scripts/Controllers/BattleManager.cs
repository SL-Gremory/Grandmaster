using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
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

    public static Unit[,] unitsGrid;
    public static BattleManager Instance { get; private set; }

	void Start()
	{
        if (Instance != null)
            Debug.LogError("There can't be multiple BattleManagers in the scene.");
        Instance = this;
        var mapSize = LevelGrid.Instance.GetMapSize();
        unitsGrid = new Unit[mapSize.x, mapSize.y];

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


    public void PrepareAttack(Int2 aPos, Int2 dPos)
    {

        // This is dirty REEEEE
        Unit defender = unitsGrid[dPos.x, dPos.y].GetComponentInParent<Unit>();
        Unit attacker = unitsGrid[aPos.x, aPos.y].GetComponentInParent<Unit>();
        Attack.CommenceBattle(attacker, defender);

        if (attacker.CHP <= 0)
        {
            Debug.Log(string.Format("{0} has died", attacker.GetUnitName()));
            //Destroy(attacker);
        }

        if (defender.CHP <= 0)
        {
            Debug.Log(string.Format("{0} has died", defender.GetUnitName()));
            //Destroy(defender);
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
                if(unit.isAlly)
				//if (unitInfo.GetUnitAffiliation() == Team.HERO)
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
                if(!unit.isAlly)
				//if (unitInfo.GetUnitAffiliation() == Team.ENEMY)
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
			//win
			Debug.Log("player wins");
            //exhaust units and disable done button
            //Unit[] units = FindObjectsOfType(typeof(Unit)) as Unit[];

            //foreach (Unit unit in units)
            foreach (Unit unit in unitsGrid)
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
			//Unit[] units = FindObjectsOfType(typeof(Unit)) as Unit[];
			foreach (Unit unit in unitsGrid)
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

    public bool IsEnemyAt(Int2 pos) {
        return unitsGrid[pos.x, pos.y] != null && !unitsGrid[pos.x, pos.y].IsAlly;
    }

    public bool IsUnitAt(Int2 pos) {
        return unitsGrid[pos.x, pos.y] != null;
    }

    public void AddUnit(Int2 pos, Unit unit) {
        if (unitsGrid[pos.x, pos.y] != null)
            Debug.LogError("Logic error, trying to place one unit on top of another. " + unitsGrid[pos.x, pos.y].GetUnitName() + ", " + unit.GetUnitName(), this);
        unitsGrid[pos.x, pos.y] = unit;
    }

    public void RemoveUnit(Int2 pos) {
        if (unitsGrid[pos.x, pos.y] == null)
            Debug.LogWarning("Trying to remove a unit from empty position, probably an error. " + pos, this);
        unitsGrid[pos.x, pos.y] = null;
    }

}
