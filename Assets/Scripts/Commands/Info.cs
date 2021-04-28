using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextInput/Commands/Info")]
public class Info : Command
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

        string infoInput = string.Join(" ", separatedInputWords);

        if (infoInput == "")
        {
            gameController.LogStringWithReturn($"Info command requires extra input. Please specify what you information on. Inputs include Resources and Upgrades. Example:\n\n\t> Info Rune Axes");
            
            return;
        }

        ResourceObject resource = resourceProductionManager.TryGetResource(infoInput);

        if (resource != null)
        {
            gameController.LogStringWithReturn($"{resource.resourceName}\t-\t{resource.goldToUnlock} Gold\n\n{resource.resourceDescription}");
            
            return;
        }

        var upgrade = resourceUpgradeManager.GetResourceUpgrade(infoInput);

        if (upgrade != null)
        {
            gameController.LogStringWithReturn($"{upgrade.upgradeName}\t-\t{upgrade.goldToUnlock} Gold\n\n{upgrade.upgradeDescription}");
            
            return;
        }

        gameController.LogStringWithReturn($"Unknown info command input.");
	}
}