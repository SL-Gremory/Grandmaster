using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : MonoBehaviour
{
    //store all player and enemy characters, turns and whatever here
    // Ronald: "Implementing unit attacking here as well"

    [SerializeField]
    BattleEnd end;

    [HideInInspector]
    public BattleSceneSO BattleData;

  //  [SerializeField]
  //  private static List<GrandmasterUnit> fieldedUnits; // Contains all references to units on the field




    public float RealTimeElapsed { get; private set; }

    private void Start()
    {

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
            ReturnToWorldMap();
        }
        else if (BattleJudges.JudgeEnd(BattleData.LoseCondition, this))
        {
            //TODO: BATTLE IS LOST, RETURN TO MAP
            ReturnToWorldMap();
        }
    }

    /*
    public static void AddUnitToList(GrandmasterUnit unit)
    {
      fieldedUnits.Add(unit);
    }
*/

    // CHECK TO SEE IF THIS MAKES SENSE

    void AttackUnitAt(int posX, int posZ, GrandmasterUnit attacker)
    {
        GrandmasterUnit defender = new GrandmasterUnit();
        List<GameObject> prefabs = LevelGrid.Instance.GetPrefabsAt(posX, posZ);
    
        foreach (GameObject obj in prefabs)
        {
            if((defender = obj.GetComponent<GrandmasterUnit>()) != null)
            {
                Debug.Log(string.Format("{0} is attacking {1} for {2} damage",
                    attacker.GetUnitName(),
                    defender.GetUnitName(),
                    Attack.CalculateProjectedDamage(attacker, defender)));

                Attack.CommenceBattle(attacker, defender);
            }
        }

        // HP checking goes here?

    }



    void ReturnToWorldMap()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("world", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
