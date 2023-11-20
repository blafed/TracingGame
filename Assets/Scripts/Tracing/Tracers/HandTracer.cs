using System.Collections.Generic;
using UnityEngine;


namespace KidLetters.Tracing
{

    public class HandTracer : TracerBase
    {

        [SerializeField] float distanceThreshold = .5f;
        [SerializeField] float dotBoundingRadius = .5f;
        [SerializeField] float addingSpeed = 5;
        [SerializeField] float maxSpeed = 4;


        [SerializeField] float followLerpFactor = 5;
        [SerializeField] float followLerpMinDt = .02f;
        [SerializeField] float plottingDistance = .5f;

        static InputManager im => InputManager.o;


        protected override void Awake()
        {
            base.Awake();
            enabled = false;
        }
        public void setEnabled(bool value)
        {
            enabled = value;
        }


        float addedDistance;
        float totalAddedDistance;
        float addedDistanceAt;
        bool isDotPlotted;
        // float movedDistance;



        List<Vector2> plottedPoints = new List<Vector2>();

        public override void flush()
        {
            totalAddedDistance = 0;
            isDotPlotted = false;
        }


        public override float getNewMovement(LetterSegmentFiller segment, float dt)
        {
            isWrongTracing = false;
            var p = im.point;






            if (segment.isDot)
            {
                var boundingOnDot = new BoundingSphere(segment.transform.position, dotBoundingRadius);

                if (im.isEnter && boundingOnDot.contains(im.point))
                {
                    isDotPlotted = true;
                }
                else
                {
                    isWrongTracing = true;
                }


                if (isDotPlotted)
                {
                    return segment.movedDistance + maxSpeed * Time.deltaTime;
                }
                return segment.movedDistance;
            }
            else
            if (im.isEnter)
            {
                //the current point where the segment filler ends
                var currentPoint = segment.getPoint(totalAddedDistance);
                //input point
                var inPoint = im.point;

                //direction where the tracer should go
                var dir = segment.getDirection(totalAddedDistance);
                //the goal point where should the tracer go
                var goalPoint = currentPoint + dir * distanceThreshold;

                var dstToCurrentPoint = Vector2.Distance(currentPoint, inPoint);
                var dstToGoalPoint = Vector2.Distance(goalPoint, inPoint);

                // var r = Rect.MinMaxRect(currentPoint.x, currentPoint.y, point2.x, point2.y);

                if (dstToCurrentPoint > distanceThreshold || dstToGoalPoint > distanceThreshold)
                {
                    if (dstToGoalPoint > distanceThreshold * 1.5f)
                    {
                        isWrongTracing = true;
                    }
                }
                else
                {
                    var leftDistance = segment.pathLength - totalAddedDistance;
                    var diff = Mathf.Clamp(leftDistance + addingSpeed, 0, addingSpeed);
                    // addedDistanceAt = totalAddedDistance;
                    totalAddedDistance += diff * Time.deltaTime;
                }

            }


            return Mathf.Lerp(segment.movedDistance, totalAddedDistance, Time.deltaTime.max(followLerpMinDt) * followLerpFactor);

        }

    }
}