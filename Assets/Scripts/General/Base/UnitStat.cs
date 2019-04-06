using System;
using System.Collections.Generic;

public class UnitStat
{
    // Unit's base stat
    public float BaseValue;

    // Cannot alter this unless we are in the constructor of a class
    private readonly List<StatModifier> statModifiers;


    // Checks to see if last value is garbage
    private bool isDirty = true;
    private float _value;


    public float Value {
        get {
            if (isDirty)
            {
                _value = CalculateFinalValue();
                isDirty = false;
            }
            return _value;
        }
    }



    public UnitStat(float baseValue)
    {
        BaseValue = baseValue;
        statModifiers = new List<StatModifier>();
    }


    // Adding modifiers
    public void AddModifier(StatModifier modifier)
    {
        isDirty = true;
        statModifiers.Add(modifier);
        statModifiers.Sort(CompareModifierOrder);
    }
    
    // Some modifiers have higher precedence over other modifiers
    // This will check the modifiers
    private int CompareModifierOrder(StatModifier a, StatModifier b)
    {
        if (a.Order < b.Order)
            return -1;
        else if (a.Order > b.Order)
            return 1;
        else
            return 0;
    }


    // Removing modifiers
    public bool RemoveModifier(StatModifier modifier)
    {
        isDirty = true;
        return statModifiers.Remove(modifier);
    }

    public bool RemoveAllModifiersFromSource(object source)
    {
        bool didRemove = false;

        for(int i = statModifiers.Count - 1; i >= 0; i--)
        {
            if (statModifiers[i].Source == source)
            {
                isDirty = true;
                didRemove = true;
                statModifiers.RemoveAt(i);
            }
        }

        return didRemove;
    }

    // Calculate the final multiplier for affecting unit's stats
    private float CalculateFinalValue()
    {
        float finalValue = BaseValue;
        float sumPercentAdd = 0;

        for(int i = 0; i < statModifiers.Count; i++)
        {
            StatModifier modifier = statModifiers[i];

            if (modifier.Type == StatModType.Flat)
            {
                finalValue += statModifiers[i].Value;
            }
            else if (modifier.Type == StatModType.PercentAdd)
            {
                sumPercentAdd += modifier.Value;

                if(i + 1 >= statModifiers.Count || statModifiers[i+1].Type != StatModType.PercentAdd)
                {
                    finalValue *= 1 + sumPercentAdd;
                    sumPercentAdd = 0;
                }
            }
            else if (modifier.Type == StatModType.PercentMult)
            {
                finalValue *= 1 + modifier.Value;
            }
        }
        return (float)Math.Round(finalValue);
    }
}
