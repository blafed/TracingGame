using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using System.Reflection;

namespace KidLetters.Tracing
{
    public static class SplinePathHelper
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
    }
}