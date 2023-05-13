using UnityEngine;
using UnityEngine.U2D;
using KidLetters.Tracing;
namespace KidLetters
{
    public class LetterSegmentFiller : MonoBehaviour
    {
        public virtual float width => letterFiller.width;
        public LetterFiller letterFiller { get; private set; }
        // LetterRawSegment segment { get; set; }


        public bool isDot { get; private set; }
        public Path targetPath { get; private set; }


        public Vector2 startPoint => getPoint(0);
        public Vector2 endPoint => getPoint(pathLength);
        public Vector2 currentPoint => getPoint(movedDistance);

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
                _movedDistance = Mathf.Clamp(value, 0, pathLength);
                onMoved();
            }
        }
        public virtual float addedLength => 0;


        public float dotRadius => LetterObjectConfig.o.dotRadius;

        public float pathLength => isDot ? dotRadius * 2 : _pathLength;
        public bool isProgressCompleted => progress >= 1;


        float _pathLength;
        float _movedDistance;


        public void setup(LetterFiller letterFiller, GlyphSegment segment)
        {
            this.letterFiller = letterFiller;
            this.isDot = segment.isDot;
            this.targetPath = segment.path;
            _pathLength = segment.pathLegnth;
            onSetup();
        }

        protected virtual void onSetup()
        {

        }

        public Vector2 getPoint(float movedDistance)
        {
            return transform.position + targetPath.evaluate(movedDistance).toVector3();
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
            if (t > pathLength)
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

            shapeController.splineDetail = 64;
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
            // shapeController.transform.localScale = 2f.vector();
            SplinePathHelper.pathToSpline(path, shapeController.spline, factory);
        }

        public void moveSpline(SpriteShapeController shapeContrller, float movedDistance, Path pathInstance)
        {
            targetPath.lerpFast(movedDistance, pathInstance);
            SplinePathHelper.pathToSpline(pathInstance, shapeContrller.spline, splineControlPointFactory);
            // moveSplineAdvanced(shapeContrller, 0, movedDistance, splineHeight, pathInstance);
        }
        public void moveSplineAdvanced(SpriteShapeController shapeContrller, float startAtDistance, float endAtDistance, Path pathInstance)
        {
            targetPath.lerpFast(movedDistance, pathInstance);
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

        public virtual void onMoved()
        {
        }

        public virtual void onWidthChanged()
        {
            onMoved();
        }


        public virtual void setColor(Color color) { }
        public virtual void setAlpha(float alpha) { }



    }
}