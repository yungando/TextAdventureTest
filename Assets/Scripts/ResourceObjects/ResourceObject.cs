using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Resource", menuName = "Resources/Resource Object", order = 1)]
public class ResourceObject : ScriptableObject
{
    [HideInInspector] public string resourceName;
    public string resourceDescription;

    [SerializeField] public bool unlocked;
    [SerializeField] public int goldToUnlock;

    [SerializeField] private ResourceObjectAttribute timeToProduce;

    [SerializeField] private float currentProductionProgress;
    [SerializeField] private int currentSupply;

    [SerializeField] private ResourceObjectAttribute goldValue;
    [SerializeField] private ResourceObjectAttribute supplyCapacity;
    [SerializeField] private ResourceObjectAttribute supplyPerProduction;

    void OnValidate()
    {
        resourceName = name;
    }

    public void FreshStartProtocol()
    {
        unlocked = false;
        
        currentProductionProgress = 0;;
        currentSupply = 0;

        timeToProduce.Init();
        goldValue.Init();
        supplyCapacity.Init();
        supplyPerProduction.Init();
    }

    public void IncreaseProductionProcess(float timeToAdvance)
    {
        currentProductionProgress += timeToAdvance;

        if (currentSupply >= supplyCapacity.CurrentValue)
        {
            currentProductionProgress = 0;
            return;
        }

        if (currentProductionProgress >= timeToProduce.CurrentValue)
        {
            currentSupply += (int)Mathf.Floor(supplyPerProduction.CurrentValue);
            currentProductionProgress = 0;
        }

        //EditorUtility.SetDirty(this);
    }

    public float Sell(int desiredSellAmount)
    {
        if(desiredSellAmount > currentSupply)
            return -1;

        currentSupply -= desiredSellAmount;
        
        //EditorUtility.SetDirty(this);

        return goldValue.CurrentValue * desiredSellAmount;
    }

    public void TryUnlockResource()
    {
        if (unlocked == true)
        {
            GameController.Instance.LogStringWithReturn($"{resourceName} resource already Unlocked.");
        
            return;
        }

        if (GameController.Instance.gold < goldToUnlock)
        {
            GameController.Instance.LogStringWithReturn($"Could not unlock {resourceName}. Not enough Gold.");

            return;
        }
        
        GameController.Instance.LogStringWithReturn($"Unlocked {resourceName} for {goldToUnlock} Gold.");


        GameController.Instance.gold -= goldToUnlock;
        unlocked = true;
        
        //EditorUtility.SetDirty(this);
    }

    public int GetCurrentSupply()
    {
        return currentSupply;
    }

    public float GetGoldValue()
    {
        return goldValue.CurrentValue;
    }
    
    public float GetSupplyCapacity()
    {
        return supplyCapacity.CurrentValue;
    }

    #region resource_object_mods

    public void AddTimeToProduceModiferValue(ModifierValue _mod)
    {
        timeToProduce.AddModifier(_mod);
    }

    public void RemoveTimeToProduceModiferValue(ModifierValue _mod)
    {
        timeToProduce.RemoveModifier(_mod);
    }

    public void AddGoldModifierValue(ModifierValue _mod)
    {
        goldValue.AddModifier(_mod);
    }

    public void RemoveGoldModifierValue(ModifierValue _mod)
    {
        goldValue.RemoveModifier(_mod);
    }

    public void AddSupplyCapacityModifierValue(ModifierValue _mod)
    {
        supplyCapacity.AddModifier(_mod);
    }

    public void RemoveSupplyCapacityModifierValue(ModifierValue _mod)
    {
        supplyCapacity.RemoveModifier(_mod);
    }

    public void AddProductionPerCycleModifierValue(ModifierValue _mod)
    {
        supplyPerProduction.AddModifier(_mod);
    }

    public void RemoveProductionPerCycleModifierValue(ModifierValue _mod)
    {
        supplyPerProduction.RemoveModifier(_mod);
    }

    public void AddToResourceSupply(ModifierValue _mod)
    {
        if(_mod.percent == true)
        {
            currentSupply *= (int)(_mod.valueToChange + 1);
        }
        else
        {
            currentSupply += (int)_mod.valueToChange;
        }

        if (currentSupply < 0)
            currentSupply = 0;
    }

    public void AddToResourceProgress(ModifierValue _mod)
    {
        if(_mod.percent == true)
        {
            currentProductionProgress *= (int)(_mod.valueToChange + 1);
        }
        else
        {
            currentProductionProgress += (int)_mod.valueToChange;
        }

        if (currentProductionProgress < 0)
            currentProductionProgress = 0;
    }

    #endregion
}