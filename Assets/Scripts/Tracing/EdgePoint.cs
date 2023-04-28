using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TPhase = KidLetters.TracingPhase;
public class EdgePoint : MonoBehaviour
{



    protected enum State
    {
        paused,
        playing,
        completed,

    }
    public Pattern pattern { get; set; }

    public float diameter = .575f;
    [Space]
    [SerializeField] float rotationPerDistance = 30;
    [SerializeField] float rotationDamping = 3f;
    [SerializeField] float spawnAnimationDuration = .5f;
    [SerializeField] float spawnAnimationCurveHeight = 1.4f;
    [SerializeField] protected float spawnAnimationScale = 1.4f;
    [SerializeField] float transitionDuration = .4f;
    [SerializeField] SpriteRenderer playingRenderer, pausedRenderer;
    [SerializeField] PairList<State, SpriteRenderer> stateRenderers = new PairList<State, SpriteRenderer>();

    Tween currentTween;

    float diffMovement;
    float movedOnRotation;

    bool didStateComplete;

    Tween transitTween;




    protected virtual void Start()
    {
        if (TPhase.o.stageButton)
        {
            spawnFromPoint(TPhase.o.stageButtonPosition);
        }
        else
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(1, spawnAnimationDuration).SetEase(Ease.OutBack);
        }

        setStopped();

        // transitRenderer(PatternState.unknown);
    }
    protected virtual Tween transitRenderer(State state)
    {
        if (transitTween != null)
        {
            transitTween.Kill();
        }
        var seq = DOTween.Sequence();
        foreach (var x in stateRenderers)
        {
            x.value.transform.localScale = (x.key == state ? 0f : 1f).vector();
            seq.Join(x.value.transform.DOScale(x.key == state ? 1 : 0, transitionDuration).SetEase(Ease.OutQuad));
        }
        transitTween = seq;

        return seq;
    }


    public virtual void setPlaying()
    {
        transitRenderer(State.playing);
    }
    public virtual void setStopped()
    {
        transitRenderer(State.paused);
    }
    public virtual void setCompleted()
    {
        transitRenderer(State.completed);
    }



    public virtual Tween punch()
    {
        return transform.DOPunchScale(.2f.vector(), .2f);
    }
    public virtual Tween scaleFromZero()
    {
        return transform.DOScale(1, transitionDuration).SetEase(Ease.OutBack);
    }
    public virtual Tween spawnFromPoint(Vector2 point)
    {
        transform.localScale = Vector3.one * spawnAnimationScale;
        var targetPoint = transform.position;
        transform.position = point;
        return DOTween.Sequence().Append(transform.DOMoveCurvy(targetPoint, spawnAnimationDuration, spawnAnimationCurveHeight))
        .Append(transform.DOScale(1, .5f));
    }
    public virtual void rotateByDistance()
    {
        diffMovement += pattern.movedDistance - movedOnRotation;
        movedOnRotation = pattern.movedDistance;



        var rotationSpeed = Time.deltaTime * this.rotationPerDistance * diffMovement;
        transform.Rotate(Vector3.forward * rotationSpeed);

        diffMovement = Mathf.Lerp(diffMovement, 0, Mathf.Max(Time.fixedDeltaTime, .02f) * rotationDamping);
    }
    public virtual void rotateToOrigin()
    {
        transform.localEulerAngles = new Vector3();
    }






}