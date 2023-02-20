using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
public class LetterSegment : MonoBehaviour
{
    public static void splineToPath(Spline spline, Path path)
    {
        path.points = new List<Vector2>();
        for (int i = 0; i < spline.GetPointCount(); i++)
        {
            var point = spline.GetPosition(i);
            var rightTangent = spline.GetRightTangent(i);

            path.points.Add(point);
            if (i != spline.GetPointCount() - 1)
            {
                var leftTangent = spline.GetLeftTangent(i + 1);
                var next = spline.GetPosition(i + 1);
                path.points.Add(rightTangent + point);
                path.points.Add(leftTangent + next);
            }
        }
    }
    public static void pathToSpline(Path path, Spline spline, System.Func<SplineControlPoint> factory = null)
    {
        var field = typeof(Spline).GetField("m_ControlPoints", BindingFlags.Instance | BindingFlags.NonPublic);
        var list = new List<SplineControlPoint>(path.NumSegments);
        if (factory == null)
            factory = () => new SplineControlPoint
            {
                height = .1f,
                corner = true,
                cornerMode = Corner.Automatic,
                mode = ShapeTangentMode.Broken

            };


        for (int i = 0; i < path.NumSegments + 1; i++)
        {
            list.Add(factory());
        }

        for (int i = 0; i < path.NumSegments; i++)
        {

            var j = i * 3;

            SplineControlPoint point = list[i];

            point.position = path.points[j];
            point.rightTangent = (Vector3)path.points[j + 1] - point.position;
            SplineControlPoint next = list[i + 1];
            next.position = path.points[j + 3];
            next.leftTangent = (Vector3)path.points[j + 2] - next.position;

        }
        field.SetValue(spline, list);
    }
    public Path path;
    public Spline spline => shapeController.spline;
    Spline mirrorSpline => mirrorShape.spline;


    public float mirrorTime = 2;
    public SpriteShapeController mirrorShape;
    SpriteShapeController shapeController;

    public List<LetterSegmentAnchor> anchors = new();


    public float debugPointEv;
    public float totalLength;
    public List<float> anchorLengthes = new List<float>();
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        totalLength = path.totalLength;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(path.evaluate(debugPointEv * path.totalLength), .15f);

        // var f = 

        // Gizmos.DrawWireSphere(evaluate(debugPointEv) + (Vector2)transform.position, .1f);
    }

    void calculateLength()
    {

    }


    private void Awake()
    {
        shapeController = GetComponent<SpriteShapeController>();
    }
    private void OnEnable()
    {
        // print(spline.GetPointCount());
        // path.center = transform.position;
        // pathToSpline(path, spline);

        path = new(transform.position);
        gameObject.AddComponent<PathCreator>().path = path;
        splineToPath(spline, path);
    }


    IEnumerator startMirror()
    {

        var t = 0f;
        int anchorIndex = 0;
        while (t < mirrorTime)
        {

            var p = t / mirrorTime;
            var nextAnchorIndex = anchorIndex + 1;

            if (nextAnchorIndex >= spline.GetPointCount())
            {
                break;
            }
            var startPosition = spline.GetPosition(anchorIndex);
            var endPosition = spline.GetPosition(nextAnchorIndex);
            var point = Vector2.Lerp(startPosition, endPosition, p);
            mirrorSpline.SetPosition(anchorIndex, point);
            mirrorSpline.SetLeftTangent(anchorIndex, spline.GetLeftTangent(anchorIndex));
            mirrorSpline.SetRightTangent(anchorIndex, spline.GetRightTangent(anchorIndex));


            if (p >= 1)
            {
                t = 0;
                anchorIndex++;
            }


            t += Time.deltaTime;
            yield return null;
        }
    }



}

[System.Serializable]
public class LetterSegmentAnchor
{
    public Vector2 position;
    public Vector2 leftTangent, rightTangent;
    public float upLength;
}