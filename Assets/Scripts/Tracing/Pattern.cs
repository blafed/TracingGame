using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.U2D;
using UnityEngine;
using KidLetters;

public class Pattern : LetterSegmentFiller
{
    #region FIELDS
    [SerializeField] protected AudioSource tracingAudio;
    [SerializeField] protected AudioSource animationAudio;
    [SerializeField] protected AudioSource unitedAudio;
    [SerializeField] protected GameObject endTracingEffect;
    #endregion

    #region PROPERTIES
    public float newMovedDistance { get; set; }
    public virtual float waitTimeAfterTracing => 0;
    public virtual bool useAnimation => true;
    public virtual bool useUnitedAnimation => true;
    #endregion


    #region FIELDS
    float tracingAudioVolume = -1;
    #endregion

    #region FUNCTIONS
    public virtual void onCreated() { }
    public virtual void onStartTracing() { }
    public virtual void whileTracing(float movedDistance)
    {
        this.movedDistance = movedDistance;
    }
    public virtual void onEndTracing()
    {
        if (tracingAudio)
            tracingAudio.Stop();

        if (endTracingEffect)
        {
            endTracingEffect.transform.position = isDot ? getPoint(.5f) : getPoint(pathLength);
            endTracingEffect.myActive();
        }
    }
    public virtual void onStartAnimation()
    {
        if (animationAudio)
            animationAudio.Play();
    }
    public virtual void whileAnimation(float movedDistance)
    {
    }
    public virtual void onEndAnimation()
    {
        if (animationAudio)
            animationAudio.Stop();
    }
    /// called after animation is done
    public virtual void onDone() { }
    /// called after all segments are completed
    public virtual void onAllDone() { }
    public virtual void onStartUnited()
    {
        if (unitedAudio)
            unitedAudio.Play();
    }
    public virtual bool whileUnited(float time) => true;
    public virtual void onEndUnited() { }
    protected virtual void OnDestroy() { }
    //called when moved distance added each frame
    public virtual void onDistanceState(bool didMoved)
    {
        playTracingAudio(didMoved);
    }

    void playTracingAudio(bool didMoved)
    {
        if (tracingAudio)
        {
            if (tracingAudioVolume < 0)
                tracingAudioVolume = tracingAudio.volume;
            if (!tracingAudio.isPlaying)
                tracingAudio.Play();
        }
        if (tracingAudio && !isDot)
            if (didMoved)
            {
                tracingAudio.volume = Mathf.MoveTowards(tracingAudio.volume, tracingAudioVolume, Time.fixedDeltaTime * 4 * tracingAudioVolume);
            }
            else
            {
                tracingAudio.volume = Mathf.MoveTowards(tracingAudio.volume, 0, Time.fixedDeltaTime * 4 * tracingAudioVolume);
            }
    }
    #endregion


}