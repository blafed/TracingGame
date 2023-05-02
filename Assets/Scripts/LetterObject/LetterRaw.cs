using DG.Tweening;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.Sprites;
using System.Collections.Generic;
using TMPro;
using KidLetters;

[DefaultExecutionOrder(1)]
public class LetterRaw : MonoBehaviour
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
    // public TextMeshPro text { get; private set; }

    public List<LetterRawSegment> segments { get; private set; } = new List<LetterRawSegment>();

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
        var text = GetComponentInChildren<TextMeshPro>(true);
        text.gameObject.SetActive(false);
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            var letterSegment = child.GetComponent<LetterRawSegment>();
            if (letterSegment)
            {
                segments.Add(letterSegment);
                letterSegment.gameObject.SetActive(false);
            }
        }
    }


    public LetterRawSegment get(int index) => segments[index];



    private void OnMouseDown()
    {
        onClick?.Invoke();
    }



    private void OnDrawGizmosSelected()
    {
        rect.drawGizmos(Color.cyan);
        viewRect.drawGizmos(Color.magenta);
    }


    public Glyph generateGlyph()
    {
        List<GlyphSegment> segments = new List<GlyphSegment>();
        foreach (var x in this.segments)
        {
            var seg = new GlyphSegment(x.path, x.isDot, x.transform.localPosition);
            segments.Add(seg);
        }
        var glyph = new Glyph(segments, relativeViewRect);
        return glyph;
    }

}
