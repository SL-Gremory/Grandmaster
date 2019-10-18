using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UnitInfoUI : MonoBehaviour
{

    Text infoText;

    //GameObject IconDisplay;
    Text StatsDisplay;
    Text AffinitiesDisplay;

    private void Start()
    {
        //infoText = GetComponentInParent<Text>();
        //IconDisplay = GameObject.Find("HUD Icon").GetComponent<Text>();
        //StatsDisplay = GameObject.Find("HUD Stats").GetComponent<Text>();
        //AffinitiesDisplay = GameObject.Find("HUD Affinities").GetComponent<Text>();
    }


    public void DisplayStats(GameObject unit)
    {
        UnitData unitData = unit.GetComponent<UnitData>();
        Parameters unitParam = unit.GetComponent<Parameters>();

        infoText.text =
            unitData.UnitName + "\t" + "Lv." + unitParam.LVL +
            "\nEXP: " + unitParam.EXP +
            "\nHP: " + unitParam.CHP + "/" + unitParam.MHP +
            "\nMP: " + unitParam.CMP + "/" + unitParam.MMP +
            "\nATK: " + unitParam.ATK + 
            "\nDEF: " + unitParam.DEF +
            "\nSPR: " + unitParam.SPR +
            "\nSPD: " + unitParam.SPD +
            "\nMOV: " + unitParam.MOV +
            "\nJMP: " + unitParam.JMP;
    }

    public void DisplayInformation(GameObject selected)
    {
        UnitData unitData = selected.GetComponent<UnitData>();
        Parameters unitParameters = selected.GetComponent<Parameters>();

        StatsDisplay.text = "Stats for " + unitData.UnitName;
        AffinitiesDisplay.text = "Affinities for " + unitData.UnitName;


    }

    public void ResetDisplay()
    {
        //infoText = default;
        StatsDisplay = default;
        AffinitiesDisplay = default;
        
    }

}
