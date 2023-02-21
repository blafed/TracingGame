using System.Collections.Generic;
using UnityEngine.U2D;
using UnityEngine;

public class Pattern : MonoBehaviour
{


    // public EdgePoint startEdgePoint { get; set; }
    // public EdgePoint endEdgePoint { get; set; }



    public LetterSegment segment { get; set; }
    public float progress { get; set; }



    // protected virtual void OnDestroy()
    // {
    //     Destroy(startEdgePoint.gameObject);
    //     Destroy(endEdgePoint.gameObject);
    // }


    protected List<EdgePoint> createEdgePoints(LetterSegment segment, GameObject prefab)
    {
        List<EdgePoint> edges = new List<EdgePoint>();
        for (int j = 0; j < 2; j++)
        {
            var p = segment.path.startPoint;
            if (j == 1)
                p = segment.path.endPoint;

            var x = Instantiate(prefab, p, default);
            var edge = x.GetComponent<EdgePoint>();
            edges.Add(edge);
            if (j == 1)
            {
                x.transform.position = segment.path.endPoint;
            }
            else
            {
                x.transform.position = segment.path.startPoint;
            }
            x.transform.parent = segment.transform;
        }
        return edges;
    }




    // public virtual void initSegment(LetterSegment segment)
    // {
    //     this.segment = segment;
    //     transform.parent = segment.transform;
    //     transform.localPosition = default;
    //     for (int j = 0; j < 2; j++)
    //     {
    //         var p = segment.path.startPoint;
    //         if (j == 1)
    //             p = segment.path.endPoint;

    //         var x = Instantiate(edgePointPrefab, p, default);
    //         var edge = x.GetComponent<EdgePoint>();
    //         if (j == 1)
    //         {
    //             endEdgePoint = edge;
    //             x.transform.position = segment.path.endPoint;
    //         }
    //         else
    //         {
    //             startEdgePoint = edge;
    //             x.transform.position = segment.path.startPoint;
    //         }
    //         x.transform.parent = segment.transform;
    //     }
    // }

    // public virtual void setTrail(LetterSegment segment, float t)
    // {

    // }

}