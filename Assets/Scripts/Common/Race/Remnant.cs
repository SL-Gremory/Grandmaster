using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remnant : Race
{

    // Name of race
    public string RaceName = "Remnant";

    // Description of race's effect on units of the same race
    public string RaceEffect = "Remnant's earn 20% more experience after battle if unit survives";


    public override void RaceEffectOnStats()
    {
        throw new System.NotImplementedException();
    }
}
