using System.Collections.Generic;
using UnityEngine;


public class ButterflyPattern : ObjectPattern
{
    //Fields
    public float flyingSpeed = 2;
    public float flyingRandomness = .5f;
    public float rotationSpeed = 720;

    //Variables
    bool keepAnimation;

    //Unity Events
    private void FixedUpdate()
    {
        if (keepAnimation)
            animateObjects(pathLength);
    }


    //Inherited functions
    public override void onEndAnimation()
    {
        base.onEndAnimation();
        keepAnimation = true;
    }
    public override bool whileUnited(float time)
    {
        return true;
    }

    public override void whileAnimation(float movedDistance)
    {
        animateObjects(movedDistance);
    }

    //functions
    void animateObjects(float moved)
    {
        foreach (var x in objects)
        {

            var total = moved + x.delay;


            if (total > pathLength || isDot)
            {
                if (!x.didExit)
                {
                    // x.transform.GetChild(0).parent = x.transform.parent;
                }
                x.didExit = true;
                var flyDir = x.transform.right.toVector2();

                var d = flyDir;
                x.transform.position += flyDir.toVector3() * flyingSpeed * Time.fixedDeltaTime;
                if (Random.value < .5f)
                    x.randomParameter *= -1;
                x.transform.Rotate(Vector3.forward * rotationSpeed * Time.fixedDeltaTime * x.randomParameter);
            }
            else
            {
                moveObjectAlong(x.transform, total);
            }
        }
    }



}