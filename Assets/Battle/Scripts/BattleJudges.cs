using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EndCondition {
    NEVER,
    afterTenSeconds
}

public static class BattleJudges 
{
    static readonly System.Predicate<BattleState>[] endConditions = new System.Predicate<BattleState>[] {
        returnsFalse,
        hasTenSecondsPassed
    };

    public static bool JudgeEnd(EndCondition cond, BattleState state) {
        return endConditions[(int)cond](state);
    }

    static bool returnsFalse(BattleState state) {
        return false;
    }

    static bool hasTenSecondsPassed(BattleState state) {
        return state.RealTimeElapsed >= 10;
    }
}
