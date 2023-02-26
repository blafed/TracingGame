using UnityEngine;
using DG.Tweening;
public class RoadPattern : SplinePattern
{
    protected override void Start()
    {
        base.Start();
        followObject.gameObject.SetActive(false);
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (isTracing)
            moveSpline();
        if (isAnimation)
        {
            followObject.gameObject.SetActive(true);
            moveObjectAlong(followObject, movedDistance);
        }
    }


    protected override void onStageChanged(PatternState old)
    {
        base.onStageChanged(old);
        if (state.isAnimation())
        {
            followObject.gameObject.SetActive(true);
            followObject.localScale = Vector3.zero;
            followObject.DOScale(1, .25f);
        }
        else if (state.isDone())
        {
            followObject.localScale = Vector3.one;
            followObject.DOScale(0, .25f);
        }
        else
        {
            followObject.gameObject.SetActive(false);
        }
    }
}