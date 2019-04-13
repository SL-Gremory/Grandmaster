using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Party = System.Collections.Generic.List<UnityEngine.GameObject>;

public class LevelGrowthTest : MonoBehaviour
{

    void Start()
    {
        VerifyLevelToExperienceCalculations();
        //VerifySharedExperienceDistribution();
    }

    // Test awarding exeperience to party members
    // Members initialized to different levels
    void VerifySharedExperienceDistribution()
    {
        string[] names = new string[] {"Emmy"};

        Party heroes = new Party();

        for(int i = 0; i < names.Length; ++i)
        {
            GameObject actor = new GameObject(names[i]);
            actor.AddComponent<Stats>();
            Rank rank = actor.AddComponent<Rank>();
            rank.Init((int)UnityEngine.Random.Range(1, 5));
            heroes.Add(actor);
        }

        Debug.Log("-- BEFORE ADDING EXPERIENCE --");
        PrintAuxiliary(heroes);

        ExperienceManager.AwardExperience(450, heroes);

        Debug.Log("-- AFTER ADDING EXPERIENCE --");
        PrintAuxiliary(heroes);

    }

    // Loop through levels 1 through 40 to make sure the sum total
    // of experience is correct for each level
    void VerifyLevelToExperienceCalculations()
    {

        for (int i = 1; i <= 40; ++i)
        {
            int expForLevel = Rank.ExperienceForLevel(i);
            int levelForExp = Rank.LevelForExperience(expForLevel);

            
            if(levelForExp != i)
            {
                Debug.Log(string.Format("Mismatch on level: {0} with exp: {1} returned {2}", i, expForLevel, levelForExp));
            }
            else
            {
                Debug.Log(string.Format("Level: {0} = Exp: {1}", levelForExp, expForLevel));
            }
            
            //Debug.Log(string.Format("Level: {0} = Exp: {1}", i, expForLevel));

            //Debug.Log(string.Format("Currently on {0} but found {1}", i, levelForExp));
        }
    }

    void PrintAuxiliary(Party heroes)
    {
        for(int i = 0; i < heroes.Count; ++i)
        {
            GameObject actor = heroes[i];
            Rank rank = actor.GetComponent<Rank>();
            Debug.Log(string.Format("Name: {0} Level: {1} Exp: {2}", actor.name, rank.LVL, rank.EXP));
        }
    }

    void OnLevelChange(object sender, object args)
    {
        Stats stats = sender as Stats;
        Debug.Log(stats.name + " leveled up");
    }

}
