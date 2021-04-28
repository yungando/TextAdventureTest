using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "TextInput/Commands/List")]
public class List : Command
{
    public override void RespondToInput(string[] separatedInputWords)
    {
        GameController gameController = GameController.Instance;
        ResourceProductionManager resourceProductionManager = ResourceProductionManager.Instance;
        ResourceUpgradeManager resourceUpgradeManager = ResourceUpgradeManager.Instance;

        if (gameController.gameRunning == false)
        {
            gameController.LogStringWithReturn($"Unknown command.");

            return;
        }

        List<string> upgradeInfo = new List<string>();

        List<ResourceUpgrade> _coalUpgrades = new List<ResourceUpgrade>();
        List<ResourceUpgrade> _fishUpgrades = new List<ResourceUpgrade>();
        List<ResourceUpgrade> _wheatUpgrades = new List<ResourceUpgrade>();
        List<ResourceUpgrade> _woodUpgrades = new List<ResourceUpgrade>();
        List<ResourceUpgrade> _specialUpgrades = new List<ResourceUpgrade>();

        upgradeInfo.Add("All available new resources and upgrades are available here. Use the <i>Info</i> command to read unlock requirments and descriptions. Use the <i>Unlock</i> command to purchase with Gold.");

        bool allResourcesUnlocked = false;

        var resources = resourceProductionManager.GetUnlockedResources(false).OrderBy(resource => resource.goldToUnlock).ToList();

        if (resources.Count == 0)
        {
            allResourcesUnlocked = true;
        }

        if (allResourcesUnlocked == false)
        {
            List<string> resourceStringList = new List<string>();

            resourceStringList.Add("\tNew Resources:\n");

            foreach (var resource in resources)
            {
                resourceStringList.Add($"\t\t{resource.resourceName}\t\t-\t\t{resource.goldToUnlock} Gold");
            }

            upgradeInfo.Add(string.Join("\n", resourceStringList));
        }

        var upgrades = resourceUpgradeManager.GetUnlockedUpgrades(false).OrderBy(upgrade => upgrade.goldToUnlock).ToList();

        foreach (var upg in upgrades)
        {
            switch (upg.UpgradeCategory)
            {
                case (ResourceUpgradeCategory.Coal):
                    _coalUpgrades.Add(upg);
                    break;
                case (ResourceUpgradeCategory.Fish):
                    _fishUpgrades.Add(upg);
                    break;
                case (ResourceUpgradeCategory.Wheat):
                    _wheatUpgrades.Add(upg);
                    break;
                case (ResourceUpgradeCategory.Wood):
                    _woodUpgrades.Add(upg);
                    break;
                case (ResourceUpgradeCategory.Special):
                    _specialUpgrades.Add(upg);
                    break;
            }
        }

        if (resourceProductionManager.TryGetResource("Wood").unlocked && _woodUpgrades.Count != 0)
        {
            List<string> woodUpgradeList = new List<string>();

            woodUpgradeList.Add("\tWood Upgrades:\n");

            foreach (var upgrade in _woodUpgrades)
            {
                woodUpgradeList.Add($"\t\t{upgrade.upgradeName}\t\t-\t\t{upgrade.goldToUnlock} Gold");
            }

            upgradeInfo.Add(string.Join("\n", woodUpgradeList));
        }

        if (resourceProductionManager.TryGetResource("Coal").unlocked && _coalUpgrades.Count != 0)
        {
            List<string> coalUpgradeList = new List<string>();

            coalUpgradeList.Add("\tCoal Upgrades:\n");

            foreach (var upgrade in _coalUpgrades)
            {
                coalUpgradeList.Add($"\t\t{upgrade.upgradeName}\t\t-\t\t{upgrade.goldToUnlock} Gold");
            }

            upgradeInfo.Add(string.Join("\n", coalUpgradeList));
        }

        if (resourceProductionManager.TryGetResource("Fish").unlocked && _fishUpgrades.Count != 0)
        {
            List<string> fishUpgradeList = new List<string>();

            fishUpgradeList.Add("\tFish Upgrades:\n");

            foreach (var upgrade in _fishUpgrades)
            {
                fishUpgradeList.Add($"\t\t{upgrade.upgradeName}\t\t-\t\t{upgrade.goldToUnlock} Gold");
            }

            upgradeInfo.Add(string.Join("\n", fishUpgradeList));
        }

        if (resourceProductionManager.TryGetResource("Wheat").unlocked && _wheatUpgrades.Count != 0)
        {
            List<string> wheatUpgradeList = new List<string>();

            wheatUpgradeList.Add("\tWheat Upgrades:\n");

            foreach (var upgrade in _wheatUpgrades)
            {
                wheatUpgradeList.Add($"\t\t{upgrade.upgradeName}\t\t-\t\t{upgrade.goldToUnlock} Gold");
            }

            upgradeInfo.Add(string.Join("\n", wheatUpgradeList));
        }

        if (allResourcesUnlocked && _specialUpgrades.Count != 0)
        {
            List<string> specialUpgradeList = new List<string>();

            specialUpgradeList.Add("\tSpecial Upgrades:\n");

            foreach (var upgrade in _specialUpgrades)
            {
                specialUpgradeList.Add($"\t{upgrade.upgradeName}\t\t-\t\t{upgrade.goldToUnlock} Gold");
            }

            upgradeInfo.Add(string.Join("\n", specialUpgradeList));
        }

        if (upgradeInfo.Count == 1)
        {
            upgradeInfo.Add("\tAll upgrades are unlocked.");
        }

        gameController.LogStringWithReturn($"{string.Join("\n\n", upgradeInfo)}");
    }
}