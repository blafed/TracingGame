using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.U2D;
using UnityEngine;

public enum PatternState
{
    unknown,
    tracing,
    animation,
    done

}

static partial class Extensions
{
    public static bool isAnimation(this PatternState s) => s == PatternState.animation;
    public static bool isTracingDone(this PatternState s) => s.isAnimation() || s.isDone();
    public static bool isTracing(this PatternState s) => s == PatternState.tracing;
    public static bool isDone(this PatternState s) => s == PatternState.done;

}
public class Pattern : MonoBehaviour
{
    public PatternCode code;

    EdgePoint[] edgePoints = new EdgePoint[2];
    public EdgePoint startEdgePoint => edgePoints[0];
    public EdgePoint endEdgePoint => edgePoints[1];
    public LetterSegment segment { get; private set; }


    public bool isDone => state == PatternState.done;
    public bool isAnimation => state == PatternState.animation;
    public bool isTracing => state == PatternState.tracing;
    public bool isTracingDone => isDone || isAnimation;

    public bool isStateCompleted => progress >= 1;
    protected virtual bool createEdgePointsByDefault => true;








    public PatternState state
    {
        get => _state;
        set
        {

            var old = _state;
            _state = value;
            if (old != value)
                onStageChanged(old);
        }
    }

    PatternState _state = PatternState.unknown;


    float _movedDistance;

    /// <summary>
    /// progress from 0 to 1
    /// </summary>
    public float progress
    {
        get => (_movedDistance / pathLength).clamp01();
        set => _movedDistance = value.clamp01() * pathLength;
    }

    /// <summary>
    /// setup the pattern instance with required fields (call this after instantiate)
    /// </summary>
    /// <param name="segment"></param>
    public virtual void setup(LetterSegment segment)
    {
        this.segment = segment;
    }
    /// <summary>
    /// amount of unscaled distance
    /// </summary>
    public float movedDistance
    {
        get => _movedDistance;
        set => _movedDistance = value;
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


    protected virtual void Start()
    {
        if (createEdgePointsByDefault)
            createEdgePoints();
    }
    protected virtual void FixedUpdate()
    {

    }



    /// <summary>
    /// once stage get chagned, this will be called instantly with paramter of the old stage
    /// </summary>
    /// <param name="old"></param>
    protected virtual void onStageChanged(PatternState old)
    {
    }


    /// <summary>
    /// evaluate the point at absolute moved distancec
    /// </summary>
    /// <param name="movedDistance"></param>
    /// <returns></returns>
    protected Vector2 getPoint(float movedDistance)
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
    protected virtual Vector2 getDirection(float movedDistance)
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


    protected virtual GameObject edgePointPrefab => TracingManager.o.options.edgePointPrefab;

    protected void createEdgePoints(System.Action<EdgePoint> callback = null)
    {
        var prefab = edgePointPrefab;
        for (int j = 0; j < 2; j++)
        {
            var p = segment.path.startPoint;
            if (j == 1)
                p = segment.path.endPoint;

            var x = Instantiate(prefab, p, default);
            var edgePoint = x.GetComponent<EdgePoint>();
            edgePoint.pattern = this;
            edgePoints[j] = edgePoint;
            x.transform.parent = transform;
            if (j == 1)
            {
                x.transform.localPosition = segment.path.endPoint;
            }
            else
            {
                x.transform.localPosition = segment.path.startPoint;
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