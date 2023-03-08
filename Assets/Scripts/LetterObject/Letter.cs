using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.Sprites;
using System.Collections.Generic;
using TMPro;

[DefaultExecutionOrder(1)]
public class Letter : MonoBehaviour
{

    public event System.Action onClick;
    public Rect rect => Extensions2.rectFromCenter(transform.position, size);
    public Vector2 size => _size;
    [SerializeField] Vector2 _size;
    public int letterId => LetterUtility.charToLetterId(name[0]);

    public int segmentCount => segments.Count;
    public TextMeshPro text { get; private set; }

    public List<LetterSegment> segments { get; private set; } = new List<LetterSegment>();

    // [SerializeField]
    // char _letterId => name[0];

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

    private void OnDrawGizmosSelected()
    {
        rect.drawGizmos(Color.cyan);
    }
    private void OnMouseDown()
    {
        onClick?.Invoke();
    }


}
