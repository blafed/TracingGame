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
