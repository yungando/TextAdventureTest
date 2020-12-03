using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI mainText;
    public TMP_InputField inputField;
    public float delay;

    private int visibleCount = 0;
    private bool textEffectRunning;
    private Coroutine runningCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        //mainText.SetText("<<<< Welcome >>>>");
        mainText.ForceMeshUpdate();

        runningCoroutine = StartCoroutine(ShowMainText());
    }

    IEnumerator ShowMainText()
    {
        textEffectRunning = true;

        inputField.DeactivateInputField();

        TMP_TextInfo textInfo = mainText.textInfo;

        int totalVisibleCharacters = textInfo.characterCount; // Get # of Visible Character in text object

        while (visibleCount <= totalVisibleCharacters)
        {
            mainText.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?

            visibleCount += 1;

            yield return new WaitForSeconds(delay);
        }

        mainText.ForceMeshUpdate();

        textEffectRunning = false;

        inputField.ActivateInputField();
    }

    // Update is called once per frame
    void Update()
    {
        if (textEffectRunning && (Input.GetKey(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            StopCoroutine(runningCoroutine);

            mainText.maxVisibleCharacters = mainText.textInfo.characterCount;
            mainText.ForceMeshUpdate();

            inputField.ActivateInputField();

            textEffectRunning = false;
        }
        else if (textEffectRunning == false)
        {
            if (Input.GetKey(KeyCode.Return))
            {
                if (inputField.text != "")
                {
                    visibleCount = mainText.maxVisibleCharacters;
                    mainText.SetText(mainText.text + "\n\n> " + inputField.text);
                    mainText.ForceMeshUpdate();

                    inputField.text = "";

                    runningCoroutine = StartCoroutine(ShowMainText());
                }
                else
                {
                    inputField.ActivateInputField();
                }
            }
        }

            if (Input.GetMouseButtonDown(0))
        {
            inputField.ActivateInputField();
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            inputField.ActivateInputField();
        }
    }
}