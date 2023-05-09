using DG.Tweening;
using UnityEngine;

public class CandyPattern : ObjectPattern
{
    //fields
    [Space]
    public float gravityScale = 1;
    public float maxAddForce = 10;
    public float maxAddTorque = 8;
    public MinMaxF decayDelay = new MinMaxF(.5f, .7f);
    bool keepAnimation;



    //unity events
    private void FixedUpdate()
    {
        if (keepAnimation)
            animateObjects(pathLength);
    }

    //inherited functions

    public override void onEndAnimation()
    {
        //not inherited to not stop audio instantly
        keepAnimation = true;

        Invoke(nameof(stopAudio), 2);
    }

    public override void whileAnimation(float movedDistance)
    {
        animateObjects(movedDistance);
    }

    public override bool whileUnited(float time)
    {
        return true;
    }

    protected override void onObjectCreated(CreatedObject obj)
    {
        base.onObjectCreated(obj);
    }

    //functions
    void stopAudio()
    {
        animationAudio.Stop();
    }
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
                    var rb = x.gameObject.AddComponent<Rigidbody2D>();
                    rb.gravityScale = gravityScale;
                    rb.AddForce(Random.insideUnitCircle * maxAddForce * x.randomParameter, ForceMode2D.Impulse);
                    rb.AddTorque(maxAddTorque * x.randomParameter, ForceMode2D.Impulse);

                    x.transform.DOScale(0, .5f).SetDelay(decayDelay.random);

                }
                x.didExit = true;

            }
            else
            {
                moveObjectAlong(x.transform, total);
            }
        }
    }

}