using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class GameController : Singleton<GameController>
{
    [SerializeField] public float gold = 0;
    [HideInInspector] public bool gameRunning = false;
    [HideInInspector] public TextInput textInput;

    public TextMeshProUGUI mainText;

    [SerializeField, Range(0.01f, 1.0f)] private float delay = 0.01f;
    
    private int mainTextVisibleCount = 0;

    private Coroutine runningCoroutine;

    List<string> actionLog = new List<string>();

    public override void Awake()
    {
        base.Awake();
        
        textInput = GetComponent<TextInput>();

        gameRunning = false;
    }

    void Start()
    {
        ClearMainText();
    }

    public void ClearMainText()
    {
        mainText.text = "";
        mainText.maxVisibleCharacters = 0;
        actionLog.Clear();

        for (int i = 0; i < textInput.stringDictionary.Count; i++)
        {
            if (textInput.stringDictionary[i].keyWord == "start")
            {
                LogStringWithReturn(textInput.stringDictionary[i].text);
            }
        }

        UpdateMainText();

        runningCoroutine = StartCoroutine(DisplayMainText());
    }

    IEnumerator DisplayMainText()
    {
        textInput.TextAnimRunning = true;

        TMP_TextInfo textInfo = mainText.textInfo;

        int totalVisibleCharacters = textInfo.characterCount;

        while (mainTextVisibleCount <= totalVisibleCharacters)
        {
            mainText.maxVisibleCharacters = mainTextVisibleCount;
            mainTextVisibleCount += 1;

            yield return new WaitForSeconds(delay);
        }

        mainText.ForceMeshUpdate();

        textInput.TextAnimRunning = false;
    }

    public void SkipTextAnimation()
    {
        StopCoroutine(runningCoroutine);

        mainText.maxVisibleCharacters = mainText.textInfo.characterCount;
        mainText.ForceMeshUpdate();

        textInput.TextAnimRunning = false;
    }

    public void UpdateMainText()
    {
        string logAsText = string.Join("\n\n", actionLog.ToArray());

        mainTextVisibleCount = mainText.maxVisibleCharacters;
        mainText.text = logAsText;
        mainText.ForceMeshUpdate();

        runningCoroutine = StartCoroutine(DisplayMainText());
    }

    public void LogStringWithReturn(string stringToAdd)
    {
        actionLog.Add(stringToAdd);
    }
}