using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoneButton : MonoBehaviour
{
	//Initialize variables
	private EventManager eventManager;
	private Color originalColor;
	private Color darkColor1;
	private Color darkColor2;
	public bool isEnabled;
	
    // Start is called before the first frame update
    void Start()
    {
		eventManager = GameObject.FindObjectOfType<EventManager>();
        originalColor = this.GetComponent<SpriteRenderer>().color;
		darkColor1 = originalColor; darkColor1.r -= 0.3f; darkColor1.g -= 0.3f; darkColor1.b -= 0.3f; //excuse my bullshit
		darkColor2 = originalColor; darkColor2.r -= 0.6f; darkColor2.g -= 0.6f; darkColor2.b -= 0.6f; //excuse my bullshit
		ResetButton(eventManager.isPlayerTurn);
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/
	
	public void ResetButton(bool isPlayerTurn)
	{
		if (isPlayerTurn)
		{
			ChangeColor(1);
			isEnabled = true;
		}
		else
		{
			ChangeColor(2);
			isEnabled = false;
		}
	}
	
	public void BrightenButton()
	{
		ChangeColor(0);
	}
	
	void OnMouseEnter()
	{
		if (isEnabled)
		{
			Highlight(true);
		}
	}
	
	void OnMouseExit()
	{
		Highlight(false);
	}
	
	void OnMouseOver()
	{
		//Left click to activate
		if (Input.GetMouseButtonDown(0) && isEnabled)
		{
			eventManager.turnIsDone = true;
			Debug.Log("Player declared end of turn");
		}
	}
	
	void Highlight(bool highlighted)
	{
		//not a true highlight, just changes the transparency
		Color tmp = this.GetComponent<SpriteRenderer>().color;
		if(highlighted)
		{
			tmp.a = 0.7f;
		}
		else
		{
			tmp.a = 1f;
		}
		this.GetComponent<SpriteRenderer>().color = tmp;
	}
	
	void ChangeColor(int choice)
	{
		if (choice == 0)
		{
			this.GetComponent<SpriteRenderer>().color = originalColor;
		}
		else if (choice == 1)
		{
			this.GetComponent<SpriteRenderer>().color = darkColor1;
		}
		else if (choice == 2)
		{
			this.GetComponent<SpriteRenderer>().color = darkColor2;
		}
	}
}