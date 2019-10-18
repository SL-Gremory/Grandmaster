using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsContainer : MonoBehaviour
{
    //CharacterSkill charSkill;                   // 1 
    //RaceSkill raceSkill;                        // 1
    List<Skill> skills;                          // 0-3 


    private void Start()
    {
        skills = gameObject.GetComponent<UnitData>().UnitSkills;

        if (skills == null)
            skills = new List<Skill>();
    }

    void AddSkill(Skill skill)
    {
        if (skills.Count >= 3)
            return;

        skills.Add(skill);

        List<ModApplication> toApply = skills[skills.Count - 1].Activate();

    }

    void RemoveSkill(Skill skill)
    {
        if (skills.Contains(skill))
        {
            int removeIndex = skills.IndexOf(skill);
            skills[removeIndex].Deactivate();
            skills.Remove(skill);
        }
    }
}
