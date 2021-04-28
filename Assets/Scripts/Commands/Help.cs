using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextInput/Commands/Help")]
public class Help : Command
{
	public override void RespondToInput(string[] separatedInputWords)
	{
		GameController gameController = GameController.Instance;

        string stringRequest = string.Join(" ", separatedInputWords);

        if (stringRequest == "")
        {
            stringRequest = "help";
        }

		for (int i = 0; i < gameController.textInput.stringDictionary.Count; i++)
		{
			StringDictionary stringDictionary = gameController.textInput.stringDictionary[i];
			if (stringDictionary.keyWord == stringRequest)
			{
				gameController.LogStringWithReturn(gameController.textInput.stringDictionary[i].text);
				break;
			}
		}
	}
}