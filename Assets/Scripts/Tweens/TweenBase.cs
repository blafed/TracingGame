using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using DG.Tweening;

public abstract class TweenBase : MonoBehaviour
{
    public float duration = 1;
    public float delay = 0;
    public float afterDelay = 0;
    public Ease ease;
    public TweenBase startAfter;
    public UnityEngine.Events.UnityEvent onDone;

    public virtual bool isDone => tween.IsComplete();
    bool isStarted = false;
    Tween tween;

    private void OnEnable()
    {
        if (!isStarted)
            return;
        if (tween != null)
            tween.Rewind();
        tween = createBasicTween().SetDelay(delay).SetEase(ease);
    }

    private void OnDisable()
    {
        tween.Kill();
    }

    private void Start()
    {

        isStarted = true;
        OnEnable();
    }

    public abstract Tween createBasicTween();
}