using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngageBattleState : MonoBehaviour
{
    GameObject attackerObject;
    GameObject defenderObject;
    GrandmasterUnit attacker;
    GrandmasterUnit defender;

    public void InitializeBattle()
    {
        attacker = attackerObject.GetComponent<GrandmasterUnit>();
        defender = defenderObject.GetComponent<GrandmasterUnit>();

        CommenceBattle();
    }

    public void CommenceBattle()
    {
        // NOTE: The attacker will always strike first currently
        //       Anything skills/items involving priority-manipulation 
        //       is not currently accounted for

        int attackerDamage = DamageCalculation(attacker, defender);
        int defenderDamage = DamageCalculation(defender, attacker);
        
        if(defender.CHP > attackerDamage)
        {
            defender.CHP -= attackerDamage;

            if(attacker.CHP > defenderDamage)
            {
                attacker.CHP -= defenderDamage;
            } else
            {
                attacker.CHP = 0;
            }
        } else
        {
            defender.CHP = 0;
        }

    }

    public int DamageCalculation(GrandmasterUnit unitA, GrandmasterUnit unitB)
    { 

        // Speed not being used to calculate doubled damage yet
   
        double s1 = (double)unitA.SPD;    // Attacker speed
        double s2 = (double)unitB.SPD;    // Defender speed
        double a = (double)unitA.ATK;     // Attack
        double ad = 1f;                  // Attack Debuff
        double ab = 1f;                  // Attack Buff
        double d = (double)unitB.DEF;     // Enemy Defense
        double dd = 1f;                  // Defense Debuff
        double db = 1f;                  // Defense Buff
        double e = 1f;                   // Elemental Strength/Weakness
        double x = 1f;                   // Multiplier
        double t = 1f;                   // Triangle bonus +/- 20%
        double f = 0f;                   // Flat damage

        return (int)Math.Floor((1 / e) * (t * ((x * (a * (ad + ab))) - (d * (dd + db)))) + f);
    }

}
