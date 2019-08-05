using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : MonoBehaviour
{
    //store all player and enemy characters, turns and whatever here

    [SerializeField]
    BattleEnd end;

//    [HideInInspector]
    [SerializeField]
    public BattleSceneSO BattleData;

    public float RealTimeElapsed { get; private set; }

    // Reference to grid data
    public static GameObject[,] unitsGrid;

    public static int playerUnitCount = 0;
    public static int enemyUnitCount = 0;
    public static int neutralUnitCount = 0;

    private void Start()
    {
        Debug.Log("Battle State starting");
    }

    public int GetPlayerUnitCount()
    {
        return playerUnitCount;
    }

    public int GetEnemyUnitCount()
    {
        return enemyUnitCount;
    }


    private void Update()
    {
        RealTimeElapsed += Time.deltaTime;
        if (BattleData == null) {
            Debug.LogWarning("No battle data here, must be a test.", this);
            return;
        }
        if (BattleJudges.JudgeEnd(BattleData.WinCondition, this))
        {
            //TODO: BATTLE IS WON, RETURN TO MAP
            //Debug.Log("Won because of time.");
            //Debug.Log("The battle has been won!");
            Debug.Log("ALL THINGS DED");
            ReturnToWorldMap();
        }
        else if (BattleJudges.JudgeEnd(BattleData.LoseCondition, this))
        {
            //TODO: BATTLE IS LOST, RETURN TO MAP
            ReturnToWorldMap();
        }


        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("Enemy count: " + BattleState.enemyUnitCount);

        }
    }


    void ReturnToWorldMap()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("world", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
