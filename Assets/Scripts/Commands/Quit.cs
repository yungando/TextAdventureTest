using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextInput/Commands/Quit")]
public class Quit : Command
{
    public override void RespondToInput(string[] separatedInputWords)
    {
        GameController.Instance.LogStringWithReturn("Quitting application.");
            
        Debug.Log("Quitting application.");
        Application.Quit();
    }
}