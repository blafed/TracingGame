using DG.Tweening;
using UnityEngine;

public class ChainsPattern : SplinePattern
{
    [SerializeField]
    float hookRotation = 5;
    [SerializeField]
    float hookDuration = .7f;


    bool isHookAnimationDone;
    bool isHookAnimationStarted;



    public override void onCreated()
    {
        base.onCreated();
        followObject.localScale = splineHeight.vector();
    }

    public override void onStartTracing()
    {
        base.onStartTracing();
    }
    public override void onEndTracing()
    {
        base.onEndTracing();
        hookAnimate();
    }
    public override void whileTracing()
    {
        moveSpline();
        moveObjectAlong(followObject, movedDistance);
        followObject.localEulerAngles += Vector3.forward * hookRotation * Random.Range(-1, 1f);
    }

    void hookAnimate()
    {
        var rot = transform.localEulerAngles.z;
        followObject.DOLocalRotate(Vector3.forward * (rot + 90), hookDuration / 2).SetEase(Ease.OutBack)
        .OnComplete(() =>
            followObject.DOLocalRotate(Vector3.forward * (rot - 90), hookDuration / 2).SetEase(Ease.OutBack)
            .OnComplete(() => isHookAnimationDone = true)
        );
    }
}