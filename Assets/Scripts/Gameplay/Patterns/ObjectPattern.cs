using UnityEngine;

public class ObjectPattern : Pattern
{
    public Transform obj;


    private void Start()
    {
        obj = transform.GetChild(0);
    }
    private void Update()
    {
        obj.position = segment.path.evaluate(progress * segment.totalLength);
    }
}