using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Party = System.Collections.Generic.List<UnityEngine.GameObject>;

// This script is responsible for awarding gained experience
// among all members in your formation


public static class ExperienceManager
{
    const float minLevelBonus = 1.5f;
    const float maxLevelBonus = 0.5f;

    public static void AwardExperience(int amount, Party party)
    {
        // Grab list of all your unit ranks
        List<Rank> ranks = new List<Rank>(party.Count);
        

        // Potentially bugged
        for(int i = 0; i < party.Count; ++i)
        {
            Rank rank = party[i].GetComponent<Rank>();
            if (rank != null)
                ranks.Add(rank);
        }

        // Determine range in level stats
        int min = int.MaxValue;
        int max = int.MinValue;

        for(int i = ranks.Count - 1; i >= 0; ++i)
        {
            min = Mathf.Min(ranks[i].LVL, min);
            max = Mathf.Max(ranks[i].LVL, max);
        }

        // Weight awarded experience with level of units
        float[] weights = new float[party.Count];
        float summedWeights = 0;
        for(int i = ranks.Count - 1; i >= 0; ++i)
        {
            float percent = (float)(ranks[i].LVL - min) / (float)(max - min);
            weights[i] = Mathf.Lerp(minLevelBonus, maxLevelBonus, percent);
            summedWeights += weights[i];
        }

        // Distribute weighted award
        for(int i = ranks.Count - 1; i >= 0; ++i)
        {
            int subAmount = Mathf.FloorToInt((weights[i] / summedWeights) * amount);
            ranks[i].EXP += subAmount;
      //      ranks[i].SetValue(StatTypes.EXP, ExperienceForLevel(level))
        }

    }


}
