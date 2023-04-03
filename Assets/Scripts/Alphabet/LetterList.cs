using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;

#endif

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

#if UNITY_EDITOR
    [ContextMenu("AutoSetup")]
    void AutoSetup()
    {
        Undo.RegisterCompleteObjectUndo(this, "AutoSetup LetterList");
        audioClips.Clear();
        for (int i = 0; i < LetterUtility.sizeSet; i++)
        {
            var c = LetterUtility.letterToChar(i);
            var clip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/AudioClips/LetterNames/" + c + ".wav");
            audioClips.Add(clip);
        }

    }
#endif
}