using TMPro;
using UnityEngine;

public class HighlightText : MonoBehaviour
{
    public int startFrom;
    public int endTo;

    public Color color = Color.white;
    TextMeshPro _text;
    private void Awake()
    {
        _text = GetComponent<TextMeshPro>();
    }

    private void Update()
    {
    }



    public void changeColor(int from, int to, Color color)
    {

    }



    public void applyColor(int charIndex)
    {
        var vertIndex = _text.textInfo.characterInfo[charIndex].vertexIndex;

        var vertices = _text.textInfo.meshInfo[0].vertices;


    }


}