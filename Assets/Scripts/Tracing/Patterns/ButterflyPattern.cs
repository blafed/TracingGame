using System.Collections.Generic;
using UnityEngine;


public class ButterflyPattern : ObjectPattern
{
    public float flyingSpeed = 2;

    public float flyingRandomness = .5f;
    public float rotationSpeed = 720;



    bool keepAnimation;

    public override void onEndAnimation()
    {
        keepAnimation = true;
    }


    void keepFlying(float movedDistance)
    {
        foreach (var x in objects)
        {

            var total = movedDistance + x.delay;


            if (total > pathLength)
            {

                var flyDir = x.transform.right.toVector2();

                var d = flyDir;
                x.transform.position += flyDir.toVector3() * flyingSpeed * Time.fixedDeltaTime;
                if (Random.value < .5f)
                    x.randomParameter *= -1;
                x.transform.Rotate(Vector3.forward * rotationSpeed * Time.fixedDeltaTime * x.randomParameter);
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
            keepFlying(pathLength);
    }


    public override void whileAnimation(float movedDistance)
    {
        keepFlying(movedDistance);
    }

    public override bool whileUnited(float time)
    {
        return true;
    }
}