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


    public void DisplayStats(Unit unit)
    {
        infoText.text =
            unit.name + "\t" + "Lv." + unit.LVL +
            "\nEXP: " + unit.EXP +
            "\nHP: " + unit.CHP + "/" + unit.MHP +
            "\nMP: " + unit.CMP + "/" + unit.MMP +
            "\nATK: " + unit.ATK + 
            "\nDEF: " + unit.DEF +
            "\nSPR: " + unit.SPR +
            "\nSPD: " + unit.SPD +
            "\nMOV: " + unit.MOV +
            "\nJMP: " + unit.JMP;
    }

    public void ResetDisplay()
    {
        infoText = default;
    }

}
