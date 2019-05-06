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
	private BattleManager battleManager;
	private BattleNavigate battleNavigate;

	[SerializeField]
	internal bool isAlly; //must define as true or false in editor, on prefab, or when spawning object

    public bool IsAlly { get { return isAlly; } }

    private bool moveIsDone;
	private bool actionIsDone;

    Int2 currentUnitPosition = new Int2();

    void Start()
    {
		battleManager = BattleManager.Instance;
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




    /*
    void RegisterToBattleManager()
    {
        var battleNavigator = this.GetComponentInParent<BattleNavigate>();
        Int2 unitPosition = battleNavigator.GetUnitPosition();

        BattleManager.Instance.AddUnit(unitPosition, this);
    }
    */

    void Update()
    {
        // Maybe this shouldn't be here...
        currentUnitPosition = battleNavigate.GetUnitPosition();

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
            /*
			if (Input.GetKeyDown(KeyCode.K))
			{
				KillUnit();
			}
            */
        }
    }

    public void AttackUnit(Int2 dPos)
    {
        if (Int2.Distance(currentUnitPosition, dPos) > 1)
        {
            Debug.Log("That unit is too far to attack");
            return;
        }

        BattleManager.Instance.PrepareAttack(currentUnitPosition, dPos);
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
