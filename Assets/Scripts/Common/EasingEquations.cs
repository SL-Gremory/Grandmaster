using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Experience gain will be based on an Ease In Quad curve
// i.e. Low levels can be attained in less experience compared
// to hard levels

// Credits to C.J. Kimberlin (http://cjkimberlin.com) this function
// https://gist.github.com/cjddmut/d789b9eb78216998e95c

public class EasingEquations : MonoBehaviour
{
    public static float EaseInQuad(float start, float end, float levelPercentage)
    {
        end -= start;
        return end * levelPercentage * levelPercentage + start;
    }

    // Add more equations here if we need
}
