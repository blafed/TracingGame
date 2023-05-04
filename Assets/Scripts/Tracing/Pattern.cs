using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.U2D;
using UnityEngine;
using KidLetters;

public class Pattern : LetterSegmentFiller
{
    public float newMovedDistance { get; set; }
    public virtual bool useAnimation => true;
    public virtual bool useUnitedAnimation => true;

    public virtual void onCreated() { }
    public virtual void onStartTracing() { }



    public virtual void whileTracing(float movedDistance)
    {
        this.movedDistance = movedDistance;
    }
    public virtual void onEndTracing() { }
    public virtual void onStartAnimation() { }
    public virtual void whileAnimation(float movedDistance)
    {
    }
    public virtual void onEndAnimation() { }
    /// called after animation is done
    public virtual void onDone() { }
    /// called after all segments are completed
    public virtual void onAllDone() { }
    public virtual void onStartUnited() { }
    public virtual bool whileUnited(float time)
    {
        return time > 1;
    }
    public virtual void onEndUnited() { }
    protected virtual void OnDestroy() { }


}