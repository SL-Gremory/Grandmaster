using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnCountText : MonoBehaviour
{
	[SerializeField]
	private Text displayText; //this script is on Main Camera. Place UI text object here
							//Pagi: why should this be on Main Camera instead of the text itself?

	// Start is called before the first frame update
	void Start()
	{
		DisplayNewTurn();
	}

	internal void DisplayNewTurn()
	{
		displayText.text = "Turn " + BattleState.Instance.turnCounter;
	}
}