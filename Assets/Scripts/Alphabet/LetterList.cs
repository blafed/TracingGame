using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

#if UNITY_EDITOR
using UnityEditor;

#endif

[CreateAssetMenu(fileName = "LetterList", menuName = "KidLetters/LetterList", order = 0)]
public class LetterList : ScriptableObject
{

    static LetterList _o;
    public static LetterList o => Extensions2.ResourcesLoad(nameof(LetterList), ref _o);
    // [SerializeField]
    // List<AudioClip> audioClips = new List<AudioClip>();

    [SerializeField]
    List<LetterInfo> letterInfos = new List<LetterInfo>();

    public AudioClip getAudioClip(int letterId)
    {
        if (LetterUtility.isUpper(letterId))
            letterId -= LetterUtility.upperMin;
        else
            letterId -= LetterUtility.lowerMin;
        return letterInfos[letterId].clip;
    }

#if UNITY_EDITOR
    [ContextMenu("AutoSetup")]
    void AutoSetup()
    {
        Undo.RegisterCompleteObjectUndo(this, "AutoSetup LetterList");
        letterInfos.Clear();

        for (int i = 0; i < LetterUtility.sizeSet; i++)
        {
            var letterInfo = new LetterInfo();
            letterInfo.letterId = i;
            var c = LetterUtility.letterToChar(i);
            letterInfo.clip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/AudioClips/LetterNames/" + c + ".wav");
            letterInfos.Add(letterInfo);
        }

    }
#endif
}


[System.Serializable]
public class LetterInfo
{
    public int letterId;
    public AudioClip clip;
    public List<string> words = new List<string>();
}