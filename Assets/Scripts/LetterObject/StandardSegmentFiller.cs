using UnityEngine.U2D;
using UnityEngine;

namespace KidLetters.LetterFillers
{
    public class StandardSegmentFiller : LetterSegmentFiller
    {

        SpriteShapeController shapeController;
        [SerializeField] GameObject roundEdgePoint;


        Path pathInstance = new Path();
        SpriteRenderer startEdgePoint;
        SpriteRenderer endEdgePoint;
        void Awake()
        {
            shapeController = GetComponentInChildren<SpriteShapeController>();
        }


        public override void onMoved()
        {
            if (!isDot)
                moveSpline(shapeController, movedDistance, pathInstance);
            else
                moveSpline(shapeController, 0, pathInstance);

            startEdgePoint.transform.position = getPoint(0);
            if (!isDot)
                endEdgePoint.transform.position = getPoint(movedDistance);

            startEdgePoint.transform.localScale = new Vector3(width, width, 1) * (isDot ? 2 : 1);
            if (!isDot)
                endEdgePoint.transform.localScale = new Vector3(width, width, 1);

        }


        protected override void onSetup()
        {
            base.onSetup();

            startEdgePoint = Instantiate(roundEdgePoint, transform).GetComponentInChildren<SpriteRenderer>();
            startEdgePoint.transform.localScale = new Vector3(width, width, 1) * (isDot ? 2 : 1);
            if (!isDot)
            {
                endEdgePoint = Instantiate(roundEdgePoint, transform).GetComponentInChildren<SpriteRenderer>();
                endEdgePoint.transform.localScale = new Vector3(width, width, 1);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(getPoint(0), width);
            Gizmos.DrawWireSphere(getPoint(movedDistance), width);
        }


        public override void setColor(Color color)
        {
            shapeController.spriteShapeRenderer.color = color;
            if (startEdgePoint)
                startEdgePoint.color = color;
            if (endEdgePoint)
                endEdgePoint.color = color;
        }

        public override void setAlpha(float alpha)
        {
            var color = shapeController.spriteShapeRenderer.color;
            color.a = alpha;
            shapeController.spriteShapeRenderer.color = color;
            if (startEdgePoint)
            {
                color = startEdgePoint.color;
                color.a = alpha < .5f ? 0 : alpha;
                startEdgePoint.color = color;
            }
            if (endEdgePoint)
            {
                color = endEdgePoint.color;
                color.a = alpha < .5f ? 0 : alpha;
                endEdgePoint.color = color;
            }
        }


    }
}