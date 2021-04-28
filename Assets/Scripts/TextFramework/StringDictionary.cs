using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextInput/String Dictionary Entry")]
public class StringDictionary : ScriptableObject
{
    [HideInInspector] public string keyWord;

    [TextArea(10, 1000)] public string text;

    void OnValidate()
    {
        keyWord = name;
    }
}