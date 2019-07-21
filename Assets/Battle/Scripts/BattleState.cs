using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : MonoBehaviour
{
    //store all player and enemy characters, turns and whatever here

    [SerializeField]
    BattleEnd end;

    [HideInInspector]
    public BattleSceneSO BattleData;

    public float RealTimeElapsed { get; private set; }

    private void Start()
    {
        Debug.Log("Battle State starting");
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
            Debug.Log("Won because of time.");
            ReturnToWorldMap();
        }
        else if (BattleJudges.JudgeEnd(BattleData.LoseCondition, this))
        {
            //TODO: BATTLE IS LOST, RETURN TO MAP
            ReturnToWorldMap();
        }
    }

    void ReturnToWorldMap()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("world", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
