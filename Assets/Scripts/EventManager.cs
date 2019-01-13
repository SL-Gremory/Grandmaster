using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    //Initialize variables
	private bool isPlayerTurn;
	private bool turnIsDone = false;
	public bool playerFirst; //must define as true or false in editor, on prefab, or when spawning object
	public int playerUnitCount = 0;
	public int enemyUnitCount = 0;
	public int unitsDone = 0;
	
	// Awake is called before Start
	void Awake()
	{
		if (playerFirst)
		{
			isPlayerTurn = true;
		}
		else
		{
			isPlayerTurn = false;
		}
	}
	
	// Start is called before the first frame update
    /*void Start()
    {
        
    }*/
	
    // Update is called once per frame
    void Update()
    {
        if (turnIsDone)
		{
			isPlayerTurn = !isPlayerTurn;
			ResetCounts();
			turnIsDone = false;
		}
		if (isPlayerTurn && playerUnitCount == unitsDone || !isPlayerTurn && enemyUnitCount == unitsDone)
		{
			turnIsDone = true;
		}
    }
	
	void ResetCounts()
	{	
		unitsDone = 0;
		
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
}
