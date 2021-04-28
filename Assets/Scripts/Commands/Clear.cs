using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextInput/Commands/Clear")]
public class Clear : Command
{
    public override void RespondToInput(string[] separatedInputWords)
    {
        GameController.Instance.ClearMainText();
    }
}