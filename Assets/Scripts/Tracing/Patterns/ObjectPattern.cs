using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;
public class ObjectPattern : Pattern
{
    [SerializeField] protected float spacing = .25f;
    [SerializeField] protected GameObject objectSource;
    [SerializeField] protected Color[] colors;

    protected List<CreatedObject> objects = new();


    [System.Serializable]
    protected class CreatedObject
    {
        public GameObject gameObject;
        public Transform transform => gameObject.transform;
        public SpriteRenderer renderer;
        public float delay;
        public int index;
    }


    protected virtual CreatedObject createObject()
    {
        var s = Instantiate(objectSource).GetComponent<SpriteRenderer>();
        s.gameObject.SetActive(true);
        s.transform.parent = transform;
        s.transform.position = transform.position + (Vector3)segment.path.startPoint;
        s.color = colors[objects.Count % colors.Length];
        CreatedObject c;
        objects.Add(c = new CreatedObject
        {
            gameObject = s.gameObject,
            renderer = s,
            delay = progress,
            index = objects.Count
        });
        moveObjectAlong(s.transform, (progress) * segment.totalLength);

        return c;
    }


    public override void whileAnimation()
    {
        base.whileAnimation();
        foreach (var x in objects)
        {
            moveObjectAlong(x.transform, (x.delay + progress) % 1 * segment.totalLength);
            // x.transform.position = transform.position.toVector2() + ((x.delay + progress).clamp01() * segment.totalLength);
        }
    }
    public override void whileTracing()
    {
        base.whileTracing();
        if ((movedDistance / spacing).floor() > objects.Count)
        {
            var obj = createObject();
            onObjectCreated(obj);
        }
    }


    protected virtual void onObjectCreated(CreatedObject obj)
    {
        var s = obj.transform.localScale;
        obj.transform.localScale = Vector3.zero;
        obj.transform.DOScale(s, .5f);
    }

}