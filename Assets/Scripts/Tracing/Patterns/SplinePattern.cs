using DG.Tweening;

using UnityEngine;
using UnityEngine.U2D;
using KidLetters.Tracing;
public class SplinePattern : Pattern
{

    protected Spline spline => shapeController.spline;

    [SerializeField]
    public SpriteShapeController shapeController;
    [SerializeField]
    protected Transform followObject;
    [SerializeField]
    float dotSplineScale = 1;


    protected Path pathInstance = new();
    public float materialOffset = .5f;


    protected SpriteShapeRenderer spriteShapeRenderer;



    private void Awake()
    {
        spriteShapeRenderer = shapeController.GetComponent<SpriteShapeRenderer>();

    }
    protected override void onSetup()
    {
        base.onSetup();

        if (isDot)
        {
            initForDot();
        }
    }

    protected void moveSpline()
    {
        moveSpline(shapeController, movedDistance, pathInstance);
    }


    public override void onMoved()
    {
        base.onMoved();

        if (!isDot)
            moveSpline();
        else
        {
            var f = EaseType2.QuadOut.evaluate(progress);
            transform.localScale = f * Vector3.one;
        }
    }



    protected virtual void initForDot()
    {
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
        var path = new Path();

        path.points[0] = path.points[1] = Vector2.up;
        path.points[3] = path.points[2] = -Vector2.up;
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
        renderer.transform.localScale = dotRadius * 2 * Vector2.one * dotSplineScale * 2;
        renderer.transform.localPosition = Vector2.up * dotRadius;
    }

}