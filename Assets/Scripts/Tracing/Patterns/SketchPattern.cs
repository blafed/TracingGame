using UnityEngine;

public class SketchPattern : SplinePattern
{
    // protected override bool createEdgePointsByDefault => false;



    [UnityEngine.Serialization.FormerlySerializedAs("edgePointPrefab")]
    [SerializeField] GameObject _edgePointPrefab;

    Transform[] roundEdges = new Transform[2];


    Transform startRoundEdge => roundEdges[0];
    Transform endRoundEdge => roundEdges[1];

    void createRoundEdges()
    {
        for (int j = 0; j < 2; j++)
        {
            var p = segment.path.startPoint;
            if (j == 1)
                p = segment.path.endPoint;

            var x = Instantiate(_edgePointPrefab, p, default);
            var edgePoint = x.transform;
            roundEdges[j] = edgePoint;
            x.transform.parent = transform.parent;
            if (j == 1)
                x.transform.position = transform.position.toVector2() + segment.path.endPoint;
            else
                x.transform.position = transform.position.toVector2() + segment.path.startPoint;
        }
    }

    public override void onCreated()
    {
        base.onCreated();
        if (isDot)
            return;
        for (int j = 0; j < 2; j++)
        {
            var p = segment.path.startPoint;
            if (j == 1)
                p = segment.path.endPoint;

            var x = Instantiate(_edgePointPrefab, p, default);
            var edgePoint = x.transform;
            roundEdges[j] = edgePoint;
            x.transform.parent = transform.parent;
            if (j == 1)
                x.transform.position = transform.position.toVector2() + segment.path.endPoint;
            else
                x.transform.position = transform.position.toVector2() + segment.path.startPoint;
        }

        // if (startEdgePoint)
        // startEdgePoint.gameObject.SetActive(false);
        // if (endEdgePoint)
        // endEdgePoint.gameObject.SetActive(false);

    }

    public override void onStartTracing()
    {
        base.onStartTracing();
    }





    public override void whileTracing()
    {
        moveSpline();
        if (endRoundEdge)
            moveObjectAlong(endRoundEdge.transform, movedDistance);
    }
    public override void onStartAnimation()
    {
        base.onStartAnimation();
        progress = 1;
    }
}