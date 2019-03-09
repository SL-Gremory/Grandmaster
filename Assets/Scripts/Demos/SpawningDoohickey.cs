using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
void SpawningDoohickey()
{
    string[] jobs = new string[] { "Bulwark" };

    for(int i = 0; i < jobs.Length; ++i)
    {
        GameObject instance = Instantiate(owner.heroPrefab) as GameObject;

        Stats s = instance.AddComponent<Stats>();
        s[StatTypes.LVL] = 1;

        GameObject jobPrefab = Resources.Load<GameObject>("Jobs/" + jobs[i]);
        GameObject jobInstance = Instantiate(jobPrefab) as GameObject;
        jobInstance.transform.SetParent(instance.transform);

        Job job = jobInstance.GetComponent<Job>();
        job.Employ();
        job.LoadDefaultStats();

        Point p = new Point((int)levelData.tiles[i].x,
            (int)levelData.tiles[i].z);

        Unit unit = instance.GetComponent<Unit>();
        unit.Place(board.GetTile(p));
        unit.Match();

        instance.AddComponent<WalkMovement>();
        UnitScript.Add(unit);
    }
    
}
*/