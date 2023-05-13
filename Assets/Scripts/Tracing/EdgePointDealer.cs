using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;
namespace KidLetters
{
    public class EdgePointDealer : Singleton<EdgePointDealer>
    {
        List<EdgePoint> currentEdgePoints = new List<EdgePoint>();

        public float estimatedWaitTime { get; set; }


        public List<EdgePoint> spawnEdgePoints(GameObject prefab, LetterFiller letter, Vector2? startPoint = null)
        {
            List<EdgePoint> result = new List<EdgePoint>(2 * letter.segmentCount);

            var filler = letter;

            for (int i = 0; i < filler.segmentCount; i++)
            {
                var segment = filler.getSegment(i);
                if (segment.isDot)
                    continue;
                for (int j = 0; j < 2; j++)
                {
                    var obj = Instantiate(prefab);
                    obj.transform.position = (j == 0 ? segment.startPoint : segment.endPoint);
                    var edgePoint = obj.GetComponent<EdgePoint>();
                    edgePoint.isFirst = j == 0;
                    result.Add(edgePoint);
                    edgePoint.indexInSegment = j;
                    edgePoint.startupPunch(.2f * ((i * 2) + j));
                }
            }
            currentEdgePoints = result;
            estimatedWaitTime = .2f * result.Count + .4f;
            return result;
        }


        public void clearEdgePoints()
        {
            foreach (var item in currentEdgePoints)
            {
                Destroy(item.gameObject);
            }
            currentEdgePoints.Clear();
        }
        public void clearEdgePointsTween(float duration = .5f, float delay = .1f)
        {
            foreach (var item in currentEdgePoints)
            {
                item.collapse(duration).SetDelay(delay).OnComplete(() =>
                    Destroy(item.gameObject)

                );
            }
            currentEdgePoints.Clear();
        }

        public void onStartSegment(int segmentIndex)
        {
            for (int i = segmentIndex * 2; i <= segmentIndex * 2 + 1; i++)
            {
                if (i < currentEdgePoints.Count)
                    currentEdgePoints[i].onStartTracing();


            }
        }
        public void onEndSegment(int segmentIndex)
        {
            try
            {
                currentEdgePoints[segmentIndex * 2].onEndTracing();
                currentEdgePoints[segmentIndex * 2 + 1].onEndTracing();
            }
            catch { }
        }

        public void onWrongSegment(int segmentIndex)
        {
            for (int i = 0; i < currentEdgePoints.Count; i++)
            {
                currentEdgePoints[i].onWrongTracing(segmentIndex == i / 2);
            }
            // try
            // {
            //     for (int i = segmentIndex * 2; i <= segmentIndex * 2 + 1; i++)
            //         currentEdgePoints[i].onWrongTracing();

            // }
            // catch { }
        }
    }
}