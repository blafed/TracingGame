using UnityEngine.Events;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TweenScript : MonoBehaviour
{
    // public enum RenableBehaviour
    // {
    //     Restart,
    //     Continue
    // }
    // public RenableBehaviour renableBehaviour;
    public List<TweenInfo> tweens = new List<TweenInfo>();
    List<Tween> createdTweens = new List<Tween>();





    private void OnEnable()
    {
        createdTweens.Capacity = tweens.Capacity;
        for (int i = 0; i < tweens.Count; i++)
        {
            var tweenInfo = this.tweens[i];
            var tween = create(tweenInfo, gameObject);
            createdTweens.Add(tween);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < createdTweens.Count; i++)
        {
            createdTweens[i].Kill();
        }
        createdTweens.Clear();
    }


    public static Tween create(TweenInfo i, GameObject defaultTarget)
    {
        var value = i.value;
        var duration = i.duration;
        var go = i.target ? i.target : defaultTarget;
        Tween tw = i.method switch
        {
            TweenMethod.move => go.transform.DOMove(value, duration),
            TweenMethod.moveLocal => go.transform.DOLocalMove(value, duration),
            TweenMethod.scale => go.transform.DOScale(value, duration),
            TweenMethod.punch => go.transform.DOPunchScale(value, duration),
            TweenMethod.rotate => go.transform.DORotate(value, duration),
            TweenMethod.rotateLocal => go.transform.DOLocalRotate(value, duration),
            _ => throw new System.Exception("Unsupported method " + i.method)
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
    rotate,
    rotateLocal

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
    public TweenScript[] startScripts;
}