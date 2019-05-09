using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingletonManagerTest : MonoBehaviour
{
    /*
    public static SingletonManagerTest Instance { get; private set; }

    private static List<GrandmasterUnit> herosOnTheField;
    private static List<GrandmasterUnit> enemiesOnTheField;

    List<GrandmasterUnit> unitsOnTheField;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
        unitsOnTheField = new List<GrandmasterUnit>();

    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            foreach (GrandmasterUnit unit in unitsOnTheField)
            {
                Debug.Log(string.Format("Unit found: {0}", unit.GetUnitName()));
                unit.PrintStats();
            }
        }
        
        if(Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log(string.Format("{0} is attacking {1} for {2} damage",
                unitsOnTheField[0].GetUnitName(),
                unitsOnTheField[1].GetUnitName(),
                Attack.CalculateProjectedDamage(unitsOnTheField[0], unitsOnTheField[1])));

            Attack.CommenceBattle(unitsOnTheField[0], unitsOnTheField[1]);

            Debug.Log(string.Format("{0} has {1} left", unitsOnTheField[0].GetUnitName(), unitsOnTheField[0].CHP));
            Debug.Log(string.Format("{0} has {1} left", unitsOnTheField[1].GetUnitName(), unitsOnTheField[1].CHP));
        }


        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log(string.Format("{0} is attacking {1} for {2} damage",
                unitsOnTheField[1].GetUnitName(),
                unitsOnTheField[0].GetUnitName(),
                Attack.CalculateProjectedDamage(unitsOnTheField[1], unitsOnTheField[0])));

            Attack.CommenceBattle(unitsOnTheField[1], unitsOnTheField[0]);

            Debug.Log(string.Format("{0} has {1} left", unitsOnTheField[1].GetUnitName(), unitsOnTheField[1].CHP));
            Debug.Log(string.Format("{0} has {1} left", unitsOnTheField[0].GetUnitName(), unitsOnTheField[0].CHP));
        }

        if(unitsOnTheField[0].CHP == 0)
        {
            Destroy(unitsOnTheField[0].GetComponentInParent<GameObject>());
            Debug.Log(string.Format("{0} has died", unitsOnTheField[0].GetUnitName()));
        }

        if (unitsOnTheField[1].CHP == 0)
        {
            Destroy(unitsOnTheField[1].GetComponentInParent<GameObject>());
            Debug.Log(string.Format("{0} has died", unitsOnTheField[1].GetUnitName()));
        }

    }

    public void AddUnit(GrandmasterUnit unit)
    {
        unitsOnTheField.Add(unit);
    }
    */
}
