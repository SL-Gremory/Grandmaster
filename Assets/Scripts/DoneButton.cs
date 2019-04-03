using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoneButton : Selectable
{
	private EventManager eventManager;
	
    // Start is called before the first frame update
    void Start()
    {
		eventManager = GameObject.FindObjectOfType<EventManager>();
		ResetButton(eventManager.isPlayerTurn);
    }
	
	void Update()
	{
		//SPACE to activate when selected
		if (isSelected && Input.GetKeyDown("space"))
		{
			eventManager.turnIsDone = true;
			Debug.Log("Player declared end of turn");
		}
	}
	
	public void ResetButton(bool isPlayerTurn)
	{
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
	
	public void BrightenButton()
	{
		ChangeColor(0);
	}
	
	public void ActivateButton()
	{
		isActive = true;
	}
	
	public void DeactivateButton()
	{
		isActive = false;
	}
}