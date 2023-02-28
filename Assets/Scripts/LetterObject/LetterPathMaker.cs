
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
[DefaultExecutionOrder(1)]
public class LetterPathMaker : MonoBehaviour, IPathProvider
{
    public enum Type
    {
        strightLine,
        circle,
    }

    public Type type;
    [Min(0)]
    public float height = 1;
    [Header("circle")]
    [Min(0)]
    public float diameter = 1;
    [Range(0, 1)]
    public float absense = 0;
    [Header("Debug")]
    [SerializeField] Path _path;
    [SerializeField] bool _manualPath;



    public bool isChildOfMaker => transform.parent && transform.parent.GetComponent<LetterPathMaker>();

    private void Awake()
    {

    }

    public Path generate()
    {

        Path path = new Path();

        if (!_manualPath)
            _path = path;
        if (transform.childCount > 0)
        {
            path.points.Clear();
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i).GetComponent<LetterPathMaker>();
                if (!child)
                    continue;
                if (path.points.Count > 0)
                    path.points.RemoveAt(path.points.Count - 1);
                var childPath = child.generate();
                path.points.Capacity += childPath.points.Count;
                for (int j = 0; j < childPath.points.Count; j++)
                {
                    var pp = transform.TransformPoint(childPath.points[j]) + child.transform.localPosition;
                    path.points.Add(pp);

                }

            }
            return path;
        }
        else
            switch (type)
            {
                case Type.strightLine:
                    path.points = new List<Vector2>(){
                Vector2.zero,
                Vector2.zero,
                Vector2.right * height,
                Vector2.right * height,
            };
                    break;
                case Type.circle:


                    const float k = 0.5522f;

                    path.points = new System.Collections.Generic.List<Vector2>
                {
                    new Vector2 (-1, 0),
                    new Vector2 (-1, k),
                    new Vector2 (-k, 1),
                    new Vector2 (0, 1),
                    new Vector2 (k, 1),
                    new Vector2 (1, k),
                    new Vector2 (1, 0),
                    new Vector2 (1, -k),
                    new Vector2 (k, -1),
                    new Vector2 (0, -1),
                    new Vector2 (-k, -1),
                    new Vector2 (-1,-k),
                    new Vector2 (-1, 0),
                };

                    var tl = path.totalLength;
                    path = path.lerp(path.totalLength - absense * tl);


                    for (int i = 0; i < path.points.Count; i++)
                    {
                        path.points[i] *= diameter / 2;
                    }

                    break;

            }

        //apply rotation and scale
        for (int i = 0; i < path.points.Count; i++)
        {
            path.points[i] = transform.TransformPoint(path.points[i]) - transform.position;
        }

        return path;
    }


    public Path path => generate();

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (isChildOfMaker)
            return;
        // if (transform.parent)
        //     return;
        // var m = false;
        // foreach (Transform x in transform)
        //     if (Selection.Contains(x.gameObject))
        //         m = true;
        // if (Selection.Contains(gameObject) || m)
        Draw();
    }

    void Draw()
    {
        var path = generate();
        path.center = transform.position;

        for (int i = 0; i < path.NumSegments; i++)
        {
            Vector2[] points = path.GetPointsInSegment(i);
            // Handles.color = Color.black;
            // Handles.DrawLine(points[1], points[0]);
            // Handles.DrawLine(points[2], points[3]);
            Handles.DrawBezier(points[0], points[3], points[1], points[2], Color.cyan, null, 10);
        }
        path.center = default;

    }
#endif
}