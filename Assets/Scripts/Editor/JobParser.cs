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

    // GrowthStats parser
    private static void ParseGrowthStats()
    {
        string readPath = string.Format(FilepathConstants.GROWTH_STATS, Application.dataPath);
        string[] readInText = File.ReadAllLines(readPath);
        for (int i = 1; i < readInText.Length; ++i)
            ParseGrowthStatsAuxiliary(readInText[i]);
    }

    private static void ParseGrowthStatsAuxiliary(string v)
    {
        throw new NotImplementedException();











    }
 

    // JobStartingStats parser
    static void ParseStartingStats()
    {
        string readPath = string.Format("{0}/Settings/JobStartingStats.csv", Application.dataPath);
        string[] readInText = File.ReadAllLines(readPath);
        for (int i = 1; i < readInText.Length; ++i)
            ParseStartingStatsAuxiliary(readInText[i]);
    }

    static void ParseStartingStatsAuxiliary(string line)
    {
        string[] elements = line.Split(',');
 










    }

}
