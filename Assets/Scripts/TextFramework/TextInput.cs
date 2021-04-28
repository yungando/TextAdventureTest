using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

using TMPro;

public class TextInput : MonoBehaviour
{
    public TMP_InputField inputField;

    public List<Command> commands = new List<Command>();
    public List<StringDictionary> stringDictionary = new List<StringDictionary>();

    [HideInInspector] public bool TextAnimRunning = false;

    void Awake()
    {
        inputField.onEndEdit.AddListener(AcceptStringInput);

        foreach (Command c in Resources.LoadAll("", typeof(Command)))
        {
            commands.Add(c);
        }

        foreach (StringDictionary s in Resources.LoadAll("", typeof(StringDictionary)))
        {
            stringDictionary.Add(s);
        }

        UnlockInputField();
    }

    void OnDestroy()
    {
        inputField.onEndEdit.RemoveListener(AcceptStringInput);
    }

    void AcceptStringInput(string userInput)
    {
        ClearInputField();
        UnlockInputField();

        if (TextAnimRunning)
            GameController.Instance.SkipTextAnimation();

        if (userInput == "")
            return;

        GameController.Instance.LogStringWithReturn("> " + userInput);

        TryExecuteCommand(userInput);

        GameController.Instance.UpdateMainText();
    }

    void TryExecuteCommand(string userInput)
    {
        bool executedCommand = false;

        string[] separatedInputWords = userInput.Split(' ');

        for (int i = 0; i < commands.Count; i++)
        {
            Command cmd = commands[i];
            if (cmd.keyWord.ToLower() == separatedInputWords[0].ToLower())
            {
                cmd.RespondToInput(separatedInputWords.Skip(1).ToArray());
                executedCommand = true;
                break;
            }
        }

        if (!executedCommand)
        {
            GameController.Instance.LogStringWithReturn("Unknown command.");
        }
    }

    public void ClearInputField()
    {
        inputField.text = null;
    }

    public void LockInputField()
    {
        inputField.DeactivateInputField();
    }

    public void UnlockInputField()
    {
        inputField.ActivateInputField();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.Escape))
        {
            if (TextAnimRunning)
            {
                GameController.Instance.SkipTextAnimation();
            }

            UnlockInputField();
        }
    }
}