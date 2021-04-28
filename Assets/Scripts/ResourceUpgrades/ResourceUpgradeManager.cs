using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ResourceUpgradeManager : Singleton<ResourceUpgradeManager>
{
    private ResourceProductionManager _resourceProductionManager;

    private List<ResourceUpgrade> _upgrades = new List<ResourceUpgrade>();

    public override void Awake()
    {
        base.Awake();

        _resourceProductionManager = ResourceProductionManager.Instance;

        foreach (ResourceUpgrade u in Resources.LoadAll("", typeof(ResourceUpgrade)))
        {
            _upgrades.Add(u);
        }
    }

    public List<ResourceUpgrade> GetAllUpgrades()
    {
        return _upgrades;
    }

    /// <summary>
    /// Returns a list of locked or unlocked upgrades
    /// </summary>
    /// <param name="unlocked">True returns all unlocked upgrades, False for locked upgrades.</param>
    public List<ResourceUpgrade> GetUnlockedUpgrades(bool unlocked)
    {
        var upgrades = new List<ResourceUpgrade>();

        foreach (var item in _upgrades)
        {
            if (item.unlocked == unlocked)
            {
                upgrades.Add(item);
            }
        }
        
        return upgrades;
    }

    public ResourceUpgrade GetResourceUpgrade(string upgradeName)
    {
        foreach (var upgrade in _upgrades)
        {
            if (upgrade.upgradeName.ToLower() == upgradeName.ToLower())
            {
                return upgrade;
            }
        }

        return null;
    }

    public void TryUnlockUpgrade(ResourceUpgrade upgrade)
    {
        upgrade.TryUnlockUpgrade();

        return;
    }
}