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
        bool isInit;
        protected override void Awake()
        {
            shapeController = GetComponentInChildren<SpriteShapeController>();



        }
        protected override void onMoved()
        {
            init();
            if (!segment.isDot)
                moveSpline(shapeController, movedDistance, width, pathInstance);
            else
                moveSpline(shapeController, 0, 0, pathInstance);

            startEdgePoint.transform.position = getPoint(0);
            if (!segment.isDot)
                endEdgePoint.transform.position = getPoint(movedDistance);
        }

        void init()
        {
            if (isInit)
                return;

            isInit = true;

            startEdgePoint = Instantiate(roundEdgePoint, transform.parent).GetComponentInChildren<SpriteRenderer>();
            startEdgePoint.transform.localScale = new Vector3(width, width, 1) * (segment.isDot ? 2 : 1);
            if (!segment.isDot)
            {
                endEdgePoint = Instantiate(roundEdgePoint, transform.parent).GetComponentInChildren<SpriteRenderer>();
                endEdgePoint.transform.localScale = new Vector3(width, width, 1);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(getPoint(0), width);
            Gizmos.DrawWireSphere(getPoint(movedDistance), width);
        }
    }
}