using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Selectable : MonoBehaviour
{
	protected Color originalColor;
	protected Color darkColor1;
	protected Color darkColor2;
	protected bool isActive = false; //if can be selected
	protected bool isSelected = false; //if is selected
	
    void Awake()
    {
        originalColor = GetComponent<SpriteRenderer>().color;
		darkColor1 = new Color(originalColor.r-0.3f, originalColor.g-0.3f, originalColor.b-0.3f);
		darkColor2 = new Color(originalColor.r-0.6f, originalColor.g-0.6f, originalColor.b-0.6f);
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
	
	protected void OnMouseOver()
	{
		if (isActive && Input.GetMouseButtonDown(0))
		{
			Debug.Log("Selected a selectable");
			//find all selectables; if is selected, revert selection
			Selectable[] selectables = FindObjectsOfType(typeof(Selectable)) as Selectable[];
			foreach (Selectable selectable in selectables)
			{
				if (selectable.isSelected)
				{
					selectable.isSelected = false;
				}
			}
			//select only the clicked selectable
			isSelected = true;
			SelectThis(true);
		}
	}
	
	protected void SelectThis(bool yes)
	{
		if (yes)
		{
			SelecterIcon.targetObject = this.gameObject;
		}
		else
		{
            SelecterIcon.targetObject = null;
            SelecterIcon.ResetPosition();
		}
	}
	
	protected void Highlight(bool highlighted)
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
	
	protected void ChangeColor(int choice)
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
