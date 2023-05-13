using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.U2D;
public static class BezierHelper
{

    public static Vector2 EvaluateQuadratic(Vector2 a, Vector2 b, Vector2 c, float t)
    {
        Vector2 p0 = Vector2.Lerp(a, b, t);
        Vector2 p1 = Vector2.Lerp(b, c, t);
        return Vector2.Lerp(p0, p1, t);
    }

    public static Vector2 EvaluateCubic(Vector2 a, Vector2 b, Vector2 c, Vector2 d, float t)
    {
        Vector2 p0 = EvaluateQuadratic(a, b, c, t);
        Vector2 p1 = EvaluateQuadratic(b, c, d, t);
        return Vector2.Lerp(p0, p1, t);
    }
    public static float GetLength(Vector2 o, Vector2 p1, Vector2 p2, Vector2 x)
    {
        var chord = (x - o).magnitude;
        var contNet =
        Vector2.Distance(o, p1) +
        Vector2.Distance(p1, p2) +
        Vector2.Distance(p2, x);
        return (chord + contNet) / 2;
    }

    public static float BezierSingleLength(Vector3[] points)
    {
        var p0 = points[0] - points[1];
        var p1 = points[2] - points[1];
        var p2 = new Vector3();
        var p3 = points[3] - points[2];

        var l0 = p0.magnitude;
        var l1 = p1.magnitude;
        var l3 = p3.magnitude;
        if (l0 > 0) p0 /= l0;
        if (l1 > 0) p1 /= l1;
        if (l3 > 0) p3 /= l3;

        p2 = -p1;
        var a = Mathf.Abs(Vector3.Dot(p0, p1)) + Mathf.Abs(Vector3.Dot(p2, p3));
        if (a > 1.98f || l0 + l1 + l3 < (4 - a) * 8) return l0 + l1 + l3;

        var bl = new Vector3[4];
        var br = new Vector3[4];

        bl[0] = points[0];
        bl[1] = (points[0] + points[1]) * 0.5f;

        var mid = (points[1] + points[2]) * 0.5f;

        bl[2] = (bl[1] + mid) * 0.5f;
        br[3] = points[3];
        br[2] = (points[2] + points[3]) * 0.5f;
        br[1] = (br[2] + mid) * 0.5f;
        br[0] = (br[1] + bl[2]) * 0.5f;
        bl[3] = br[0];

        return BezierSingleLength(bl) + BezierSingleLength(br);
    }

    public static Vector2 EvaluateCubicWithControls(Vector2 a, Vector2 b, Vector2 c, Vector2 d, float t
    , out Vector2 newB, out Vector2 newC
    )
    {
        Vector2 p0 = EvaluateQuadratic(a, b, c, t);
        Vector2 p1 = EvaluateQuadratic(b, c, d, t);
        newB = Vector2.Lerp(a, b, t);
        newC = p0;
        return Vector2.Lerp(p0, p1, t);
    }
}
