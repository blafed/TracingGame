using UnityEngine;
using UnityEngine.U2D;
namespace KidLetters
{
    public class LetterSegmentFiller : MonoBehaviour
    {
        public float width = .5f;
        public LetterFiller letterFiller { get; private set; }
        public LetterSegment segment { get; private set; }
        protected virtual Path targetPath => segment.path;
        protected virtual float pathScale => 1;

        public float progress
        {
            get => (movedDistance / pathLength).clamp01();
            set => movedDistance = value.clamp01() * pathLength;
        }
        public float movedDistance
        {
            get => _movedDistance;
            set
            {
                _movedDistance = value;
                onMoved();
            }
        }

        float _movedDistance;

        public float pathLength => segment.isDot ? segment.dotRadius * 2 : segment.totalLength;
        public bool isProgressCompleted => progress >= 1;


        public void setup(LetterFiller letterFiller, LetterSegment segment)
        {
            this.letterFiller = letterFiller;
            this.segment = segment;
        }

        public Vector2 getPoint(float movedDistance)
        {
            movedDistance /= pathScale;
            return transform.position + targetPath.evaluate(movedDistance).toVector3() * pathScale;
        }
        protected void moveObjectAlong(Transform obj, float movedDistance)
        {
            obj.transform.position = getPoint(movedDistance);
            var dir = getDirection(movedDistance);
            obj.right = dir;
        }
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




        public void initSpline(float splineHeight, SpriteShapeController shapeController)
        {

            SplineControlPoint factory()
            {
                return new SplineControlPoint
                {
                    height = width,
                    corner = true,
                    mode = ShapeTangentMode.Continuous,
                    cornerMode = Corner.Automatic,
                };
            }
            var path = new Path();

            path.points[0] = path.points[1] = splineHeight * Vector2.up;
            path.points[3] = path.points[2] = -splineHeight * Vector2.up;
            shapeController.transform.localScale = 2f.vector();
            SplinePathHelper.pathToSpline(path, shapeController.spline, factory);
        }

        public void moveSpline(SpriteShapeController shapeContrller, float movedDistance, float splineHeight, Path pathInstance)
        {
            targetPath.lerpFast(movedDistance / splineHeight, pathInstance);
            SplinePathHelper.pathToSpline(pathInstance, shapeContrller.spline, splineControlPointFactory);
        }

        SplineControlPoint splineControlPointFactory()
        {
            return new SplineControlPoint
            {
                height = width,
                corner = true,
                mode = ShapeTangentMode.Continuous,
                cornerMode = Corner.Automatic,
            };
        }
        protected virtual void Start() { }
        protected virtual void Awake() { }

        protected virtual void onMoved()
        {
            // print("moved");
        }


        public virtual void setColor(Color color) { }
        public virtual void setAlpha(float alpha) { }



    }
}