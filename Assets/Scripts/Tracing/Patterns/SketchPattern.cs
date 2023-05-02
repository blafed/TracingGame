using UnityEngine;

public class SketchPattern : SplinePattern
{
    // protected override bool createEdgePointsByDefault => false;



    [UnityEngine.Serialization.FormerlySerializedAs("edgePointPrefab")]
    [SerializeField] GameObject _edgePointPrefab;

    Transform[] roundEdges = new Transform[2];


    Transform startRoundEdge => roundEdges[0];
    Transform endRoundEdge => roundEdges[1];

    protected override void onSetup()
    {
        base.onSetup();

        if (isDot)
            return;
        for (int j = 0; j < 2; j++)
        {
            var p = startPoint;
            if (j == 1)
                p = endPoint;

            var x = Instantiate(_edgePointPrefab, p, default);
            var edgePoint = x.transform;
            roundEdges[j] = edgePoint;
            x.transform.parent = transform.parent;
            if (j == 1)
                x.transform.position = endPoint;
            else
                x.transform.position = startPoint;
        }
    }

    void createRoundEdges()
    {
        for (int j = 0; j < 2; j++)
        {
            var p = startPoint;
            if (j == 1)
                p = endPoint;

            var x = Instantiate(_edgePointPrefab, p, default);
            var edgePoint = x.transform;
            roundEdges[j] = edgePoint;
            x.transform.parent = transform.parent;
            if (j == 1)
                x.transform.position = transform.position.toVector2() + endPoint;
            else
                x.transform.position = transform.position.toVector2() + startPoint;
        }
    }

    public override void onMoved()
    {
        base.onMoved();

        if (isDot)
            return;

        startRoundEdge.transform.position = getPoint(0);
        endRoundEdge.transform.position = getPoint(movedDistance);
        foreach (var edge in roundEdges)
        {
            if (edge)
                edge.localScale = new Vector3(width, width, 1);
        }
    }

}