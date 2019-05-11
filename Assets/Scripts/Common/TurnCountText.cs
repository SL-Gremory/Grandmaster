using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnCountText : MonoBehaviour
{
	//Initialize variables
	private TurnManager turnManager;
	[SerializeField]
	private Text displayText; //this script is on Main Camera. Place UI text object here
	
    // Start is called before the first frame update
    void Start()
    {
		turnManager = GameObject.FindObjectOfType<TurnManager>();
		DisplayNewTurn();
    }
	
	internal void DisplayNewTurn()
	{
		displayText.text = "Turn " + turnManager.turnCounter;
	}
}