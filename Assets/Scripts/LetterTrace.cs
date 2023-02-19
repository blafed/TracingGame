using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.Sprites;
public class LetterTrace : MonoBehaviour
{

    public AnimationCurve curve;



    public GameObject sample;
    public GameObject endpoint;
    public List<Vector2> points = new();

    public Vector2 getPoint(int index) => points[index] + (Vector2)transform.position;


    public float duration = 1;
    public float elabsed;

    int inPoint = 0;

    SpriteShapeController ssc;
    Transform currentEndPoint;

    private void Start()
    {
        foreach (var x in points)
        {
            currentEndPoint = Instantiate(endpoint, (Vector2)transform.position + x, default).transform;
        }
    }
    void createSsc()
    {
        ssc = Instantiate(sample).GetComponent<SpriteShapeController>();
        ssc.transform.position = transform.position;
    }

    private void Update()
    {

        if (inPoint + 1 < points.Count)
        {
            if (!ssc)
            {
                createSsc();
            }
            var current = points[inPoint];
            var next = points[inPoint + 1];

            var t = elabsed / duration;

            var lerp = Vector2.Lerp(current, next, Mathf.Max(t, .01f));
            ssc.spline.SetPosition(0, current);
            ssc.spline.SetPosition(1, lerp);
        }

        elabsed += Time.deltaTime;
        if (elabsed >= duration)
        {
            inPoint++;
            elabsed = 0;
            ssc = null;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        for (int i = 0; i < points.Count; i++)
        {
            var p = getPoint(i);
            Gizmos.DrawWireSphere(p, .3f);
        }
    }
}
