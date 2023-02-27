using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.U2D;
using UnityEngine;

// public enum PatternState
// {
//     unknown,
//     tracing,
//     animation,
//     done

// }

// static partial class Extensions
// {
//     public static bool isAnimation(this PatternState s) => s == PatternState.animation;
//     public static bool isTracingDone(this PatternState s) => s.isAnimation() || s.isDone();
//     public static bool isTracing(this PatternState s) => s == PatternState.tracing;
//     public static bool isDone(this PatternState s) => s == PatternState.done;

// }
public class Pattern : MonoBehaviour
{
    public PatternCode code;

    EdgePoint[] edgePoints = new EdgePoint[2];
    public EdgePoint startEdgePoint => edgePoints[0];
    public EdgePoint endEdgePoint => edgePoints[1];
    public LetterSegment segment { get; private set; }


    public virtual float waitBeforeEnableTracing => 1;
    public virtual float unitedTime => 0;


    public bool isProgressCompleted => progress >= 1;
    protected virtual bool createEdgePointsByDefault => true;


    protected virtual float addedLength => 0;


    public virtual void onCreated()
    {
        if (createEdgePointsByDefault)
            createEdgePoints();
    }
    public virtual void onStartTracing()
    {
        foreach (var x in edgePoints)
            x.setPlaying();
    }
    public virtual void whileTracing()
    {
        foreach (var x in edgePoints)
            x.rotateByDistance();
    }
    public virtual void onEndTracing()
    {
        foreach (var x in edgePoints)
        {
            x.rotateToOrigin();
            x.setStopped();
        }
    }
    public virtual void onStartAnimation()
    {
        foreach (var x in edgePoints)
            x.setPlaying();
    }
    public virtual void whileAnimation()
    {
    }
    public virtual void onEndAnimation()
    {

    }
    /// <summary>
    /// called after animation is done
    /// </summary>
    public virtual void onDone()
    {
        foreach (var x in edgePoints)
            x.setCompleted();
    }
    /// <summary>
    /// called after all segments are completed
    /// </summary>
    public virtual void onAllDone() { }
    public virtual void onStartUnited() { }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="time"></param>
    /// <returns>true to break the united loop, false to keep running</returns>
    public virtual void whileUnited(float time) { }
    public virtual void onEndUnited() { }


    protected virtual void OnDestroy()
    {
        foreach (var x in edgePoints)
            if (x)
                Destroy(x.gameObject);
    }




    float _movedDistance;

    /// <summary>
    /// progress from 0 to 1
    /// </summary>
    public float progress
    {
        get => (movedDistance / pathLength).clamp01();
        set => movedDistance = value.clamp01() * pathLength;
    }

    /// <summary>
    /// setup the pattern instance with required fields (call this after instantiate)
    /// </summary>
    /// <param name="segment"></param>
    public void setup(LetterSegment segment)
    {
        this.segment = segment;
    }
    /// <summary>
    /// amount of unscaled distance
    /// </summary>
    public float movedDistance
    {
        get => _movedDistance + addedLength;
        set => _movedDistance = Mathf.Max(value - addedLength, addedLength);
    }
    /// <summary>
    /// the target path that pattern should follow (scaled)
    /// </summary>
    protected virtual Path targetPath => segment.path;
    /// <summary>
    /// how much does the targetPath scale
    /// </summary>
    protected virtual float pathScale => 1;
    /// <summary>
    /// path absolute length
    /// </summary>
    protected float pathLength => segment.totalLength;


    /// <summary>
    /// evaluate the point at absolute moved distancec
    /// </summary>
    /// <param name="movedDistance"></param>
    /// <returns></returns>
    public Vector2 getPoint(float movedDistance)
    {
        movedDistance /= pathScale;
        return transform.position + targetPath.evaluate(movedDistance).toVector3() * pathScale;
    }
    /// <summary>
    /// move the object and the same time make it takes the same direction of path
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="movedDistance"></param>
    protected void moveObjectAlong(Transform obj, float movedDistance)
    {
        obj.transform.position = getPoint(movedDistance);
        var dir = getDirection(movedDistance);
        obj.right = dir;
    }
    /// <summary>
    /// gets the direction at moved distance
    /// </summary>
    /// <param name="movedDistance"></param>
    /// <returns></returns>
    public virtual Vector2 getDirection(float movedDistance)
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



    protected void createEdgePoints(System.Action<EdgePoint> callback = null)
    {
        var prefab = TracingManager.o.options.edgePointPrefab.gameObject;
        for (int j = 0; j < 2; j++)
        {
            var p = segment.path.startPoint;
            if (j == 1)
                p = segment.path.endPoint;

            var x = Instantiate(prefab, p, default);
            var edgePoint = x.GetComponent<EdgePoint>();
            edgePoint.pattern = this;
            edgePoints[j] = edgePoint;
            x.transform.parent = transform.parent;
            if (j == 1)
            {
                x.transform.position = transform.position.toVector2() + segment.path.endPoint;
            }
            else
            {
                x.transform.position = transform.position.toVector2() + segment.path.startPoint;
            }

            callback?.Invoke(edgePoint);
        }
    }


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