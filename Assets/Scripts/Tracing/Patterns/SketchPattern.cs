using UnityEngine;

public class SketchPattern : SplinePattern
{
    // protected override bool createEdgePointsByDefault => false;



    [UnityEngine.Serialization.FormerlySerializedAs("edgePointPrefab")]
    [SerializeField] GameObject _edgePointPrefab;


    protected override GameObject edgePointPrefab => _edgePointPrefab;

    public override void onCreated()
    {
        base.onCreated();



        if (startEdgePoint)
            startEdgePoint.gameObject.SetActive(false);
        if (endEdgePoint)
            endEdgePoint.gameObject.SetActive(false);


        // if (startEdgePoint)
        // startEdgePoint.gameObject.SetActive(false);
        // if (endEdgePoint)
        // endEdgePoint.gameObject.SetActive(false);

    }

    public override void onStartTracing()
    {
        base.onStartTracing();
        if (startEdgePoint)
            startEdgePoint.gameObject.SetActive(true);
        if (endEdgePoint)
            endEdgePoint.gameObject.SetActive(true);
    }





    public override void whileTracing()
    {
        moveSpline();
        moveObjectAlong(endEdgePoint.transform, movedDistance);
    }
    public override void onStartAnimation()
    {
        base.onStartAnimation();
        progress = 1;
    }
}