using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EndCondition {
    NEVER,
    noEnemies,
    afterTenSeconds,
    escaped
}

public static class BattleJudges 
{
    static readonly System.Predicate<BattleState>[] endConditions = new System.Predicate<BattleState>[] {
        returnsFalse,
        enemiesCleared,
        hasTenSecondsPassed,
        reachedEscape
    };


    public static bool JudgeEnd(EndCondition cond, BattleState state) {
        return endConditions[(int)cond](state);
    }

    static bool returnsFalse(BattleState state) {
        return false;
    }

    static bool hasTenSecondsPassed(BattleState state) {
        if (state.RealTimeElapsed >= 10)
        {
            Debug.Log("Ten seconds have passed");
            return true;
        }

        return false;
    }

    static bool enemiesCleared(BattleState state) {
        if (state.GetEnemyUnitCount() == 0)
        {
            Debug.Log("ENEMIES DED");
            return true;
        }
            

        return false;
    }

    static bool reachedEscape(BattleState state)
    {
        return false;
    }
}


