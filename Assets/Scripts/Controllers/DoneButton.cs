using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoneButton : Selectable
{
	private TurnManager turnManager;
	
    // Start is called before the first frame update
    void Start()
    {
		turnManager = TurnManager.Instance;
		ResetButton(turnManager.isPlayerTurn);
    }
	
	void Update()
	{
		//SPACE to activate when selected
		if (isSelected) //&& Input.GetKeyDown("space"))
		{
            Selectable.zoomed = false;
            Selectable.currentSelected = null;
			turnManager.turnIsDone = true;
			isSelected = false; //failsafe to prevent multiple button presses
			SelectThis(false); //remove selecter icon before it shows up on button
			Debug.Log("Player declared end of turn");
		}
	}
	
	internal void ResetButton(bool isPlayerTurn)
	{
		SelectThis(false); //reset selecter icon at change of turn
		
		if (isPlayerTurn)
		{
			ChangeColor(1);
			ActivateButton();
		}
		else
		{
			ChangeColor(2);
			DeactivateButton();
		}
	}
	
	internal void BrightenButton()
	{
		ChangeColor(0);
	}
	
	internal void ActivateButton()
	{
		isActive = true;
	}
	
	internal void DeactivateButton()
	{
		isActive = false;
	}
}