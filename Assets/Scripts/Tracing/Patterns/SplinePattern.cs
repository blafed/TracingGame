
using UnityEngine;
using UnityEngine.U2D;
public class SplinePattern : Pattern
{

    [SerializeField]
    protected float splineHeight = .5f;
    protected Spline spline => shapeController.spline;

    [SerializeField]
    protected SpriteShapeController shapeController;
    [SerializeField]
    protected Transform followObject;

    protected Path currentPath = new();
    protected override Path targetPath => _targetPath;


    protected override float pathScale => splineHeight;


    Path _targetPath;


    public override void onCreated()
    {
        base.onCreated();
        shapeController.transform.localScale = Vector3.one * splineHeight;
        _targetPath = segment.path.clone();

        shapeController.splineDetail = 64;


        for (int i = 0; i < targetPath.points.Count; i++)
        {
            targetPath.points[i] /= splineHeight;
        }
    }


    protected void moveSpline()
    {
        moveSpline(this.shapeController);
    }
    protected void moveSpline(SpriteShapeController shapeController)
    {
        targetPath.lerpFast(movedDistance / splineHeight, currentPath);
        SplinePathHelper.pathToSpline(currentPath, shapeController.spline, factory);
    }


    SplineControlPoint factory()
    {
        return new SplineControlPoint
        {
            height = 1,
            corner = true,
            mode = ShapeTangentMode.Continuous,
            cornerMode = Corner.Automatic,
        };
    }

}