using UnityEngine;

public class HandTracing : MonoBehaviour
{
    public static HandTracing o;
    public float distanceThreshold = .5f;
    public float addingSpeed = 5;
    public float maxSpeed = 4;


    [SerializeField] float followLerpFactor = 5;
    [SerializeField] float followLerpMinDt = .02f;

    static InputManager im => InputManager.o;
    static TracingManager tracingManager => TracingManager.o;



    private void Awake()
    {
        enabled = false;
        o = this;
    }

    public void setEnabled(bool value)
    {
        enabled = value;
    }


    float addedDistance;
    float totalAddedDistance;
    float addedDistanceAt;

    public void reset()
    {
        addedDistance = addedDistanceAt = totalAddedDistance = 0;
    }

    void incorrectInput()
    {

    }


    public void onSegmentPatternChange(Pattern pattern)
    {
        if (pattern)
            totalAddedDistance = pattern.movedDistance;
    }
    private void Update()
    {
        var p = im.point;

        if (TracingManager.o.state == TracingState.tracing && !tracingManager.hasSegmentChanged)
        {

            var pattern = TracingManager.o.currentSegmentPattern;
            if (im.isEnter)
            {
                var currentPoint = pattern.getPoint(totalAddedDistance);
                var inPoint = im.point;
                var dst = Vector2.Distance(currentPoint, inPoint);
                var dir = pattern.getDirection(totalAddedDistance);
                var point2 = currentPoint + dir * distanceThreshold;


                // var r = Rect.MinMaxRect(currentPoint.x, currentPoint.y, point2.x, point2.y);

                if (Vector2.Distance(currentPoint, inPoint) > distanceThreshold || Vector2.Distance(inPoint, point2) > distanceThreshold)
                {
                    incorrectInput();
                }
                else
                {
                    var leftDistance = pattern.segment.totalLength - totalAddedDistance;
                    var diff = Mathf.Clamp(leftDistance + addingSpeed, 0, addingSpeed);
                    // addedDistanceAt = totalAddedDistance;
                    totalAddedDistance += diff * Time.deltaTime;
                }

            }


            // pattern.movedDistance = Mathf.MoveTowards(pattern.movedDistance, totalAddedDistance, Time.deltaTime * maxSpeed);
            pattern.movedDistance = Mathf.Lerp(pattern.movedDistance, totalAddedDistance, Time.deltaTime.max(followLerpMinDt) * followLerpFactor);

            // pattern.movedDistance = totalAddedDistance;

            // var increment = Mathf.Clamp(totalAddedDistance - pattern.movedDistance, 0, maxSpeed) * Time.deltaTime;
            // pattern.movedDistance += increment;
        }
    }
}