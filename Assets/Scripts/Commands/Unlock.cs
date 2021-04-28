using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextInput/Commands/Unlock")]
public class Unlock : Command
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

        string unlockInput = string.Join(" ", separatedInputWords);

        if (unlockInput == "")
        {
            gameController.LogStringWithReturn($"Unlock command requires extra input. Please specify what you wish to Unlock. Inputs include Resources and Upgrades. Example:\n\n\t> Unlock Rune Axes");
            
            return;
        }

        ResourceObject resourceToUnlock = resourceProductionManager.TryGetResource(unlockInput);

        if (resourceToUnlock != null)
        {
            resourceToUnlock.TryUnlockResource();
            
            return;
        }

        var upgrade = resourceUpgradeManager.GetResourceUpgrade(unlockInput);

        if (upgrade != null)
        {
            upgrade.TryUnlockUpgrade();
            
            return;
        }

        gameController.LogStringWithReturn($"Unknown Upgrade command input.");
    }
}