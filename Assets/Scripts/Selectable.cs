﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
	protected Color originalColor;
	protected Color darkColor1;
	protected Color darkColor2;
	protected bool isActive = false;
	protected bool isSelected = false;
	private SelecterIcon selecterIcon;
	private Vector3 iconPos;
	
    void Awake()
    {
		selecterIcon = GameObject.FindObjectOfType<SelecterIcon>();
        originalColor = this.GetComponent<SpriteRenderer>().color;
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
			iconPos = this.transform.position;
			iconPos.z -= 1f;
			selecterIcon.transform.position = iconPos;
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
