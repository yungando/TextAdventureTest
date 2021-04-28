using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextInput/Commands/Check")]
public class Check : Command
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

        string checkInput = string.Join(" ", separatedInputWords);

        if (checkInput == "")
        {
            gameController.LogStringWithReturn($"Check command requires extra input. Please specify what you wish to check. Inputs include Gold, Resources and Upgrades. Example:\n\n\t> Check Wood");
            
            return;
        }

        ResourceObject resource = resourceProductionManager.TryGetResource(checkInput);

        if (resource != null)
        {
            if (resource.unlocked == false)
            {
                gameController.LogStringWithReturn($"You haven't unlocked {resource.resourceName}.");
            
                return;
            }

            float currentSupply = resource.GetCurrentSupply();
            float supplyCapacity = resource.GetSupplyCapacity();
            float totalValue = resource.GetGoldValue() * currentSupply;

            gameController.LogStringWithReturn($"Current supply of {(int)currentSupply}/{(int)supplyCapacity} {resource.name} is worth {(int)totalValue} Gold.");
            
            return;
        }

        if (checkInput == "gold")
        {
            float playerGold = gameController.gold;

            gameController.LogStringWithReturn($"Current supply of Gold is {(int)playerGold}.");
            
            return;
        }

        if (checkInput == "upgrade" || checkInput == "upgrades")
        {
            List<ResourceUpgrade> upgrades = resourceUpgradeManager.GetUnlockedUpgrades(true);

            if(upgrades.Count == 0)
            {
                gameController.LogStringWithReturn($"No upgrades currently unlocked.");
                return;
            }

            List<string> upgradeInfo = new List<string>();

            List<ResourceUpgrade> _coalUpgrades = new List<ResourceUpgrade>();
            List<ResourceUpgrade> _fishUpgrades = new List<ResourceUpgrade>();
            List<ResourceUpgrade> _wheatUpgrades = new List<ResourceUpgrade>();
            List<ResourceUpgrade> _woodUpgrades = new List<ResourceUpgrade>();
            List<ResourceUpgrade> _specialUpgrades = new List<ResourceUpgrade>();

            List<string> upgradesList = new List<string>();

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

            if (_woodUpgrades.Count != 0)
            {
                List<string> woodUpgradeList = new List<string>();

                woodUpgradeList.Add("\tWood Upgrades:\n");

                foreach (var upgrade in _woodUpgrades)
                {
                    woodUpgradeList.Add($"\t\t{upgrade.upgradeName}");
                }

                upgradeInfo.Add(string.Join("\n", woodUpgradeList));
            }

            if (_coalUpgrades.Count != 0)
            {
                List<string> coalUpgradeList = new List<string>();

                coalUpgradeList.Add("\tCoal Upgrades:\n");

                foreach (var upgrade in _coalUpgrades)
                {
                    coalUpgradeList.Add($"\t\t{upgrade.upgradeName}");
                }

                upgradeInfo.Add(string.Join("\n", coalUpgradeList));
            }

            if (_fishUpgrades.Count != 0)
            {
                List<string> fishUpgradeList = new List<string>();

                fishUpgradeList.Add("\tFish Upgrades:\n");

                foreach (var upgrade in _fishUpgrades)
                {
                    fishUpgradeList.Add($"\t\t{upgrade.upgradeName}");
                }

                upgradeInfo.Add(string.Join("\n", fishUpgradeList));
            }

            if (_wheatUpgrades.Count != 0)
            {
                List<string> wheatUpgradeList = new List<string>();

                wheatUpgradeList.Add("\tWheat Upgrades:\n");

                foreach (var upgrade in _wheatUpgrades)
                {
                    wheatUpgradeList.Add($"\t\t{upgrade.upgradeName}");
                }

                upgradeInfo.Add(string.Join("\n", wheatUpgradeList));
            }

            if (_specialUpgrades.Count != 0)
            {
                List<string> specialUpgradeList = new List<string>();

                specialUpgradeList.Add("\tSpecial Upgrades:\n");

                foreach (var upgrade in _specialUpgrades)
                {
                    specialUpgradeList.Add($"\t\t{upgrade.upgradeName}");
                }

                upgradeInfo.Add(string.Join("\n", specialUpgradeList));
            }

            if (upgradeInfo.Count == 1)
            {
                upgradeInfo.Add("\tAll upgrades are unlocked.");
            }

            gameController.LogStringWithReturn($"Current unlocked upgrades are:{string.Join("\n\n", upgradeInfo)}");
            
            return;
        }

        gameController.LogStringWithReturn($"Unknown Check command input.");
    }
}