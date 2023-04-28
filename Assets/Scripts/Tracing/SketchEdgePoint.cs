using DG.Tweening;
using UnityEngine;

public class SketchEdgePoint : EdgePoint
{
    public override void setCompleted()
    {
    }
    public override void setStopped()
    {

    }
    public override void setPlaying()
    {

    }

    public override Tween spawnFromPoint(Vector2 point)
    {
        transform.localScale = Vector3.one;
        var targetPoint = transform.position;
        return transform.DOMove(targetPoint, 0).SetEase(Ease.OutBack);
    }
}