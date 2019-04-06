using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using System.IO;

// Starting stats
/*
 *  JobName, HP, MP, ATK, DEF, SPR, SPD
 *  Archer, 12, 7, 9, 4, 5, 506
 * /


// Growth rate of stats
/*
 *  JobName, HP, MP, ATK, DEF, SPR, SPD
 *  Archer, .3, .124867, 2.3, 0.8, 1.1, 504
 */


static class FilepathConstants
{
    public const string GROWTH_STATS = "{0}/Settings/GROWTHS.csv";
    public const string BASE_STATS = "{0}/Settings/BASE.csv";
}

public static class JobParser
{
    [MenuItem("Pre Production/Parse Jobs")]
    public static void Parse()
    {
        CreateDirectories();
        ParseStartingStats();
        ParseGrowthStats();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    // Directory maker
    static void CreateDirectories()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Jobs"))
            AssetDatabase.CreateFolder("Assets/Resources", "Jobs");
    }

    static void ParseStartingStats()
    {
        string readPath = string.Format(FilepathConstants.BASE_STATS, Application.dataPath);
        string[] readInText = File.ReadAllLines(readPath);
        for (int i = 1; i < readInText.Length; ++i)
            ParseStartingStatsAuxiliary(readInText[i]);
    }

    static void ParseStartingStatsAuxiliary(string v)
    {
        string[] elements = v.Split(',');
        GameObject obj = GetOrCreate(elements[0]);
        Job job = obj.GetComponent<Job>();
        for (int i = 1; i < Job.statOrder.Length + 1; ++i)
        {
            job.baseStats[i - 1] = Convert.ToInt32(elements[i]);
        }

        StatModifierFeature move = GetFeature(obj, StatTypes.MOV);
        move.amount = Convert.ToInt32(elements[8]);

        StatModifierFeature jump = GetFeature(obj, StatTypes.JMP);
        jump.amount = Convert.ToInt32(elements[9]);

    }


    // GrowthStats parser
    static void ParseGrowthStats()
    {
        string readPath = string.Format(FilepathConstants.GROWTH_STATS, Application.dataPath);
        string[] readInText = File.ReadAllLines(readPath);
        for (int i = 1; i < readInText.Length; ++i)
            ParseGrowthStatsAuxiliary(readInText[i]);
    }

    // GrowthStats parser overloaded
    static void ParseGrowthStatsAuxiliary(string v)
    {
        string[] elements = v.Split(',');
        GameObject obj = GetOrCreate(elements[0]);
        Job job = obj.GetComponent<Job>();
        for (int i = 1; i < elements.Length; ++i)
        {
            job.growStats[i - 1] = Convert.ToSingle(elements[i]);
        }
    }

    static StatModifierFeature GetFeature(GameObject obj, StatTypes type)
    {
        StatModifierFeature[] statModFeat = obj.GetComponents<StatModifierFeature>();
        for(int i = 0; i < statModFeat.Length; ++i)
        {
            if (statModFeat[i].type == type)
                return statModFeat[i];
        }
        StatModifierFeature feature = obj.AddComponent<StatModifierFeature>();
        feature.type = type;
        return feature;
    }

    static GameObject GetOrCreate(string jobTitle) {

        string path = string.Format("Assets/Resources/Jobs/{0}.prefab", jobTitle);
        GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);

        if (obj == null)
            obj = Create(path);

        return obj;
    }

    static GameObject Create(string path)
    {
        GameObject instance = new GameObject("temp");
        instance.AddComponent<Job>();

#pragma warning disable CS0618 // Type or member is obsolete

        GameObject prefab = PrefabUtility.CreatePrefab(path, instance);

#pragma warning restore CS0618 // Type or member is obsolete

        GameObject.DestroyImmediate(instance);
        return prefab;
    }
}