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
    [SerializeField]
    float dotScale = 1;

    protected List<CreatedObject> objects = new();


    [System.Serializable]
    public class CreatedObject
    {
        public GameObject gameObject;
        public Transform transform => gameObject.transform;
        public SpriteRenderer renderer;
        public float delay;
        public int index;


        public float randomParameter;
    }


    public virtual CreatedObject createObject()
    {
        var s = Instantiate(objectSource).GetComponent<SpriteRenderer>();
        s.gameObject.SetActive(true);
        s.transform.parent = transform;
        s.transform.position = transform.position + (Vector3)startPoint;
        s.color = colors[objects.Count % colors.Length];
        CreatedObject c;
        objects.Add(c = new CreatedObject
        {
            gameObject = s.gameObject,
            renderer = s,
            delay = (movedDistance / spacing).floor() * spacing,
            index = objects.Count,
            randomParameter = Random.value
        });
        moveObjectAlong(s.transform, movedDistance);

        return c;
    }

    public override void onStartTracing()
    {
        base.onStartTracing();


        if (isDot)
        {
            var obj = createObject();
            obj.transform.position = transform.position;
            obj.transform.localScale = dotScale.vector();
            obj.transform.right = Vector3.up;
        }
    }

    public override void onStartAnimation()
    {
        base.onStartAnimation();
        progress = 1;
    }
    public override void whileTracing(float movedDistance)
    {
        base.whileTracing(movedDistance);

        if (isDot)
        {
            objects[0].transform.localScale = Vector3.Lerp(Vector3.zero, (dotRadius * 2).vector(), EaseType2.QuadOut.evaluate(progress));
            return;
        }
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

    public override bool whileUnited(float time)
    {
        base.whileUnited(time);
        if (isDot)
            return false;
        moveAllObjectsAlong(unitedSpeed * time);

        return time > _unitedTime;
    }


    protected virtual void onObjectCreated(CreatedObject obj)
    {
        var s = obj.transform.localScale;
        obj.transform.localScale = Vector3.zero;
        obj.transform.DOScale(s, .5f);
    }

}