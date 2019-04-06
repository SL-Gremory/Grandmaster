using System.Reflection; //for GetType and GetField and GetValue
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
	//									HP		MP		ATK		DEF		SPR		SPD
	public int[] DefaultStatList	= {100,		100,	10,		10,		10,		10};
	public int[] EfriyeetStatList	= {100,		80,		15,		12,		14,		16};
	public int[] JavelinStatList	= {90,		80,		17,		6,		18,		21};
	public int[] PrinzStatList		= {150,		80,		10,		20,		8,		6};
	
	public int[] ReportStats(string characterName)
	{
		string search = characterName + "StatList";
		int[] characterStats = (int[])this.GetType().GetField(search).GetValue(this); //GetField only works on public fields. "this" refers to object of interest
		return characterStats;
	}
}
