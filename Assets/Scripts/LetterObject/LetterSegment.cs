using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
[DefaultExecutionOrder(0)]
public class LetterSegment : MonoBehaviour
{
    public Vector2 position
    {
        get => transform.position;
        set => transform.position = value;
    }
    public Letter letter => this.cachedComponentInParent(ref _letter);
    public Path path { get; private set; }



    public float totalLength { get; private set; }

    public bool isDot;
    public float dotRadius = .4f;


    Letter _letter;


    private void Awake()
    {
        var pathCreator = GetComponent<PathCreator>();
        if (pathCreator)
            path = pathCreator.path;
        else
        {
            IPathProvider pathProvider;
            if (TryGetComponent(out pathProvider))
            {
                path = pathProvider.path;
            }
            else
            {
                var shapeController = GetComponent<SpriteShapeController>();
                if (shapeController)
                {
                    path = new Path(transform.position);
                    SplinePathHelper.splineToPath(shapeController.spline, path);
                }
                else
                    Debug.LogError("Segment doesn't have path", this);
            }
        }
        totalLength = path.totalLength;


        path.center = default;

    }


    private void OnDrawGizmos()
    {
        if (isDot)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, dotRadius);
        }
    }
}