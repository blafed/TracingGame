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




    internal static float BezierLength(IList<ShapeControlPoint> shapePoints, int splineDetail, ref float smallestSegment)
    {
        // Expand the Bezier.
        int controlPointContour = shapePoints.Count - 1;
        float spd = 0;
        float fmax = (float)(splineDetail - 1);
        for (int i = 0; i < controlPointContour; ++i)
        {
            int j = i + 1;
            ShapeControlPoint cp = shapePoints[i];
            ShapeControlPoint pp = shapePoints[j];

            Vector3 p0 = cp.position;
            Vector3 p1 = pp.position;
            Vector3 sp = p0;
            Vector3 rt = p0 + cp.rightTangent;
            Vector3 lt = p1 + pp.leftTangent;

            for (int n = 1; n < splineDetail; ++n)
            {
                float t = (float)n / fmax;
                Vector3 bp = BezierUtility.BezierPoint(rt, p0, p1, lt, t);
                float d = math.distance(bp, sp);
                spd += d;
                sp = bp;
            }
        }

        float ssc = fmax * controlPointContour;
        float ssl = spd / (ssc * 1.08f);
        smallestSegment = math.min(ssl, smallestSegment);
        return spd;
    }
}
