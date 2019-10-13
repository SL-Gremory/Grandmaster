using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Selectable : MonoBehaviour
{
    BattleHUD battleHUD;
	protected Color originalColor;
	protected Color darkColor1;
	protected Color darkColor2;
	internal bool isActive; //if can be selected. for most things it will always be true.
	protected bool isSelected; //if is selected

    public static GameObject actionWheel;
    public static bool zoomed;
    public static GameObject currentSelected; //most recent thing clicked
    //UnitInfoUI ui = new UnitInfoUI();

    public void SetSelectStatus(bool status)
    {
        isSelected = status;
    }


    public bool GetSelectStatus()
    {
        return isSelected;
    }


    public virtual void Awake()
    {
        isActive = true;
        isSelected = false;


        originalColor = this.GetComponent<SpriteRenderer>().color;
		darkColor1 = new Color(originalColor.r-0.3f, originalColor.g-0.3f, originalColor.b-0.3f);
		darkColor2 = new Color(originalColor.r-0.6f, originalColor.g-0.6f, originalColor.b-0.6f);
        //actionWheel = GameObject.Find("Default Radial Menu");
        //battleHUD = GameObject.Find("HUD").GetComponent<BattleHUD>();
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

	public void OnMouseOver()
	{
		if (isActive && Input.GetMouseButtonDown(0))
		{
			Debug.Log("Selected a selectable");
            //battleHUD.DisplayInfo(gameObject);
            if (isSelected)
			{
				isSelected = false;
				SelectThis(false);
				Debug.Log("Deselected a selectable via toggle");
                zoomed = false;
                currentSelected = null;
            }
            else
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
				SelectThis(true);

            }

        }


    }

	public void SelectThis(bool yes)
	{
		if (yes)
		{
            BattleState.SelectUnit(gameObject);
            SelecterIcon.targetObject = this.gameObject;
            currentSelected = this.gameObject;
            zoomed = true;
            //ui.DisplayInformation(gameObject);

        }
        else
		{
            SelecterIcon.targetObject = null;
            SelecterIcon.ResetPosition();
            zoomed = false;
            currentSelected = null;
            //ui.ResetDisplay();
        }
    }




	public void Highlight(bool highlighted)
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

	public void ChangeColor(int choice)
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
