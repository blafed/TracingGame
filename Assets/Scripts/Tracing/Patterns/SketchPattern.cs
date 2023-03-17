using UnityEngine;

public class SketchPattern : SplinePattern
{
    // protected override bool createEdgePointsByDefault => false;

    public override void onCreated()
    {
        base.onCreated();

        if (startEdgePoint)
            startEdgePoint.gameObject.SetActive(false);
        if (endEdgePoint)
            endEdgePoint.gameObject.SetActive(false);
    }

    public override void whileTracing()
    {
        moveSpline();
    }
    public override void onStartAnimation()
    {
        base.onStartAnimation();
        progress = 1;
    }
}