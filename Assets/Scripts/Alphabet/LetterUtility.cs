using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

public static class LetterUtility
{
    public static readonly int lowerMin = 0;
    public static readonly int upperMin = sizeSet;
    public static readonly int sizeSet = 26;
    public static readonly int count = sizeSet * 2;
    public static bool isUpper(int id) => id >= sizeSet;
    public static char letterToChar(int id)
    {
        if (id >= sizeSet)
            return ((char)('A' + id - sizeSet));
        return ((char)('a' + (char)id));
    }
    public static string letterToString(int id)
    {
        if (id >= sizeSet)
            return ((char)('A' + id - sizeSet)).ToString();
        return ((char)('a' + (char)id)).ToString();
    }
    public static int charToLetterId(char c)
    {
        if (char.IsUpper(c))
            return (int)c - 'A' + upperMin;
        else
            return (int)c - 'a' + lowerMin;
    }

    public static bool isDigraph(int letterA, int letterB)
    {
        char ca = letterToChar(letterA);
        char cb = letterToChar(letterB);
        string sum = ca.ToString() + cb;
        return sum == "sh" || sum == "ch" || sum == "th" || sum == "ph" || sum == "ck" || sum == "ng" || sum == "qu" || sum == "wh";
    }


    static int[] _allLetters;
    public static int[] listAllLetters()
    {
        if (_allLetters == null)
        {
            _allLetters = new int[count];
            for (int i = 0; i < count; i++)
                _allLetters[i] = i + lowerMin;
        }
        return _allLetters;
    }

}