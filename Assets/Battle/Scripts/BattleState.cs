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

    // Global reference for current selected unit
    public static GameObject currentSelect;

    public static GameObject radialMenu;


    [SerializeField]
    private DoneButton doneButton;
    [SerializeField]
    private TurnCountText turnCountText;
    [SerializeField]
    private bool playerFirst; //must define as true or false in editor, on prefab, or when spawning object


    internal bool turnIsDone = false;
    internal bool isPlayerTurn;
    internal static int playerUnitCount = 0;
    internal static int enemyUnitCount = 0;
    internal static int neutralUnitCount = 0;
    internal int unitsDone = 0;
    internal int turnCounter = 1;

    public static BattleState Instance { get; private set; }

    void Awake()
    {
        if (Instance != null)
            Debug.LogError("There can't be multiple BattleManagers in the scene.");
        Instance = this;
        //var mapSize = LevelGrid.Instance.GetMapSize();
        //unitsGrid = new Unit[mapSize.x, mapSize.y];

        if (playerFirst)
        {
            isPlayerTurn = true;
        }
        else
        {
            isPlayerTurn = false;
        }
    }


    private void Start()
    {
        Debug.Log("Battle State starting");
        currentSelect = null;
        //radialMenu = GameObject.Find("Custom Radial Menu");
        //radialMenu.SetActive(false);
    }

    public void AddPlayerUnitCount(int amt)
    {
        playerUnitCount += amt;
    }

    public void AddEnemyUnitCount(int amt)
    {
        enemyUnitCount += amt;
    }

    public void AddUnitsDone(int amt)
    {
        unitsDone += amt;
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

        CheckWinConditions();


        if (turnIsDone)
        {
            //Debug.Log("Changing turns");
            ChangeTurns();
            turnIsDone = false;
        }

        if (isPlayerTurn && playerUnitCount == unitsDone)
        {
            doneButton.BrightenButton();
            unitsDone++; //stupid, hacky, but efficient solution that prevents brightening the button in every frame even after it has already been brightened
        }
        else if (!isPlayerTurn && enemyUnitCount == unitsDone)
        {
            turnIsDone = true;
        }




    }

    /*
    public static void SelectUnit(GameObject u)
    {
        currentSelect = u;
        Debug.Log(u.GetComponent<UnitData>().UnitName);
    }
    */

    void ChangeTurns()
    {
        isPlayerTurn = !isPlayerTurn; //current turn is done, at this point forward it is the other side's turn
        unitsDone = 0;
        doneButton.ResetButton(isPlayerTurn);
        if (isPlayerTurn)
        {
            turnCounter++; //only increments when it is becoming the player's turn
            turnCountText.DisplayNewTurn();
        }

        //goes through every object of type Unit and readies/exhausts allies or enemies appropriately
        //Unit[] units = FindObjectsOfType(typeof(Unit)) as Unit[];
        UnitData[] units = FindObjectsOfType(typeof(UnitData)) as UnitData[];


        foreach (UnitData unit in units)
        {
            UnitStateController usc = unit.gameObject.GetComponent<UnitStateController>();
            //Debug.Log("finded a unit");
            //var unitInfo = unit.GetComponentInParent<Unit>();
            if (isPlayerTurn)
            {
                if (unit.UnitTeam == Team.HERO)
                {
                    usc.ReadyUnit();
                    //Debug.Log("ally is woke");
                }
                else
                {
                    usc.ExhaustUnit();
                    //Debug.Log("enemy is exhausted");
                }
            }
            else
            {
                if (unit.UnitTeam == Team.ENEMY)
                {
                    usc.ReadyUnit();
                    //Debug.Log("enemy is woke");
                }
                else
                {
                    usc.ExhaustUnit();
                    //Debug.Log("ally is exhausted");
                }
            }
        }
    }

    private void CheckWinConditions()
    {
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
    }

    void ReturnToWorldMap()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("world", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
