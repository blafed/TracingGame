using DG.Tweening;
using UnityEngine;

public class ChainsPattern : SplinePattern
{
    [SerializeField] float unitedTime = 2f;
    [SerializeField] float unitedMotionSpeed = 3;
    [Space]
    [SerializeField] float hookLength = .25f;
    [SerializeField] CosWave hookWaving = new CosWave { amplitude = 2, frequency = 5 };
    [SerializeField]
    float hookDuration = .7f;
    [SerializeField]
    [Space]
    float materialOffsetFactor = 2;



    // public override float unitedTime => _unitedTime;



    bool didHookAppearAnimation;
    bool didHookFetchAnimation;

    float animationRunningTime = 0;



    void addMaterialOffset(float amount)
    {
        foreach (var mat in spriteShapeRenderer.materials)
        {
            var offset = mat.mainTextureOffset;
            offset.x += amount;
            mat.mainTextureOffset = offset;
        }
    }
    void setMaterialOffset(float amount)
    {
        foreach (var mat in spriteShapeRenderer.materials)
        {
            var offset = mat.mainTextureOffset;
            offset.x = amount;
            mat.mainTextureOffset = offset;
        }
    }


    public override void onCreated()
    {
        base.onCreated();
        followObject.GetChild(0).localScale = width.vector();
    }

    public override void onStartTracing()
    {
        base.onStartTracing();

        if (isDot)
        {
            followObject.gameObject.SetActive(false);
        }


        // movedDistance = Mathf.Max(_hookLength, movedDistance);
    }
    public override void whileTracing(float movedDistance)
    {
        base.whileTracing(movedDistance);
        if (!isDot)
        {
            setMaterialOffset(movedDistance * materialOffsetFactor);
        }
    }



    public override void onMoved()
    {

        if (isDot) base.onMoved();
        else
        {
            if (!didHookAppearAnimation)
            {
                followObject.transform.localScale = Vector3.zero;
            }
            if (movedDistance < hookLength && movedDistance > hookLength / 4f)
            {
                if (!didHookAppearAnimation)
                {
                    didHookAppearAnimation = true;
                    hookAppearAnimation();
                }
            }
            else
            {
                var endOfSpline = movedDistance - hookLength;
                moveSpline(shapeController, endOfSpline, pathInstance);
                moveObjectAlong(followObject, endOfSpline);

                if (Time.time - animationRunningTime > hookDuration && !didHookFetchAnimation)
                    followObject.localEulerAngles += Vector3.forward * hookWaving.calculate(movedDistance);


                if (movedDistance >= pathLength)
                {
                    if (!didHookFetchAnimation)
                    {
                        didHookFetchAnimation = true;
                        hookFetchAnimation();
                    }
                }
            }
        }
    }

    protected override void initForDot()
    {
        var d = Instantiate(followObject, transform);
        d.transform.localScale = Vector3.one * dotRadius;
        d.transform.localEulerAngles = Vector3.forward * 90;
        shapeController.gameObject.SetActive(false);

    }


    public override void whileAnimation(float movedDistance)
    {
        base.onStartAnimation();
        newMovedDistance = pathLength;
    }



    public override bool whileUnited(float time)
    {
        var p = time / unitedTime;

        p = p.clamp01();
        if (!isDot)
        {
            addMaterialOffset(unitedMotionSpeed * Time.fixedDeltaTime);
        }
        else
            transform.localEulerAngles = new Vector3(0, 0, (360) * p);


        return p >= 1;
    }

    void hookAppearAnimation()
    {
        animationRunningTime = Time.time;
        var dir = getDirection(hookLength);
        var targetRotation = Vector3.forward * Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        followObject.DOLocalRotate(targetRotation, hookDuration).SetEase(Ease.InQuad);
        followObject.DOScale(1, hookDuration / 2f).SetEase(Ease.OutBack);
    }

    void hookFetchAnimation()
    {
        var dir = getDirection(pathLength - hookLength);
        var targetRotation = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        DOTween.Sequence().Append(

            followObject.DOLocalRotate(Vector3.forward * (targetRotation - 90), hookDuration / 2).SetEase(Ease.OutBack)
        )
        .Append(
        followObject.DOLocalRotate(Vector3.forward * (targetRotation), hookDuration / 2).SetEase(Ease.OutBack)
        );
    }
}