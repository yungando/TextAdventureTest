using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextInput/Commands/Sell")]
public class Sell : Command
{
    public override void RespondToInput(string[] separatedInputWords)
    {
        GameController gameController = GameController.Instance;
		ResourceProductionManager resourceProductionManager = ResourceProductionManager.Instance;
        
        if (gameController.gameRunning == false)
        {
            gameController.LogStringWithReturn($"Unknown command.");

            return;
        }

        string sellInput = string.Join(" ", separatedInputWords);

        if (sellInput == "")
        {
            gameController.LogStringWithReturn($"Sell command requires extra input. Please specify what resource you wish to sell. Example:\n\n\t> Sell Wood");
            
            return;
        }

        ResourceObject resource = resourceProductionManager.TryGetResource(sellInput);

        if (resource == null)
        {
            gameController.LogStringWithReturn($"Could not find \"{sellInput}\" resource.");

            return;
        }

        if (resource.unlocked == false)
        {
            gameController.LogStringWithReturn($"You haven't unlocked {resource.resourceName}");

            return;
        }

        int sellAmount = resource.GetCurrentSupply();
        
        float totalValue = resourceProductionManager.SellResource(resource, sellAmount);
        
        gameController.LogStringWithReturn($"Sold current supply of {(int)sellAmount} {resource.name} for {(int)totalValue} Gold.");
    }
}