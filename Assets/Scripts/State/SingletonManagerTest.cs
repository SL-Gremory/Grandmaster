using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingletonManagerTest : MonoBehaviour
{
    public static SingletonManagerTest Instance { get; private set; }

    public static List<GrandmasterUnit> herosOnTheField;
    public static List<GrandmasterUnit> enemiesOnTheField;

    int count = 0;

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
        if (herosOnTheField.Count != count)
        {
            Debug.Log("Printing stuff now");
            //Debug.Log(enemiesOnTheField.Count);
            count = herosOnTheField.Count;

            foreach (GrandmasterUnit unit in herosOnTheField)
            {
                Debug.Log(string.Format("Unit found: {0}", unit.GetUnitName()));
            }

        }


    }

}
