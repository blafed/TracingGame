using DG.Tweening;

using UnityEngine;
using UnityEngine.U2D;
public class SplinePattern : Pattern
{

    [SerializeField]
    protected float splineHeight = .5f;
    protected Spline spline => shapeController.spline;

    [SerializeField]
    public SpriteShapeController shapeController;
    [SerializeField]
    protected Transform followObject;
    [SerializeField]
    float dotSplineScale = 1;

    protected Path currentPath = new();
    protected override Path targetPath => _targetPath;


    protected override float pathScale => splineHeight;


    Path _targetPath;

    public override void onStartTracing()
    {
        base.onStartTracing();


        if (isDot)
        {
            var path = new Path();

            path.points[0] = path.points[1] = splineHeight * Vector2.up;
            path.points[3] = path.points[2] = -splineHeight * Vector2.up;
            shapeController.transform.localScale = 2f.vector();
            SplinePathHelper.pathToSpline(path, shapeController.spline, factory);

            transform.localScale = new Vector3();


            var renderer = this.shapeController.GetComponent<SpriteShapeRenderer>();
            var materials = renderer.materials;
            renderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;




            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = Resources.Load<Material>("Materials/SplineDot");
            }
            var mask = Instantiate(Resources.Load<GameObject>("Prefabs/SplinePatternDotMask"));
            mask.transform.parent = transform;
            mask.transform.position = transform.position;
            //SCALE
            var scale = dotRadius * Vector2.one;
            mask.transform.localScale = scale;
            renderer.transform.localScale = dotRadius * 2 * Vector2.one * dotSplineScale;
        }
    }

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
        if (isDot)
        {
            transform.localScale = EaseType2.QuadOut.evaluate(progress) * Vector3.one;
            return;
        }
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