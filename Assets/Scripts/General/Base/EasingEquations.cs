using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Experience gain will be based on an Ease In Quad curve
// i.e. Low levels can be attained in less experience compared
// to hard levels

// Credits to C.J. Kimberlin (http://cjkimberlin.com) this function

public class EasingEquations : MonoBehaviour
{
    public static float EaseInQuad(float start, float end, float value)
    {
        end -= start;
        return end * value * value + start;
    }
}
