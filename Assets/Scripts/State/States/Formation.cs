using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formation : MonoBehaviour
{
    bool done;

    GameObject battleControl;
    GameObject level;
    GameObject prefab;

    private void Awake()
    {
        battleControl = GameObject.Find("Battle Control");
        battleControl.SetActive(false);
        level = GameObject.Find("Level");
        prefab = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Press [B] to place a Brandy. Press [N] to place an Enemy. Press [J] to finish placements");
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.B))
        {
            PlaceUnit("Brandy");
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            PlaceUnit("EnemyVanguard");
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("Starting battle state controller");
            battleControl.SetActive(true);
        }

        if (Input.GetMouseButtonDown(0)) {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000f);

            var place = new Int2((int)hit.point.x, (int)hit.point.z);
            Debug.Log("Placing at [" + place.x + ", " + place.y + "]");
            LevelGrid.Instance.AddPrefab(place.x, place.y, prefab);
            prefab = null;
            Debug.Log("Placed unit");
        }
    }

    private void PlaceUnit(string v)
    {
        Debug.Log("Placing " + v + ". Select where you want to place");
        prefab = Resources.Load(Application.dataPath + "/Prefabs/" + v + ".prefab", typeof(GameObject)) as GameObject;
    }


}
