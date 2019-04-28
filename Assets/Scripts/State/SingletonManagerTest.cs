using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingletonManagerTest : MonoBehaviour
{
    public static SingletonManagerTest Instance { get; private set; }

    public static List<GrandmasterUnit> herosOnTheField;
    public static List<GrandmasterUnit> enemiesOnTheField;
    GrandmasterUnit firstHero;
    GrandmasterUnit firstEnemy;


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
        herosOnTheField = new List<GrandmasterUnit>();
        enemiesOnTheField = new List<GrandmasterUnit>();

    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            firstHero = herosOnTheField[0];
            firstEnemy = enemiesOnTheField[0];

           // Debug.Log("Printing unit statuses");

            foreach (GrandmasterUnit hero in herosOnTheField)
            {
                Debug.Log(string.Format("Hero found: {0}", hero.GetUnitName()));
                hero.PrintStats();
            }

            foreach (GrandmasterUnit enemy in enemiesOnTheField)
            {
                Debug.Log(string.Format("Enemy found: {0}", enemy.GetUnitName()));
                enemy.PrintStats();
            }
        }

        if (Input.GetKeyDown(KeyCode.D) && herosOnTheField.Count > 0 && enemiesOnTheField.Count > 0)
        {
            firstHero = herosOnTheField[0];
            firstEnemy = enemiesOnTheField[0];

            
            Debug.Log(string.Format("Hero[0] {0} is attacking Enemy[0] {1} for {2} damage", 
                firstHero.GetUnitName(),
                firstEnemy.GetUnitName(),
                Attack.CalculateProjectedDamage(firstHero, firstEnemy)));
 
            Attack.CommenceBattle(firstHero, firstEnemy);

            //Debug.Log(string.Format("{0} has {1} left", firstHero.GetUnitName(), firstHero.CHP));
            //Debug.Log(string.Format("{0} has {1} left", firstEnemy.GetUnitName(), firstEnemy.CHP));

            
            if (!(firstHero.CHP > 0))
            {
                Debug.Log(string.Format("{0} has died", firstHero.GetUnitName()));
                Destroy(firstHero);
            }
            if (!(firstEnemy.CHP > 0))
            {
                Debug.Log(string.Format("{0} has died", firstEnemy.GetUnitName()));
                Destroy(firstEnemy);
            }
            
        }

        if (Input.GetKeyDown(KeyCode.F) && herosOnTheField.Count > 0 && enemiesOnTheField.Count > 0)
        {
            firstHero = herosOnTheField[0];
            firstEnemy = enemiesOnTheField[0];

            
            Debug.Log(string.Format("Enemy[0] {0} is attacking Hero[0] {1} for {2} damage",
                firstEnemy.GetUnitName(),
                firstHero.GetUnitName(),
                Attack.CalculateProjectedDamage(firstEnemy, firstHero)));
                

            //Attack.CommenceBattle(firstEnemy, firstHero);

            //Debug.Log(string.Format("{0} has {1} HP left", firstHero.GetUnitName(), firstHero.CHP));
            //Debug.Log(string.Format("{0} has {1} HP left", firstEnemy.GetUnitName(), firstEnemy.CHP));

            if (!(firstHero.CHP > 0))
            {
                //Debug.Log(string.Format("{0} has died", firstHero.GetUnitName()));
                Destroy(firstHero);
            }
            if (!(firstEnemy.CHP > 0))
            {
                //Debug.Log(string.Format("{0} has died", firstEnemy.GetUnitName()));
                Destroy(firstEnemy);
            }

        }


    }

}
