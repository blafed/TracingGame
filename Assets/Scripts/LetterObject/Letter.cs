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
    public Rect relativeViewRect => _viewRect;
    public Rect viewRect => Extensions2.rectFromCenter(_viewRect.center + transform.position.toVector2(), _viewRect.size);
    [SerializeField] Vector2 _size;
    [SerializeField] Rect _viewRect = Extensions2.rectFromCenter(Vector2.zero, Vector2.one * 4);
    public int letterId => LetterUtility.charToLetterId(name[0]);

    public int segmentCount => segments.Count;
    public TextMeshPro text { get; private set; }

    public List<LetterSegment> segments { get; private set; } = new List<LetterSegment>();

    #region  GetCustomPatternCode
    [SerializeField]
    bool useCustomPattern;
    [SerializeField]
    List<PatternCode> customPatterns = new List<PatternCode>();

    public PatternCode getCustomPattern(int index)
    {
        if (useCustomPattern)
            if (index < customPatterns.Count)
                return customPatterns[index];
        return PatternCode.none;
    }
    #endregion
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
        viewRect.drawGizmos(Color.magenta);
    }
    private void OnMouseDown()
    {
        onClick?.Invoke();
    }
}
