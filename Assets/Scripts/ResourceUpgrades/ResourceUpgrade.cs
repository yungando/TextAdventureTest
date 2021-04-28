using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceUpgradeCategory
{
    Coal,
    Fish,
    Wheat,
    Wood,
    Special
}

[CreateAssetMenu(fileName = "Upgrade", menuName = "Resources/Resource Upgrade", order = 2)]
public class ResourceUpgrade : ScriptableObject
{
    [HideInInspector] public string upgradeName;
    [SerializeField] private ResourceUpgradeCategory upgradeCategory;
    [TextArea(1, 1000)] public string upgradeDescription;

    public bool unlocked = false;

    [SerializeField] public int goldToUnlock;

    public List<ResourceModifier> resourceModifiers = new List<ResourceModifier>();
    public List<Requirement> upgradeRequirements = new List<Requirement>();

    public ResourceUpgradeCategory UpgradeCategory => upgradeCategory;

    void OnValidate()
    {
        upgradeName = name;
        unlocked = false;
    }

    public void TryUnlockUpgrade()
    {
        if (GameController.Instance.gold < goldToUnlock)
        {
            GameController.Instance.LogStringWithReturn($"Could not unlock {upgradeName}. Not enough Gold.");

            return;
        }

        if (UpgradeRequirementsMet() == false)
        {
            GameController.Instance.LogStringWithReturn($"Could not unlock {upgradeName}. Unlock requirements are not met.");

            return;
        }

        foreach (var e in resourceModifiers)
        {
            var resource = ResourceProductionManager.Instance.TryGetResource(e.targetResource);

            if (resource == null)
            {
                Debug.LogError($"No Resource exists with name: {e.targetResource}");
                continue;
            }
            
            Debug.Log($"Adding {resource.name} effect {e.targetResourceAttribute}");

            switch (e.targetResourceAttribute)
            {
                case(ResourceModifier.TargetResourceAttribute.timeToProduce):
                    resource.AddTimeToProduceModiferValue(e.modifierValue);
                    break;
                case(ResourceModifier.TargetResourceAttribute.goldValue):
                    resource.AddGoldModifierValue(e.modifierValue);
                    break;
                case(ResourceModifier.TargetResourceAttribute.supplyCapacity):
                    resource.AddSupplyCapacityModifierValue(e.modifierValue);
                    break;
                case(ResourceModifier.TargetResourceAttribute.supplyPerProduction):
                    resource.AddProductionPerCycleModifierValue(e.modifierValue);
                    break;
                case(ResourceModifier.TargetResourceAttribute.addToResourceSupply):
                    resource.AddToResourceSupply(e.modifierValue);
                    break;
                case(ResourceModifier.TargetResourceAttribute.addToResourceProgress):
                    resource.AddToResourceProgress(e.modifierValue);
                    break;
            }
        }

        GameController.Instance.gold -= goldToUnlock;
        unlocked = true;

        GameController.Instance.LogStringWithReturn($"Unlocked {upgradeName} for {goldToUnlock} Gold.");

        return;
    }

    public bool UpgradeRequirementsMet()
    {
        foreach (var req in upgradeRequirements)
        {
            if(req.IsMet() == false)
                return false;
        }

        return true;
    } 
}