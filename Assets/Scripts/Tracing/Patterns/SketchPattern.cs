using UnityEngine;


public class SketchPattern : SplinePattern
{
    [SerializeField] AudioSource tracingEndAudio;
    [SerializeField] GameObject tracingEndEffect;
    // protected override bool createEdgePointsByDefault => false;
    public bool disableRoundEdges;


    [UnityEngine.Serialization.FormerlySerializedAs("edgePointPrefab")]
    [SerializeField] GameObject _edgePointPrefab;

    Transform[] roundEdges = new Transform[2];


    Transform startRoundEdge => roundEdges[0];
    Transform endRoundEdge => roundEdges[1];






    public override bool useAnimation => false;
    public override bool useUnitedAnimation => false;

    protected override void onSetup()
    {
        base.onSetup();

        if (isDot || disableRoundEdges)
            return;
        for (int j = 0; j < 2; j++)
        {
            var p = startPoint;
            if (j == 1)
                p = endPoint;

            var x = Instantiate(_edgePointPrefab, p, default);
            var edgePoint = x.transform;
            roundEdges[j] = edgePoint;
            x.transform.parent = transform;
            if (j == 1)
                x.transform.position = endPoint;
            else
                x.transform.position = startPoint;
        }
    }
    public override void onMoved()
    {
        base.onMoved();

        if (isDot || disableRoundEdges)
            return;

        startRoundEdge.transform.position = getPoint(0);
        endRoundEdge.transform.position = getPoint(movedDistance);
        foreach (var edge in roundEdges)
        {
            if (edge)
                edge.localScale = new Vector3(width, width, 1);
        }
    }

    public override void onStartAnimation()
    {
        newMovedDistance = pathLength;
    }

    public override bool whileUnited(float time)
    {
        return true;
    }


    public override void onEndTracing()
    {
        base.onEndTracing();

        if (tracingEndEffect)
        {
            tracingEndEffect.transform.position = endPoint;
            tracingEndEffect.SetActive(true);
        }
        if (tracingEndAudio)
            tracingEndAudio.Play();
    }

}