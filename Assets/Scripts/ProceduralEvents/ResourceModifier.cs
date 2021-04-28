using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ModifierValue
{
    public float valueToChange;
    public bool percent;

    public ModifierValue(float value, bool p)
    {
        valueToChange = value;
        percent = p;
    }
}

[System.Serializable]
public class ResourceModifier
{
    public enum TargetResourceAttribute
    {
        timeToProduce,
        goldValue,
        supplyCapacity,
        supplyPerProduction,
        addToResourceSupply,
        addToResourceProgress
    }
    
    public string targetResource;
    public TargetResourceAttribute targetResourceAttribute;

    public ModifierValue modifierValue;
}