using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;
namespace KidLetters
{
    public class EdgePointDealer : Singleton<EdgePointDealer>
    {
        List<EdgePoint> currentEdgePoints = new List<EdgePoint>();

        public float estimatedWaitTime { get; set; }


        public List<EdgePoint> spawnEdgePoints(GameObject prefab, Letter letter, Vector2? startPoint = null)
        {
            List<EdgePoint> result = new List<EdgePoint>(2 * letter.segmentCount);

            var filler = letter.filler;

            for (int i = 0; i < filler.segmentCount; i++)
            {
                var segment = filler[i];
                if (segment.isDot)
                    continue;
                for (int j = 0; j < 2; j++)
                {
                    var obj = Instantiate(prefab);
                    obj.transform.position = (j == 0 ? segment.startPoint : segment.endPoint);
                    var edgePoint = obj.GetComponent<EdgePoint>();
                    result.Add(edgePoint);
                    edgePoint.indexInSegment = j;
                    edgePoint.startupPunch(.1f * ((i * 2) + j));
                }
            }
            currentEdgePoints = result;
            estimatedWaitTime = .1f * result.Count + .4f;
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
    }
}