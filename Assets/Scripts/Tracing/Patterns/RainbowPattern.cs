using UnityEngine;
using UnityEngine.U2D;
using DG.Tweening;
public class RainbowPattern : SplinePattern
{
    [SerializeField] float spacing = 1;
    [SerializeField] MinMaxF rate = new MinMaxF(.5f, 1);
    [SerializeField] GameObject[] shines;
    [Header("Animation")]
    [SerializeField] float animScaleDuration = .5f;
    // [SerializeField]
    // float anim



    float shineTimer;
    GameObject tempFollowObject;

    public override void onCreated()
    {
        base.onCreated();
        shineTimer = rate.random;
        followObject.gameObject.SetActive(false);
    }

    public override void whileTracing()
    {
        base.whileTracing();
        moveSpline();
    }
    public override void whileAnimation()
    {
        base.whileAnimation();
        moveObjectAlong(followObject, movedDistance);
        shineTimer -= Time.fixedDeltaTime;
        if (shineTimer <= 0)
        {
            var s = Instantiate(shines.getRandom(), getPoint(movedDistance), default);
            s.SetActive(true);
            var normal = currentPath.simpleNormal(movedDistance);
            var r = Random.Range(-1f, 1).signOrZero();
            // s.transform.position += normal.toVector3() * r * splineHeight * spacing;
            s.transform.parent = transform;
            shineTimer = rate.random;
            tweenShine(s.transform);
        }
    }


    public override void onEndAnimation()
    {
        base.onEndAnimation();
        followObject.localScale = Vector3.one;
        followObject.DOScale(0, .25f);
    }
    public override void onStartTracing()
    {
        base.onStartTracing();
        followObject.gameObject.SetActive(true);
        followObject.localScale = Vector3.zero;
        followObject.DOScale(1, .25f);
    }

    void tweenShine(Transform shine)
    {
        shine.localScale = default;
        shine.DOScale(Vector3.one, animScaleDuration).SetEase(Ease.OutBack).OnComplete(() =>
        {

        }).SetLoops(-1, LoopType.Yoyo);
    }


}