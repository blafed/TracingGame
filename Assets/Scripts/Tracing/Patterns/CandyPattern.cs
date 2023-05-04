using UnityEngine;

public class CandyPattern : ObjectPattern
{
    public float gravityScale = 1;
    public float maxAddForce = 10;
    public float maxAddTorque = 8;
    bool keepAnimation;

    public override void onEndAnimation()
    {
        keepAnimation = true;
    }

    void animateObjects(float moved)
    {
        foreach (var x in objects)
        {

            var total = moved + x.delay;


            if (total > pathLength)
            {
                if (!x.didExit)
                {
                    var rb = x.gameObject.AddComponent<Rigidbody2D>();
                    rb.gravityScale = gravityScale;
                    rb.AddForce(Random.insideUnitCircle * maxAddForce * x.randomParameter, ForceMode2D.Impulse);
                    rb.AddTorque(maxAddTorque * x.randomParameter, ForceMode2D.Impulse);
                }
                x.didExit = true;

                // var flyDir = x.transform.right.toVector2();

                // var d = flyDir;
                // x.transform.position += flyDir.toVector3() * flyingSpeed * Time.fixedDeltaTime;
                // if (Random.value < .5f)
                //     x.randomParameter *= -1;
                // x.transform.Rotate(Vector3.forward * rotationSpeed * Time.fixedDeltaTime * x.randomParameter);
            }
            else
            {
                x.transform.position = getPoint(total);
            }
        }
    }


    private void FixedUpdate()
    {
        if (keepAnimation)
            animateObjects(pathLength);
    }


    public override void whileAnimation(float movedDistance)
    {
        animateObjects(movedDistance);
    }

    public override bool whileUnited(float time)
    {
        return true;
    }

}