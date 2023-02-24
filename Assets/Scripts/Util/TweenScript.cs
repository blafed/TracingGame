using UnityEngine.Events;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TweenScript : MonoBehaviour
{
    public List<TweenInfo> tweens = new List<TweenInfo>();


    Tween create(TweenInfo i)
    {
        var value = i.value;
        var duration = i.duration;
        var go = i.target ? i.target : gameObject;
        Tween tw = i.method switch
        {
            TweenMethod.move => go.transform.DOMove(value, duration),
            TweenMethod.moveLocal => go.transform.DOLocalMove(value, duration),
            TweenMethod.scale => go.transform.DOScale(value, duration),
            TweenMethod.punch => go.transform.DOPunchScale(value, duration),

        };
        tw.SetDelay(i.delay).SetEase(i.ease).SetLoops(i.loops, i.loopType);

        return tw;
    }
}

public enum TweenMethod
{
    none,
    move,
    moveLocal,
    scale,
    punch,

}

[System.Serializable]
public class TweenInfo
{
    public enum Type
    {
        tween,
        invokeScripts,
        delay,
    }


    public Type type;

    [System.NonSerialized]
    public Object cachedTarget;
    public TweenMethod method;
    public GameObject target;

    public Ease ease;
    public float duration = 1;
    public float delay;

    public Vector3 value;
    [Header("loop")]
    public LoopType loopType;
    [Min(-1)]
    public int loops = 1;

    [Header("After")]
    public string sendMessage;
    public UnityEvent finish;

    [Header("other")]
    public TweenScript[] startScripts;
}