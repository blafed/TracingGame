using UnityEngine;

public class SketchPattern : SplinePattern
{
    protected override bool createEdgePointsByDefault => false;

    public override void whileTracing()
    {
        moveSpline();
    }
}