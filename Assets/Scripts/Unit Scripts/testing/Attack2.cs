using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Attack2
{

    public static int CalculateProjectedDamage(GameObject attacker, GameObject defender)
    {

        Parameters aParam = attacker.GetComponent<Parameters>();
        Parameters dParam = defender.GetComponent<Parameters>();


        // Speed not being used to calculate doubled damage yet

        int total;
        double s1 = (double)aParam.SPD;   // Attacker speed
        double s2 = (double)dParam.SPD;    // Defender speed
        double a = (double)aParam.ATK;     // Attack
        double ad = 1;                       // Attack Debuff
        double ab = 1;                       // Attack Buff
        double d = (double)dParam.DEF;     // Enemy Defense
        double dd = 1;                       // Defense Debuff
        double db = 1;                       // Defense Buff
        double e = 1;                        // Elemental Strength/Weakness
        double x = 1;                        // Multiplier
        double t = 1;                        // Triangle bonus +/- 20%
        double f = 0;                        // Flat damage

        total = (int)Math.Floor(((1 / e) * (t * ((x * (a * (ad + ab))) - (d * (dd + db))))) + f);

        return (total > 0) ? total : 0;
    }


    public static void CommenceBattle(GameObject attacker, GameObject defender)
    {
        Parameters aParam = attacker.GetComponent<Parameters>();
        UnitStateController aControl = attacker.GetComponent<UnitStateController>();
        Parameters dParam = defender.GetComponent<Parameters>();
        UnitStateController dControl = defender.GetComponent<UnitStateController>();



        // NOTE: The attacker will always strike first currently
        //       Anything skills/items involving priority-manipulation 
        //       is not currently accounted for

        int attackerDamage = CalculateProjectedDamage(attacker, defender);
        int defenderDamage = CalculateProjectedDamage(defender, attacker);

        if (dParam.CHP > attackerDamage)
        {
            dParam.CHP -= attackerDamage;


            // Note: defender may have shorter range than attack
            if (aParam.CHP > defenderDamage)
            {
                aParam.CHP -= defenderDamage;
            }
            else
            {
                aParam.CHP = 0;
            }
        }
        else
        {
            dParam.CHP = 0;
        }


        aControl.DoneActing();
    }

}
