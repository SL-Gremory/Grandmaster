using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    Transform icon;
    Text statsText;
    Text affinitiesText;


    // Start is called before the first frame update
    void Start()
    {
        icon = transform.Find("HUD Icon");
        statsText = transform.Find("HUD Stats").GetComponentInChildren<Text>();
        affinitiesText = transform.Find("HUD Affinities").GetComponentInChildren<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayInfo(GameObject unit)
    {

        statsText.text = "Hello world!";
        
    }
}
