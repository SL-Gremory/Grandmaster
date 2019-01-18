using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This component will be used on ALL playable units and NPCs
// This includes your units, enemies, bosses, allys, etc.

public enum StatTypes
{
    LVL,    // Level
    EXP,    // Accumulated experience
    HP,     // Hit points
    MP,     // "Magic" points
    ATK,    // Physical/magical attack power
    DEF,    // Physical defense
    SPR,    // Magical defense
    SPD,    // Speed
    MOV,    // Move count
    Count
}
