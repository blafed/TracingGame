using DG.Tweening;
using UnityEngine;

public class ChainsPattern : SplinePattern
{
    [SerializeField]
    float hookRotation = 5;
    [SerializeField]
    float hookDuration = .7f;
    public override bool isFinished => base.isFinished && isHookAnimationDone;


    bool isHookAnimationDone;
    bool isHookAnimationStarted;



    protected override void Start()
    {
        base.Start();
        followObject.localScale = splineHeight.vector();
    }

    protected override void Update()
    {
        if (base.isFinished)
        {
            if (!isHookAnimationStarted)
                hookAnimate();
            isHookAnimationStarted = true;
        }
        else if (!isPostProgress)
        {
            base.Update();
            moveObjectAlong(followObject, movedDistance);
            followObject.localEulerAngles += Vector3.forward * hookRotation * Random.Range(-1, 1f);
        }
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