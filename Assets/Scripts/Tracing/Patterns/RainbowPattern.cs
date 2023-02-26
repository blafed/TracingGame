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

    protected override void Start()
    {
        base.Start();
        shineTimer = rate.random;
        followObject.gameObject.SetActive(false);
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (state == PatternState.tracing)
        {
            moveSpline();
        }
        else if (state == PatternState.animation)
        {
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
    }

    protected override void onStageChanged(PatternState old)
    {
        base.onStageChanged(old);
        switch (old)
        {
            case PatternState.animation:
                followObject.localScale = Vector3.one;
                followObject.DOScale(0, .25f);
                break;
        }
        switch (state)
        {
            case PatternState.animation:
                followObject.gameObject.SetActive(true);
                followObject.localScale = Vector3.zero;
                followObject.DOScale(1, .25f);
                break;
        }
    }

    void tweenShine(Transform shine)
    {
        shine.localScale = default;
        shine.DOScale(Vector3.one, animScaleDuration).SetEase(Ease.OutBack).OnComplete(() =>
        {

        }).SetLoops(-1, LoopType.Yoyo);
    }


}