using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "LetterList", menuName = "KidLetters/LetterList", order = 0)]
public class LetterList : ScriptableObject
{

    static LetterList _o;
    public static LetterList o => Extensions2.ResourcesLoad(nameof(LetterList), ref _o);
    [SerializeField]
    List<AudioClip> audioClips = new List<AudioClip>();


    public AudioClip getAudioClip(int letterId)
    {
        if (LetterUtility.isUpper(letterId))
            letterId -= LetterUtility.upperMin;
        else
            letterId -= LetterUtility.lowerMin;
        return audioClips[letterId];
    }
}