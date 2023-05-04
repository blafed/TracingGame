using UnityEngine;


namespace KidLetters.Tracing
{

    public class HandTracer : TracerBase
    {

        [SerializeField] float distanceThreshold = .5f;
        [SerializeField] float addingSpeed = 5;
        [SerializeField] float maxSpeed = 4;


        [SerializeField] float followLerpFactor = 5;
        [SerializeField] float followLerpMinDt = .02f;

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
        float movedDistance;


        void incorrectInput()
        {

        }



        public override void flush()
        {
            totalAddedDistance = 0;
            isDotPlotted = false;
        }

        public override float getNewMovement(LetterSegmentFiller segment, float dt)
        {
            var p = im.point;


            if (segment.isDot)
            {
                var boundingOnDot = new BoundingSphere(segment.transform.position, segment.dotRadius);

                if (im.isEnter && boundingOnDot.contains(im.point))
                {
                    isDotPlotted = true;
                }


                if (isDotPlotted)
                {
                    movedDistance += maxSpeed * Time.deltaTime;
                }
            }
            else
            if (im.isEnter)
            {
                var currentPoint = segment.getPoint(totalAddedDistance);
                var inPoint = im.point;
                var dst = Vector2.Distance(currentPoint, inPoint);
                var dir = segment.getDirection(totalAddedDistance);
                var point2 = currentPoint + dir * distanceThreshold;


                // var r = Rect.MinMaxRect(currentPoint.x, currentPoint.y, point2.x, point2.y);

                if (Vector2.Distance(currentPoint, inPoint) > distanceThreshold || Vector2.Distance(inPoint, point2) > distanceThreshold)
                {
                    incorrectInput();
                }
                else
                {
                    var leftDistance = segment.pathLength - totalAddedDistance;
                    var diff = Mathf.Clamp(leftDistance + addingSpeed, 0, addingSpeed);
                    // addedDistanceAt = totalAddedDistance;
                    totalAddedDistance += diff * Time.deltaTime;
                }

            }


            movedDistance = Mathf.Lerp(segment.movedDistance, totalAddedDistance, Time.deltaTime.max(followLerpMinDt) * followLerpFactor);


            return movedDistance;
        }

    }
}