using UnityEngine;

[CreateAssetMenu(fileName = "Requirement", menuName = "Resources/Requirement", order = 4)]
public class Requirement : ScriptableObject
{
    public enum RequirementType 
    {
        resourceUnlocked, 
        resourceSupplyThreshold, 
        resouceUpgradeApplied,
        playerGoldThreshold
    };

    public RequirementType requirementType;

    public string resourceName;
    public int resourceSupplyThreshold;
    public string upgradeName;
    public int playerGoldThreshold;

    public bool IsMet()
    {
        switch (requirementType)
        {
            case RequirementType.resourceUnlocked:
                {
                    var resource = ResourceProductionManager.Instance.TryGetResource(resourceName);

                    if (resource == null)
                        return false;

                    return resource.unlocked;
                }
            case RequirementType.resourceSupplyThreshold:
                {
                    var resource = ResourceProductionManager.Instance.TryGetResource(resourceName);

                    if (resource == null)
                        return false;

                    return resource.GetCurrentSupply() >= resourceSupplyThreshold;
                }
            case RequirementType.resouceUpgradeApplied:
                {
                    var upgrade = ResourceUpgradeManager.Instance.GetResourceUpgrade(upgradeName);

                    if (upgrade == null)
                        return false;

                    return upgrade.unlocked;
                }
            case RequirementType.playerGoldThreshold:
                {
                    return GameController.Instance.gold > playerGoldThreshold;
                }
        }

        return false;
    }
}