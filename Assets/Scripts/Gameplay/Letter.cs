using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.Sprites;
using System.Collections.Generic;
using TMPro;

[DefaultExecutionOrder(1)]
public class Letter : MonoBehaviour
{

    public int letterId => LetterUtility.charToLetterId(_letterId);

    public int segmentCount => segments.Count;
    public TextMeshPro text { get; private set; }

    public List<LetterSegment> segments { get; private set; } = new List<LetterSegment>();

    [SerializeField]
    char _letterId;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshPro>();
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            var letterSegment = child.GetComponent<LetterSegment>();
            if (letterSegment)
            {
                segments.Add(letterSegment);
                letterSegment.gameObject.SetActive(false);
            }
        }
    }

    public LetterSegment get(int index) => segments[index];


    public void setTextEnabled(bool value)
    {
        text.gameObject.SetActive(value);
    }
}
