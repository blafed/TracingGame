using UnityEngine;
using DG.Tweening;
public class RoadPattern : SplinePattern
{

    public override void onCreated()
    {
        base.onCreated();
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
    }


    public override void onStartAnimation()
    {
        base.onStartAnimation();
        followObject.gameObject.SetActive(true);
        followObject.localScale = Vector3.zero;
        followObject.DOScale(1, .25f);
    }
    public override void onEndAnimation()
    {
        base.onEndAnimation();
        followObject.localScale = Vector3.one;
        followObject.DOScale(0, .25f);
    }

}