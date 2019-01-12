using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    //Initialize variables
	private bool isPlayerTurn = true;
	private bool turnIsDone = false;
	public bool playerFirst; //must define as true or false in editor, on prefab, or when spawning object
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
		
		//goes through every object of type Unit wakes up allies or enemies appropriately
		UnitManager[] units = FindObjectsOfType(typeof(UnitManager)) as UnitManager[];
		foreach (UnitManager unit in units)
		{
			if (unit.isAlly && isPlayerTurn)
			{
				unit.moveIsDone = false;
				unit.actionIsDone = false;
				unit.RevertColor();//DELETE THIS LINE FOR THE REAL THING
				Debug.Log("ally is woke");
			}
			else if (!unit.isAlly && !isPlayerTurn)
			{
				unit.moveIsDone = false;
				unit.actionIsDone = false;
				unit.RevertColor();//DELETE THIS LINE FOR THE REAL THING
				Debug.Log("enemy is woke");
			}
		}
	}
}
