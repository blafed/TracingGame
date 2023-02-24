using DG.Tweening;
using TMPro;
using UnityEngine;

public static class TextHelper
{

    public static string addColor(string text, Color col) => $"<color={colorHexFromUnityColor(col)}>{text}</color>";
    public static string colorHexFromUnityColor(Color unityColor) => $"#{ColorUtility.ToHtmlStringRGBA(unityColor)}";


    public static string makeColoredString(string t, int from, int count, Color defaultColor, Color color)
    {
        var x = t.Substring(from, count);
        var a = t.Substring(0, from);
        var b = t.Substring(from + count, t.Length - from - count);

        return
        (a.Length > 0 ? addColor(a, defaultColor) : "") +
         addColor(x, color) +
        (b.Length > 0 ? addColor(b, defaultColor) : "");
    }

    // public static void changeColor(TextMeshPro text, int from, int count, Color color)
    // {
    //     text.text = makeColoredString(text.text, )
    //     // TMP_WordInfo info = _text.textInfo.wordInfo[wordIndex];
    //     var info = text.textInfo;
    //     for (int i = from; i < from + count; ++i)
    //     {
    //         int charIndex = i;
    //         int meshIndex = text.textInfo.characterInfo[charIndex].materialReferenceIndex;
    //         int vertexIndex = text.textInfo.characterInfo[charIndex].vertexIndex;

    //         Color32[] vertexColors = text.textInfo.meshInfo[meshIndex].colors32;
    //         vertexColors[vertexIndex + 0] = color;
    //         vertexColors[vertexIndex + 1] = color;
    //         vertexColors[vertexIndex + 2] = color;
    //         vertexColors[vertexIndex + 3] = color;
    //     }
    // }
}