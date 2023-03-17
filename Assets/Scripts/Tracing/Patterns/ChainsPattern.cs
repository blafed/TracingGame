using DG.Tweening;
using UnityEngine;

public class ChainsPattern : SplinePattern
{
    [SerializeField] float _hookLength = .9f;
    [SerializeField] float _unitedTime = 1.5f;
    [SerializeField] CosWave hookWaving = new CosWave { amplitude = 2, frequency = 5 };
    [SerializeField]
    float hookRotation = 5;
    [SerializeField]
    float hookDuration = .7f;


    bool isHookAnimationDone;
    bool isHookAnimationStarted;

    public override float unitedTime => _unitedTime;
    protected override float addedLength => isDot ? 0 : _hookLength;



    public override void onCreated()
    {
        base.onCreated();
        followObject.localScale = splineHeight.vector();
    }

    public override void onStartTracing()
    {
        base.onStartTracing();

        if (isDot)
        {
            followObject.gameObject.SetActive(false);
        }
    }
    public override void onEndTracing()
    {
        base.onEndTracing();
        hookAnimate();
    }
    public override void whileTracing()
    {
        base.whileTracing();
        moveSpline();
        moveObjectAlong(followObject, movedDistance);
        followObject.localEulerAngles += Vector3.forward * hookWaving.calculate(movedDistance);
    }


    public override void onStartAnimation()
    {
        base.onStartAnimation();
        progress = 1;
    }
    // public override void whileUnited(float speed)
    // {

    // }

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