using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UnitInfoUI : MonoBehaviour
{

    Text infoText;

    private void Start()
    {
        infoText = GetComponentInParent<Text>();
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

    public void ResetDisplay()
    {
        infoText = default;
    }

}
