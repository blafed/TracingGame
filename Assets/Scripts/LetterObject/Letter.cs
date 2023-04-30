using DG.Tweening;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.Sprites;
using System.Collections.Generic;
using TMPro;
using KidLetters;

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
    // public TextMeshPro text { get; private set; }

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


    LetterFiller letterFiller;


    public LetterFiller filler => letterFiller;



    private void Awake()
    {
        var text = GetComponentInChildren<TextMeshPro>(true);
        text.gameObject.SetActive(false);
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


        createBasedFiller(LetterObjectConfig.o.standardLetterFillerPrefab);
    }


    public LetterSegment get(int index) => segments[index];


    public void setTextEnabled(bool value)
    {
        letterFiller.gameObject.SetActive(value);
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


    public void setColor(Color color)
    {
        _tweenColor = color;
        letterFiller.setColor(color);
    }
    public void setAlpha(float alpha)
    {
        _tweenAlpha = alpha;
        letterFiller.setAlpha(alpha);
    }

    Color _tweenColor = Color.white;
    float _tweenAlpha = 1;


    Tween _doColorTween;
    Tween _doFadeTween;



    public Tween doColor(Color color, float duration)
    {
        return _doColorTween = DOTween.To(() => _tweenColor, x => setColor(x), color, duration);
    }
    public Tween doFade(float alpha, float duration)
    {
        return _doFadeTween = DOTween.To(() => _tweenAlpha, x => letterFiller.setAlpha(x), alpha, duration);
    }
    public void doKill()
    {
        this.DOKill();

        if (_doColorTween != null)
            _doColorTween.Kill();

        if (_doFadeTween != null)
            _doFadeTween.Kill();
    }



    public LetterFiller createBasedFiller(GameObject prefab)
    {
        letterFiller = Instantiate(prefab, transform).GetComponent<LetterFiller>();
        letterFiller.setup(this);
        letterFiller.setColor(Color.white);
        letterFiller.setAlpha(1);
        letterFiller.setTotalProgress(1);


        return letterFiller;

    }


    private void OnDrawGizmos()
    {
        for (int i = 0; i < segmentCount; i++)
        {
            var seg = filler[i];
            if (seg != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(seg.startPoint, 0.2f);
                Gizmos.DrawSphere(seg.endPoint, 0.2f);
            }
        }
    }




}
