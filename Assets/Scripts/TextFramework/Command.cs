using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command : ScriptableObject
{
    [HideInInspector] public string keyWord;

    public abstract void RespondToInput(string[] separatedInputWords);

    void OnValidate()
    {
        keyWord = name;
    }
}