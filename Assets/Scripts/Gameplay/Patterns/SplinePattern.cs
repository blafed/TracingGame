
using UnityEngine;
using UnityEngine.U2D;
public class SplinePattern : Pattern
{

    public float splineHeight = .5f;
    protected Spline spline => shapeController.spline;

    [SerializeField]
    protected SpriteShapeController shapeController;

    Path currentPath = new();
    Path targetPath;

    private void Awake()
    {
        shapeController.gameObject.SetActive(false);
    }

    private void Start()
    {
        shapeController.gameObject.SetActive(true);
        // SplinePathHelper.pathToSpline(targetPath, segment.);
    }


    // float moved

    private void Update()
    {
        targetPath = segment.path;
        var movedDistance = progress * segment.totalLength;
        targetPath.lerpFast(movedDistance, currentPath);
        SplinePathHelper.pathToSpline(currentPath, spline, factory);
        // if(movedDistance > currentPath.getSegmentLength(segmentIndex))
    }


    SplineControlPoint factory()
    {
        return new SplineControlPoint
        {
            height = splineHeight,
            corner = true,
            mode = ShapeTangentMode.Broken,
            cornerMode = Corner.Automatic,
        };
    }



}