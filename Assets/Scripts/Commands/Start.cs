using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextInput/Commands/Start")]
public class Start : Command
{
    [SerializeField] private bool resetGameOnRun = true;

    public override void RespondToInput(string[] separatedInputWords)
    {
        GameController gameController = GameController.Instance;

        if (gameController.gameRunning == true)
        {
            gameController.LogStringWithReturn($"Unknown command.");

            return;
        }

        gameController.LogStringWithReturn($"Starting New Game....");

        var tempSingleton1 = ProceduralGameLoop.Instance;
        var tempSingleton2 = ResourceProductionManager.Instance;
        var tempSingleton3 = ResourceUpgradeManager.Instance;

        if (resetGameOnRun)
        {
            tempSingleton2.FreshStartProtocol();
        }

        gameController.gameRunning = true;
    }
}