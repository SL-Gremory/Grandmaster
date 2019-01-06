using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    //Initialize variables
	bool isPlayerTurn = true;
	bool turnIsDone = false;
	public int playerUnitCount = 0;
	public int enemyUnitCount = 0;
	public int unitsDone = 0;
	
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
		if (isPlayerTurn)
		{
			//goes through every UnitAlly and wakes them up
			UnitAlly[] allies = FindObjectsOfType(typeof(UnitAlly)) as UnitAlly[];
			foreach (UnitAlly ally in allies)
			{
				ally.moveIsDone = false;
				ally.actionIsDone = false;
				ally.RevertColor();//DELETE THIS LINE FOR THE REAL THING
				Debug.Log("ally is woke");
			}
		}
		else
		{
			//goes through every UnitEnemy and wakes them up
			UnitEnemy[] enemies = FindObjectsOfType(typeof(UnitEnemy)) as UnitEnemy[];
			foreach (UnitEnemy enemy in enemies)
			{
				enemy.moveIsDone = false;
				enemy.actionIsDone = false;
				enemy.RevertColor();//DELETE THIS LINE FOR THE REAL THING
				Debug.Log("enemy is woke");
			}
		}
	}
}
