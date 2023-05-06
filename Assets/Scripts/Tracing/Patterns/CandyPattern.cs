using DG.Tweening;
using UnityEngine;

public class CandyPattern : ObjectPattern
{
    //fields
    public float gravityScale = 1;
    public float maxAddForce = 10;
    public float maxAddTorque = 8;
    bool keepAnimation;



    //unity events
    private void FixedUpdate()
    {
        if (keepAnimation)
            animateObjects(pathLength);
    }

    //inherited functions
    public override void onStartAnimation()
    {
        base.onStartAnimation();
        animationAudio.Play();
    }

    public override void onEndAnimation()
    {
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

                    x.transform.DOScale(0, .5f).SetDelay(2 + Random.value * 2);
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