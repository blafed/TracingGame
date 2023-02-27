using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;
public class ObjectPattern : Pattern
{
    [SerializeField] float _unitedTime = 2;
    [SerializeField] protected float unitedSpeed = 2;
    [SerializeField] protected float spacing = .25f;
    [SerializeField] protected GameObject objectSource;
    [SerializeField] protected Color[] colors;

    protected List<CreatedObject> objects = new();


    public override float unitedTime => _unitedTime;


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
            delay = (movedDistance / spacing).floor() * spacing,
            index = objects.Count
        });
        moveObjectAlong(s.transform, movedDistance);

        return c;
    }


    public override void onStartAnimation()
    {
        base.onStartAnimation();
        progress = 1;
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

    protected void moveAllObjectsAlong(float distance)
    {
        foreach (var x in objects)
        {
            moveObjectAlong(x.transform, (x.delay + distance) % pathLength);
        }
    }

    public override void whileUnited(float time)
    {
        base.whileUnited(time);
        moveAllObjectsAlong(unitedSpeed * time);
    }


    protected virtual void onObjectCreated(CreatedObject obj)
    {
        var s = obj.transform.localScale;
        obj.transform.localScale = Vector3.zero;
        obj.transform.DOScale(s, .5f);
    }

}