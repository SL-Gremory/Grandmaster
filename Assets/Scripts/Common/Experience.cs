using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Experience
{
    // This returns the level a unit would be if they had some total of accumulated experience
    public static int LevelForExperience(int exp)
    {
        int level = Consts.GAME_MAX_LEVEL;

        while (level >= Consts.GAME_MIN_LEVEL)
        {
            if (exp >= ExperienceForLevel(level))
                break;
            --level;
        }
        return level;
    }

    // This returns the grand total number of accumulated experience required to reach a certain level
    public static int ExperienceForLevel(int level)
    {
        float levelPercent = Mathf.Clamp01((float)(level - Consts.GAME_MIN_LEVEL) / (float)(Consts.GAME_MAX_LEVEL - Consts.GAME_MIN_LEVEL));
        int totalExperience = (int)EasingEquations.EaseInQuad(0, Consts.GAME_MAX_TOTAL_EXP, levelPercent);
        return totalExperience;
    }

}

