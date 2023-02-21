using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour, IPathProvider
{

    Path IPathProvider.path => this.path;
    public Path path;

    public Color anchorCol = Color.red;
    public Color controlCol = Color.white;
    public Color segmentCol = Color.green;
    public Color selectedSegmentCol = Color.yellow;
    public float anchorDiameter = .1f;
    public float controlDiameter = .075f;
    public bool displayControlPoints = true;

    [Header("Debug")]
    [SerializeField] float totalLength;
    [SerializeField] float debugPointLength = .5f;
    [Range(0, 1)]
    [SerializeField] float debugPointEv = .5f;
    [Space]
    [SerializeField] PathCreator lerpFrom;
    [Range(0, 1)]
    [SerializeField] float lerpAmount = .5f;

    public void CreatePath()
    {
        path = new Path(transform.position);
    }

    void Reset()
    {
        CreatePath();
    }

    private void Update()
    {
        path.center = transform.position;
    }

    private void OnDrawGizmos()
    {
        path.center = transform.position;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(path.evaluate(debugPointEv * path.totalLength), .15f);


        totalLength = path.totalLength;
        if (lerpFrom)
        {
            lerpFrom.path.lerpFast(lerpAmount * lerpFrom.path.totalLength, path);
            // path = lerpFrom.path.lerp(lerpAmount * lerpFrom.path.totalLength);
        }
    }
}
