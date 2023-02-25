using System.Collections.Generic;
using UnityEngine.U2D;
using UnityEngine;

public class Pattern : MonoBehaviour
{
    public PatternCode code;

    // public EdgePoint startEdgePoint { get; set; }
    // public EdgePoint endEdgePoint { get; set; }

    public LetterSegment segment { get; set; }
    /// <summary>
    /// ranges from 0 to 1 usually, can be > 1
    /// </summary>
    public float progress { get; set; }
    public bool isPostProgress { get; set; }
    /// <summary>
    /// the absolute moved distance
    /// </summary>
    protected float movedDistance => progress.clamp01() * segment.totalLength;

    protected virtual Path targetPath => segment.path;
    protected virtual float pathScale => 1;
    public virtual bool isFinished => progress >= 1;


    public virtual void onPostProgressStart() { }
    public virtual void onPostProgressEnd() { }


    // protected virtual void OnDestroy()
    // {
    //     Destroy(startEdgePoint.gameObject);
    //     Destroy(endEdgePoint.gameObject);
    // }
    protected Vector2 getPoint(float movedDistance)
    {
        movedDistance /= pathScale;
        return transform.position + targetPath.evaluate(movedDistance).toVector3() * pathScale;
    }
    protected void moveObjectAlong(Transform obj, float movedDistance)
    {
        obj.transform.position = getPoint(movedDistance);
        // transform.position + currentPath.evaluate(movedDistance / splineHeight).toVector3() * splineHeight;
        //(Vector2)transform.position + (currentPath.endPoint * splineHeight);
        var dir = getDirectionOfPoint(movedDistance);
        //-targetPath.simpleNormal(movedDistance, .01f).getNormal().normalized;
        obj.right = dir;
    }
    protected virtual Vector2 getDirectionOfPoint(float movedDistance)
    {
        // movedDistance /= pathScale;
        var a = getPoint(movedDistance);
        var t = movedDistance * 1.01f;
        bool isInverse = false;
        if (t > segment.totalLength)
        {
            t = movedDistance / 1.01f;
            isInverse = true;
        }
        var b = getPoint(t);
        var d = b - a;
        if (isInverse)
            d = a - b;
        return d.normalized;
    }



    protected List<EdgePoint> createEdgePoints(LetterSegment segment, GameObject prefab)
    {
        List<EdgePoint> edges = new List<EdgePoint>();
        for (int j = 0; j < 2; j++)
        {
            var p = segment.path.startPoint;
            if (j == 1)
                p = segment.path.endPoint;

            var x = Instantiate(prefab, p, default);
            var edge = x.GetComponent<EdgePoint>();
            edges.Add(edge);
            if (j == 1)
            {
                x.transform.position = segment.path.endPoint;
            }
            else
            {
                x.transform.position = segment.path.startPoint;
            }
            x.transform.parent = segment.transform;
        }
        return edges;
    }




    // public virtual void initSegment(LetterSegment segment)
    // {
    //     this.segment = segment;
    //     transform.parent = segment.transform;
    //     transform.localPosition = default;
    //     for (int j = 0; j < 2; j++)
    //     {
    //         var p = segment.path.startPoint;
    //         if (j == 1)
    //             p = segment.path.endPoint;

    //         var x = Instantiate(edgePointPrefab, p, default);
    //         var edge = x.GetComponent<EdgePoint>();
    //         if (j == 1)
    //         {
    //             endEdgePoint = edge;
    //             x.transform.position = segment.path.endPoint;
    //         }
    //         else
    //         {
    //             startEdgePoint = edge;
    //             x.transform.position = segment.path.startPoint;
    //         }
    //         x.transform.parent = segment.transform;
    //     }
    // }

    // public virtual void setTrail(LetterSegment segment, float t)
    // {

    // }

}


public enum PatternCode
{
    none,
    chains,
    road,
    rainbow,
    butterfly,
    candy,
    sketch,
    COUNT,
}