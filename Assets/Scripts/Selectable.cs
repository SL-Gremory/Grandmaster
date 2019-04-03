using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
	private Color originalColor;
	private Color darkColor1;
	private Color darkColor2;
	private bool isActive = true; //should be false by default. set to true for testing.
	private bool isSelected = false;
	
    void Start()
    {
        originalColor = this.GetComponent<SpriteRenderer>().color;
		darkColor1 = originalColor; darkColor1.r -= 0.3f; darkColor1.g -= 0.3f; darkColor1.b -= 0.3f; //excuse my bullshit
		darkColor2 = originalColor; darkColor2.r -= 0.6f; darkColor2.g -= 0.6f; darkColor2.b -= 0.6f; //excuse my bullshit
    }

    void OnMouseEnter()
	{
		if (isActive)
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
		if (Input.GetMouseButtonDown(0))
		{
			//find all selectables; if is selected, revert selection and color
			Selectable[] selectables = FindObjectsOfType(typeof(Selectable)) as Selectable[];
			foreach (Selectable selectable in selectables)
			{
				if (selectable.isSelected)
				{
					selectable.isSelected = false;
					selectable.ChangeColor(0);
				}
			}
			//make the clicked selectable selected
			isSelected = true;
			ChangeColor(1);
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
