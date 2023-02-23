#pragma warning disable 0618
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Path
{

    [System.Obsolete("supposed to be used internally")]
    public Vector2 center;
    [SerializeField]
    public List<Vector2> points;
    [SerializeField]
    bool isClosed;
    [SerializeField]
    bool autoSetControlPoints;

    public Path() : this(Vector2.zero) { }
    public Path(Vector2 center)
    {
        this.center = center;
        points = new List<Vector2>
        {
            Vector2.left,
            (Vector2.left+Vector2.up)*.5f,
             (Vector2.right+Vector2.down)*.5f,
             Vector2.right
        };
    }


    public Path clone()
    {
        return new Path
        {
            points = new List<Vector2>(points),
            isClosed = isClosed,
            autoSetControlPoints = autoSetControlPoints,
            center = center,
        };
    }


    public Vector2 startPoint => points[0] + center;
    public Vector2 endPoint => points[points.Count - 1] + center;

    public Vector2 simpleNormal(float tLen, float diff = .02f)
    {
        var a = evaluate(tLen);
        var b = evaluate(tLen * (1 + diff));
        var d = b - a;
        return d.getNormal().normalized;

    }

    public Vector2 evaluateNormalized(float t) => evaluate(t * totalLength);
    public Vector2 evaluate(float tLen)
    {
        tLen = Mathf.Max(tLen, 0);

        int seg = 0;
        float len = 0;
        float est = 0;


        for (int i = 0; i < NumSegments; i++)
        {
            seg = i;
            est += len;
            len = getSegmentLength(i);
            var total = len + est;

            if (tLen >= est && tLen <= total)
                break;
        }
        var delta = len;
        var r = (tLen - est) / delta;

        var j = seg * 3;

        return center + BezierHelper.EvaluateCubic(points[j], points[j + 1], points[j + 2], points[j + 3], r);

    }
    public void lerpFast(float moved, Path other)
    {
        moved = Mathf.Max(moved, 0);

        int seg = 0;
        float len = 0;
        float est = 0;


        for (int i = 0; i < NumSegments; i++)
        {
            seg = i;
            est += len;
            len = getSegmentLength(i);
            var total = len + est;

            if (moved >= est && moved <= total)
                break;
        }
        var delta = len;
        var r = (moved - est) / delta;

        var j = seg * 3;

        var p = other;
        p.center = this.center;

        var totalCount = j + 4;

        if (p.points.Count > totalCount)
        {
            try
            {
                p.points.RemoveRange(totalCount, p.points.Count - totalCount);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"{totalCount} {p.points.Count} {p.points.Count - totalCount}");
                throw e;
            }
        }
        p.points.Capacity = totalCount;



        for (int i = 0; i < totalCount; i++)
        {
            if (i >= p.points.Count)
                p.points.Add(points[i]);
            else
                p.points[i] = points[i];
        }

        Vector2 newB, newC;
        p.points[p.points.Count - 1] = BezierHelper.EvaluateCubicWithControls(points[j], points[j + 1], points[j + 2], points[j + 3], r, out newB, out newC);
        p.points[p.points.Count - 3] = newB;
        p.points[p.points.Count - 2] = newC;
    }
    [System.Obsolete("slow, use the other")]
    public Path lerp(float moved)
    {
        moved = Mathf.Max(moved, 0);

        int seg = 0;
        float len = 0;
        float est = 0;


        for (int i = 0; i < NumSegments; i++)
        {
            seg = i;
            est += len;
            len = getSegmentLength(i);
            var total = len + est;

            if (moved >= est && moved <= total)
                break;
        }
        var delta = len;
        var r = (moved - est) / delta;

        var j = seg * 3;

        Path p = new Path();
        p.center = this.center;
        p.points.Clear();
        p.points.Capacity = j + 4;

        for (int i = 0; i < j + 4; i++)
        {
            p.points.Add(points[i]);
        }


        Vector2 newB, newC;
        p.points[p.points.Count - 1] = BezierHelper.EvaluateCubicWithControls(points[j], points[j + 1], points[j + 2], points[j + 3], r, out newB, out newC);
        p.points[p.points.Count - 3] = newB;
        p.points[p.points.Count - 2] = newC;
        return p;

    }



    public float totalLength
    {
        get
        {
            var f = 0f;
            for (int i = 0; i < NumSegments; i++)
            {
                f += getSegmentLength(i);
            }
            return f;
        }
    }
    public float getSegmentLength(int segmentIndex)
    {
        var i = segmentIndex * 3;
        return BezierHelper.GetLength(
            points[i],
            points[i + 1],
            points[i + 2],
            points[i + 3]
        );
    }

    public Vector2 this[int i]
    {
        get
        {
            return center + points[i];
        }
    }

    public bool IsClosed
    {
        get
        {
            return isClosed;
        }
        set
        {
            if (isClosed != value)
            {
                isClosed = value;

                if (isClosed)
                {
                    points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
                    points.Add(points[0] * 2 - points[1]);
                    if (autoSetControlPoints)
                    {
                        AutoSetAnchorControlPoints(0);
                        AutoSetAnchorControlPoints(points.Count - 3);
                    }
                }
                else
                {
                    points.RemoveRange(points.Count - 2, 2);
                    if (autoSetControlPoints)
                    {
                        AutoSetStartAndEndControls();
                    }
                }
            }
        }
    }

    public bool AutoSetControlPoints
    {
        get
        {
            return autoSetControlPoints;
        }
        set
        {
            if (autoSetControlPoints != value)
            {
                autoSetControlPoints = value;
                if (autoSetControlPoints)
                {
                    AutoSetAllControlPoints();
                }
            }
        }
    }

    public int NumPoints
    {
        get
        {
            return points.Count;
        }
    }

    public int NumSegments
    {
        get
        {
            return points.Count / 3;
        }
    }

    public void AddSegment(Vector2 anchorPos)
    {
        anchorPos -= center;
        points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
        points.Add((points[points.Count - 1] + anchorPos) * .5f);
        points.Add(anchorPos);

        if (autoSetControlPoints)
        {
            AutoSetAllAffectedControlPoints(points.Count - 1);
        }
    }

    public void SplitSegment(Vector2 anchorPos, int segmentIndex)
    {
        anchorPos -= center;
        points.InsertRange(segmentIndex * 3 + 2, new Vector2[] { Vector2.zero, anchorPos, Vector2.zero });
        if (autoSetControlPoints)
        {
            AutoSetAllAffectedControlPoints(segmentIndex * 3 + 3);
        }
        else
        {
            AutoSetAnchorControlPoints(segmentIndex * 3 + 3);
        }
    }

    public void DeleteSegment(int anchorIndex)
    {
        if (NumSegments > 2 || !isClosed && NumSegments > 1)
        {
            if (anchorIndex == 0)
            {
                if (isClosed)
                {
                    points[points.Count - 1] = points[2];
                }
                points.RemoveRange(0, 3);
            }
            else if (anchorIndex == points.Count - 1 && !isClosed)
            {
                points.RemoveRange(anchorIndex - 2, 3);
            }
            else
            {
                points.RemoveRange(anchorIndex - 1, 3);
            }
        }
    }

    public Vector2[] GetPointsInSegment(int i)
    {
        return new Vector2[] { center + points[i * 3],
        center + points[i * 3 + 1],
          center +  points[i * 3 + 2],
          center + points[LoopIndex(i * 3 + 3)] };
    }

    public void MovePoint(int i, Vector2 pos)
    {
        pos -= center;
        Vector2 deltaMove = pos - points[i];

        if (i % 3 == 0 || !autoSetControlPoints)
        {
            points[i] = pos;

            if (autoSetControlPoints)
            {
                AutoSetAllAffectedControlPoints(i);
            }
            else
            {

                if (i % 3 == 0)
                {
                    if (i + 1 < points.Count || isClosed)
                    {
                        points[LoopIndex(i + 1)] += deltaMove;
                    }
                    if (i - 1 >= 0 || isClosed)
                    {
                        points[LoopIndex(i - 1)] += deltaMove;
                    }
                }
                else
                {
                    bool nextPointIsAnchor = (i + 1) % 3 == 0;
                    int correspondingControlIndex = (nextPointIsAnchor) ? i + 2 : i - 2;
                    int anchorIndex = (nextPointIsAnchor) ? i + 1 : i - 1;

                    if (correspondingControlIndex >= 0 && correspondingControlIndex < points.Count || isClosed)
                    {
                        float dst = (points[LoopIndex(anchorIndex)] - points[LoopIndex(correspondingControlIndex)]).magnitude;
                        Vector2 dir = (points[LoopIndex(anchorIndex)] - pos).normalized;
                        points[LoopIndex(correspondingControlIndex)] = points[LoopIndex(anchorIndex)] + dir * dst;
                    }
                }
            }
        }
    }

    public Vector2[] CalculateEvenlySpacedPoints(float spacing, float resolution = 1)
    {
        List<Vector2> evenlySpacedPoints = new List<Vector2>();
        evenlySpacedPoints.Add(points[0]);
        Vector2 previousPoint = points[0];
        float dstSinceLastEvenPoint = 0;

        for (int segmentIndex = 0; segmentIndex < NumSegments; segmentIndex++)
        {
            Vector2[] p = GetPointsInSegment(segmentIndex);
            float controlNetLength = Vector2.Distance(p[0], p[1]) + Vector2.Distance(p[1], p[2]) + Vector2.Distance(p[2], p[3]);
            float estimatedCurveLength = Vector2.Distance(p[0], p[3]) + controlNetLength / 2f;
            int divisions = Mathf.CeilToInt(estimatedCurveLength * resolution * 10);
            float t = 0;
            while (t <= 1)
            {
                t += 1f / divisions;
                Vector2 pointOnCurve = BezierHelper.EvaluateCubic(p[0], p[1], p[2], p[3], t);
                dstSinceLastEvenPoint += Vector2.Distance(previousPoint, pointOnCurve);

                while (dstSinceLastEvenPoint >= spacing)
                {
                    float overshootDst = dstSinceLastEvenPoint - spacing;
                    Vector2 newEvenlySpacedPoint = pointOnCurve + (previousPoint - pointOnCurve).normalized * overshootDst;
                    newEvenlySpacedPoint += center;
                    evenlySpacedPoints.Add(newEvenlySpacedPoint);
                    dstSinceLastEvenPoint = overshootDst;
                    previousPoint = newEvenlySpacedPoint;
                }

                previousPoint = pointOnCurve;
            }
        }

        return evenlySpacedPoints.ToArray();
    }


    void AutoSetAllAffectedControlPoints(int updatedAnchorIndex)
    {
        for (int i = updatedAnchorIndex - 3; i <= updatedAnchorIndex + 3; i += 3)
        {
            if (i >= 0 && i < points.Count || isClosed)
            {
                AutoSetAnchorControlPoints(LoopIndex(i));
            }
        }

        AutoSetStartAndEndControls();
    }

    void AutoSetAllControlPoints()
    {
        for (int i = 0; i < points.Count; i += 3)
        {
            AutoSetAnchorControlPoints(i);
        }

        AutoSetStartAndEndControls();
    }

    void AutoSetAnchorControlPoints(int anchorIndex)
    {
        Vector2 anchorPos = points[anchorIndex];
        Vector2 dir = Vector2.zero;
        float[] neighbourDistances = new float[2];

        if (anchorIndex - 3 >= 0 || isClosed)
        {
            Vector2 offset = points[LoopIndex(anchorIndex - 3)] - anchorPos;
            dir += offset.normalized;
            neighbourDistances[0] = offset.magnitude;
        }
        if (anchorIndex + 3 >= 0 || isClosed)
        {
            Vector2 offset = points[LoopIndex(anchorIndex + 3)] - anchorPos;
            dir -= offset.normalized;
            neighbourDistances[1] = -offset.magnitude;
        }

        dir.Normalize();

        for (int i = 0; i < 2; i++)
        {
            int controlIndex = anchorIndex + i * 2 - 1;
            if (controlIndex >= 0 && controlIndex < points.Count || isClosed)
            {
                points[LoopIndex(controlIndex)] = anchorPos + dir * neighbourDistances[i] * .5f;
            }
        }
    }

    void AutoSetStartAndEndControls()
    {
        if (!isClosed)
        {
            points[1] = (points[0] + points[2]) * .5f;
            points[points.Count - 2] = (points[points.Count - 1] + points[points.Count - 3]) * .5f;
        }
    }

    int LoopIndex(int i)
    {
        return (i + points.Count) % points.Count;
    }

}