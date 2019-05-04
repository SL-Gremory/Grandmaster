using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewStats : MonoBehaviour
{
    [SerializeField] Text LVL;
    [SerializeField] Text EXP;
    [SerializeField] Text HP;
    [SerializeField] Text MP;
    [SerializeField] Text ATK;
    [SerializeField] Text DEF;
    [SerializeField] Text SPR;
    [SerializeField] Text SPD;
    [SerializeField] Text MOV;
    [SerializeField] Text JMP;


    public void Display(GrandmasterUnit unit)
    {
        this.LVL.text = "Lvl:\t" + unit.LVL.ToString();
        this.EXP.text = "Exp:\t" + unit.EXP.ToString();
        this.HP.text = "HP:\t" + unit.CHP.ToString() + " / " + unit.MHP.ToString();
        this.MP.text = "MP:\t" + unit.CMP.ToString() + " / " + unit.MMP.ToString();
        this.ATK.text = "Atk:\t" + unit.ATK.ToString();
        this.DEF.text = "Def:\t" + unit.DEF.ToString();
        this.SPR.text = "Spr:\t" + unit.SPR.ToString();
        this.SPD.text = "Spd:\t" + unit.SPD.ToString();
        this.MOV.text = "Mov:\t" + unit.MOV.ToString();
        this.JMP.text = "Jmp:\t" + unit.JMP.ToString();
    }

}
