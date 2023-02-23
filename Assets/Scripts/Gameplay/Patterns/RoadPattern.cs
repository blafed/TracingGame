using UnityEngine;
using DG.Tweening;
public class RoadPattern : SplinePattern
{
    protected override void Start()
    {
        base.Start();
        followObject.gameObject.SetActive(false);
    }
    protected override void Update()
    {
        if (!isPostProgress)
            base.Update();
        else
        {
            followObject.gameObject.SetActive(true);
            moveObjectAlong(followObject, movedDistance);
        }
    }

    public override void onPostProgressStart()
    {
        followObject.gameObject.SetActive(true);
        followObject.localScale = Vector3.zero;
        followObject.DOScale(1, .25f);
    }
    public override void onPostProgressEnd()
    {
        followObject.localScale = Vector3.one;
        followObject.DOScale(0, .25f);
    }
}