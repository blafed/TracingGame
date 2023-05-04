using UnityEngine;
using DG.Tweening;
public class RoadPattern : SplinePattern
{

    public override void onCreated()
    {
        base.onCreated();
        followObject.gameObject.SetActive(false);

    }
    public override void whileTracing(float movedDistance)
    {
        base.whileTracing(movedDistance);
        moveSpline();

    }
    public override void whileAnimation(float movedDistance)
    {
        base.whileAnimation(movedDistance);
        moveObjectAlong(followObject, movedDistance);
    }


    public override void onStartAnimation()
    {
        base.onStartAnimation();
        if (isDot)
        {
            followObject.gameObject.SetActive(false);
            progress = 1;
            return;
        }
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