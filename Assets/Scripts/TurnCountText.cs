using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnCountText : MonoBehaviour
{
	//Initialize variables
	private EventManager eventManager;
	public Text displayText;
	
    // Start is called before the first frame update
    void Start()
    {
		eventManager = GameObject.FindObjectOfType<EventManager>();
		DisplayNewTurn();
    }
	
	// Update is called once per frame
    /*void Update()
    {
        
    }*/
	
	public void DisplayNewTurn()
	{
		displayText.text = "Turn " + eventManager.turnCounter;
	}
}
