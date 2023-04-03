using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
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


    public List<WordInfo> listAll()
    {
        return wordInfos;
    }

    public WordInfo getWordByStartingLetter(int letterId)
    {
        var letterStr = LetterUtility.letterToString(letterId);
        var words = StartingLetters.getWords(letterStr);
        var listOfWords = new List<string>(words);


        while (listOfWords.Count > 0)
        {
            var randomIndex = Random.Range(0, listOfWords.Count);
            var found = wordInfos.Find(x => x.word == listOfWords[randomIndex]);
            listOfWords.RemoveAt(randomIndex);
            if (found != null)
                return found;
        }
        return null;
    }
    [System.Obsolete]
    public WordInfo getRandomContains(int letterId)
    {
        List<WordInfo> wordsWithLetter = new List<WordInfo>(wordInfos.Count);
        foreach (var x in wordInfos)
        {
            if (x.containsLetter(letterId) && x.clip != null)
                wordsWithLetter.Add(x);
        }
        List<WordInfo> wordsWithArt = new List<WordInfo>(wordsWithLetter.Count);
        foreach (var x in wordsWithLetter)
            if (x.prefab)
                wordsWithArt.Add(x);
        if (wordsWithArt.Count == 0)
            return wordsWithLetter.getRandom();
        return wordsWithArt.getRandom();
    }
    public WordInfo findWord(string name)
    {
        return wordInfos.Find(x => x.word == name);
    }


#if UNITY_EDITOR
    [ContextMenu("AutoSetup")]
    void AutoSetup()
    {
        Undo.RegisterCompleteObjectUndo(this, "AutoSetup");
        foreach (var x in wordInfos)
        {
            if (x.word == null)
                continue;

            x.prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/WordObjects/" + x.word + ".prefab");
            x.picture = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Textures/Words/" + x.word + ".png");
            x.clip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/AudioClips/Spelling/" + x.word + ".wav");

            var totalClipCount = x.word.Length;
            for (int i = 0; i < x.word.Length; i++)
                if (x.isDigraph(i))
                    totalClipCount--;

            x.spellingClips = new AudioClip[totalClipCount];
            for (int i = 0; i < x.spellingClips.Length; i++)
            {
                if (x.isDigraph(i))
                    x.spellingClips[i] = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/AudioClips/Spelling/" + x.word + "_" + x.word[i] + x.word[i + 1] + ".wav");
                else
                    x.spellingClips[i] = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/AudioClips/Spelling/" + x.word + "_" + x.word[i] + ".wav");
            }
        }
    }
#endif
}

[System.Serializable]
public class WordInfo : IEnumerable<int>
{
    public string word;
    public GameObject prefab;
    public Sprite picture;
    public AudioClip clip;
    public AudioClip[] spellingClips;
    [SerializeField] List<char> startingLetters = new List<char>();


    public bool isStartingLetter(int letterId)
    {
        return startingLetters.Contains(LetterUtility.letterToChar(letterId));
    }
    public bool isDigraph(int letterIndex)
    {
        return letterIndex < word.Length - 1 && LetterUtility.isDigraph(getLetterId(letterIndex), getLetterId(letterIndex + 1));
    }
    public int digraphCount(int letterIndex)
    {
        if (isDigraph(letterIndex))
            return 2;
        return 1;
    }



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

    public IEnumerator<int> GetEnumerator()
    {
        foreach (var x in word)
            yield return LetterUtility.charToLetterId(x);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}