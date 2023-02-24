using TMPro;
using UnityEngine;

public class TextColoringTest : MonoBehaviour
{

    public string text = "hello world";

    public int from = 1;
    public int count = 3;

    [Space]

    public Color backColor = Color.white;
    public Color color = Color.red;



    TextMeshPro textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }


    private void Update()
    {
        textMesh.text = TextHelper.makeColoredString(text, from, count, backColor, color);
    }
}