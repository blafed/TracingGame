using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WordList", menuName = "KidLetters/WordList", order = 0)]
public class WordList : ScriptableObject
{
    static WordList _o;
    public static WordList o => Extensions2.ResourcesLoad("WordList", ref _o);


    [SerializeField]
    List<WordInfo> wordInfos = new List<WordInfo>()
    {
        new WordInfo{
            word = "cat"
        }
    };





    public WordInfo getRandomContains(int letterId)
    {
        List<WordInfo> wordWithLetter = new List<WordInfo>(wordInfos.Count);
        foreach (var x in wordInfos)
        {
            if (x.containsLetter(letterId))
                wordWithLetter.Add(x);
        }
        Debug.Log(wordWithLetter.Count);
        return wordWithLetter.getRandom();
    }
}

[System.Serializable]
public class WordInfo
{
    public string word;
    public GameObject prefab;
    public Sprite picture;
    public AudioClip clip;



    public override string ToString()
    {
        return word;
    }

    public int letterCount => word.Length;
    public bool containsLetter(int letterId)
    {
        var c = LetterUtility.letterToChar(letterId);
        return word.Contains(c);
    }

    public int getLetterId(int index) => LetterUtility.charToLetterId(word[index]);

    public int indexOfLetter(int letterId)
    {
        return word.IndexOf(LetterUtility.letterToChar(letterId));
    }
}