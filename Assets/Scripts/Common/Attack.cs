using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Attack
{

    public static int CalculateProjectedDamage(GrandmasterUnit attacker, GrandmasterUnit defender)
    {
        // Speed not being used to calculate doubled damage yet

        int total;
        double s1 = (double)attacker.SPD;    // Attacker speed
        double s2 = (double)defender.SPD;    // Defender speed
        double a = (double)attacker.ATK;     // Attack
        double ad = 1;                  // Attack Debuff
        double ab = 1;                  // Attack Buff
        double d = (double)defender.DEF;     // Enemy Defense
        double dd = 1;                  // Defense Debuff
        double db = 1;                  // Defense Buff
        double e = 1;                   // Elemental Strength/Weakness
        double x = 1;                   // Multiplier
        double t = 1;                   // Triangle bonus +/- 20%
        double f = 0;                   // Flat damage

        total = (int)Math.Floor(((1 / e) * (t * ((x * (a * (ad + ab))) - (d * (dd + db))))) + f);

        return (total > 0) ? total : 0;
    }


    public static void CommenceBattle(GrandmasterUnit attacker, GrandmasterUnit defender)
    {
        // NOTE: The attacker will always strike first currently
        //       Anything skills/items involving priority-manipulation 
        //       is not currently accounted for

        int attackerDamage = CalculateProjectedDamage(attacker, defender);
        int defenderDamage = CalculateProjectedDamage(defender, attacker);

        if (defender.CHP > attackerDamage)
        {
            defender.CHP -= attackerDamage;

            if (attacker.CHP > defenderDamage)
            {
                attacker.CHP -= defenderDamage;
            }
            else
            {
                attacker.CHP = 0;
            }
        }
        else
        {
            defender.CHP = 0;
        }


        Debug.Log(string.Format("{0} has {1} left", attacker.GetUnitName(), attacker.CHP));
        Debug.Log(string.Format("{0} has {1} left", defender.GetUnitName(), defender.CHP));

    }

}
