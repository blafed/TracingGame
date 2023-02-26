using UnityEngine;

public class SketchPattern : SplinePattern
{
    protected override bool createEdgePointsByDefault => false;
    protected override void FixedUpdate()
    {
        if (isTracing)
        {
            moveSpline();
        }
    }
}