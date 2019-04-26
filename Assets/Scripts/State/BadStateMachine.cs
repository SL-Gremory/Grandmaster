using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadStateMachine : MonoBehaviour
{
    GrandmasterUnit[] unitsOnTheField;
    GrandmasterUnit[] herosOnTheField;
    GrandmasterUnit[] enemiesOnTheField;
    EngageBattleState engageBattleState;
    private BattleStates _currentState;


    void Start()
    {
        _currentState = BattleStates.BATTLE_START;
    }

    void Update()
    {
        switch (_currentState) {
            case (BattleStates.BATTLE_START):
                InitializeUnitsOnField();
                break;
            case (BattleStates.PLAYER_TURN):
                PlayerTurn();
                break;
            case (BattleStates.PLAYER_ACTION):
                PlayerAttack();
                break;
            case (BattleStates.TRANSITION):
                TransitionTurn();
                break;
            case (BattleStates.ENEMY_TURN):
                EnemyTurn();
                break;
            case (BattleStates.ENEMY_ACTION):
                EnemyAction();
                break;
            case (BattleStates.LOSE):
                break;
            case (BattleStates.WIN):
                break;

        }
    }

    private void InitializeUnitsOnField()
    {
        unitsOnTheField = GameObject.FindObjectsOfType<GrandmasterUnit>();

        for(int i = 0; i < unitsOnTheField.Length; ++i)
        {
            //unitsOnThe
        }







        // Player's units (heros) will always move first
        for (int i = 0; i < unitsOnTheField.Length; ++i)
        {
            switch (unitsOnTheField[i].GetUnitAffiliation())
            {
                case (Team.HERO):
                    unitsOnTheField[i].SetTurnStatus(true);
                    break;
                case (Team.NEUTRAL):
                    unitsOnTheField[i].SetTurnStatus(false);
                    break;
                case (Team.ENEMY):
                    unitsOnTheField[i].SetTurnStatus(false);
                    break;
                case (Team.OBSTACLE):
                    unitsOnTheField[i].SetTurnStatus(false);
                    break;
            }
            
        }

        _currentState = BattleStates.WIN;
    }

    private void PlayerTurn()
    {

    }

    private void PlayerAttack()
    {
        _currentState = BattleStates.TRANSITION;
    }

    private void EnemyTurn()
    {
        _currentState = BattleStates.ENEMY_ACTION;
    }

    private void EnemyAction()
    {
        _currentState = BattleStates.WIN;
    }

    private void TransitionTurn()
    {
        throw new NotImplementedException();
    }


}
