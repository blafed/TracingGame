using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parses StartingLetters.csv to determinate which word to play for a letter
/// </summary>
public static class StartingLetters
{
    static TextAsset csvText;
    static Dictionary<string, List<string>> letterToWords = new Dictionary<string, List<string>>();

    static bool isInit;
    public static List<string> getWords(string letter)
    {
        init();
        if (letterToWords.ContainsKey(letter))
            return letterToWords[letter];
        return new List<string>();
    }
    public static void init()
    {
        if (isInit)
            return;
        isInit = true;
        csvText = Resources.Load<TextAsset>("StartingLetters");

        //iterate over lines of csvText
        foreach (var x in csvText.text.lines())
        {
            if (x == "" || x[0] == '#')
                continue;
            //split line into columns by comma
            var columns = x.Split(',', System.StringSplitOptions.RemoveEmptyEntries);

            //first column is letter
            var letter = columns[0];
            //other columns are words
            var words = new List<string>();
            for (int i = 1; i < columns.Length; i++)
                words.Add(columns[i]);
            //add to dictionary
            letterToWords.Add(letter, words);
        }
    }
}