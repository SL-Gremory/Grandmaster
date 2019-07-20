using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Attack
{

    public static int CalculateProjectedDamage(GameObject attacker, GameObject defender)
    {
        UnitData aUnitData = attacker.GetComponent<UnitData>();
        UnitData dUnitData = defender.GetComponent<UnitData>();

        Parameters aParam = attacker.GetComponent<Parameters>();
        Parameters dParam = defender.GetComponent<Parameters>();


        // Speed not being used to calculate doubled damage yet

        int total;
        double s1 = (double)aParam.SPD;      // Attacker speed
        double s2 = (double)dParam.SPD;      // Defender speed
        double a = (double)aParam.ATK;       // Attack
        double ad = 1d;                       // Attack Debuff
        double ab = 1d;                       // Attack Buff
        double d = (double)dParam.DEF;       // Enemy Defense
        double dd = 1d;                       // Defense Debuff
        double db = 1d;                       // Defense Buff
        double e = 1d;                        // Elemental Strength/Weakness
        double x = 1d;                        // Multiplier
        double t = 1d;                        // Triangle bonus +/- 20%
        double f = 0d;                        // Flat damage


        t = TriangleCalculation(attacker, defender);

        total = (int)Math.Floor(((1 / e) * (t * ((x * (a * (ad + ab))) - (d * (dd + db))))) + f);

        return (total > 0) ? total : 0;
    }

    private static double TriangleCalculation(GameObject attacker, GameObject defender)
    {
        Weapon a = attacker.GetComponent<UnitData>().UnitWeapon;
        Weapon d = defender.GetComponent<UnitData>().UnitWeapon;

        if (a == null)
            return 1;

        switch(a.type)
        {
            case WeaponType.RANGED_PHYSICAL:
                RangedPhysicalTriangle(a, d);
                break;
            case WeaponType.RANGED_MAGICAL:
                return RangedMagicalTriangle(a, d);
            case WeaponType.MELEE_PHYSICAL:
                return MeleePhysicalTriangle(a, d);
            case WeaponType.MELEE_AURA:
                return MeleeAuraTriangle(a, d);
        }

        return 1;
    }

    private static double MeleeAuraTriangle(Weapon a, Weapon d)
    {
        return 1d;
    }

    private static double RangedPhysicalTriangle(Weapon a, Weapon d)
    {
        return 1d;
    }

    private static double MeleePhysicalTriangle(Weapon a, Weapon d)
    {
        if (d.type != WeaponType.MELEE_PHYSICAL)
            return 1d;

        if (a.mpAttribute == MeleePhysicalAttribute.SLASH && d.mpAttribute == MeleePhysicalAttribute.BLUNT)
            return Consts.BATTLE_AFFINITY_ADVANTAGE;
        else if (a.mpAttribute == MeleePhysicalAttribute.SLASH && d.mpAttribute == MeleePhysicalAttribute.PIERCE)
            return Consts.BATTLE_AFFINITY_DISADVANTAGE;
        else if (a.mpAttribute == MeleePhysicalAttribute.BLUNT && d.mpAttribute == MeleePhysicalAttribute.PIERCE)
            return Consts.BATTLE_AFFINITY_ADVANTAGE;
        else if (a.mpAttribute == MeleePhysicalAttribute.BLUNT && d.mpAttribute == MeleePhysicalAttribute.SLASH)
            return Consts.BATTLE_AFFINITY_DISADVANTAGE;
        else if (a.mpAttribute == MeleePhysicalAttribute.PIERCE && d.mpAttribute == MeleePhysicalAttribute.SLASH)
            return Consts.BATTLE_AFFINITY_ADVANTAGE;
        else if (a.mpAttribute == MeleePhysicalAttribute.PIERCE && d.mpAttribute == MeleePhysicalAttribute.BLUNT)
            return Consts.BATTLE_AFFINITY_DISADVANTAGE;
        else
            return 1d;

    }

    private static double RangedMagicalTriangle(Weapon a, Weapon d)
    {
        // If different weapons, return default multiplier
        if (d.type != WeaponType.RANGED_MAGICAL) 
            return 1d;


        /*
         * Fire beats Ice
         * Ice beats Lightning
         * Ice beats Fire
         * Light and Dark beat eachother
         */

        if (a.rmAttribute == RangedMagicalAttribute.FIRE && d.rmAttribute == RangedMagicalAttribute.ICE)
            return Consts.BATTLE_AFFINITY_ADVANTAGE;
        else if (a.rmAttribute == RangedMagicalAttribute.FIRE && d.rmAttribute == RangedMagicalAttribute.LIGHTNING)
            return Consts.BATTLE_AFFINITY_DISADVANTAGE;
        else if (a.rmAttribute == RangedMagicalAttribute.ICE && d.rmAttribute == RangedMagicalAttribute.LIGHTNING)
            return Consts.BATTLE_AFFINITY_ADVANTAGE;
        else if (a.rmAttribute == RangedMagicalAttribute.ICE && d.rmAttribute == RangedMagicalAttribute.FIRE)
            return Consts.BATTLE_AFFINITY_DISADVANTAGE;
        else if (a.rmAttribute == RangedMagicalAttribute.LIGHTNING && d.rmAttribute == RangedMagicalAttribute.FIRE)
            return Consts.BATTLE_AFFINITY_ADVANTAGE;
        else if (a.rmAttribute == RangedMagicalAttribute.LIGHTNING && d.rmAttribute == RangedMagicalAttribute.ICE)
            return Consts.BATTLE_AFFINITY_DISADVANTAGE;
        else if (a.rmAttribute == RangedMagicalAttribute.LIGHT && (d.rmAttribute == RangedMagicalAttribute.DARK || d.rmAttribute == RangedMagicalAttribute.LIGHT))
            return Consts.BATTLE_AFFINITY_ADVANTAGE;
        else
            return 1d;






        // TODO: Curse affinities (wind, earth, and water) must have advantages against movement types

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
