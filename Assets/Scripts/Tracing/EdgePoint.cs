using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EdgePoint : MonoBehaviour
{



    enum State
    {
        paused,
        playing,
        completed,

    }
    public Pattern pattern { get; set; }

    [SerializeField] float rotationPerDistance = 30;
    [SerializeField] float rotationDamping = 3f;
    [SerializeField] float spawnAnimationDuration = .5f;
    [SerializeField] float spawnAnimationCurveHeight = 1.4f;
    [SerializeField] float spawnAnimationScale = 1.4f;
    [SerializeField] float transitionDuration = .4f;
    [SerializeField] SpriteRenderer playingRenderer, pausedRenderer;
    [SerializeField] PairList<State, SpriteRenderer> stateRenderers = new PairList<State, SpriteRenderer>();

    Tween currentTween;

    float diffMovement;
    float movedOnRotation;

    bool didStateComplete;

    Tween transitTween;




    private void Start()
    {
        if (TracingManager.o.spawnEdgePointsFrom.HasValue)
        {
            spawnFromPoint(TracingManager.o.spawnEdgePointsFrom.Value);
        }
        else
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(1, spawnAnimationDuration).SetEase(Ease.OutBack);
        }

        setStopped();

        // transitRenderer(PatternState.unknown);
    }
    Tween transitRenderer(State state)
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


    public void setPlaying()
    {
        transitRenderer(State.playing);
    }
    public void setStopped()
    {
        transitRenderer(State.paused);
    }
    public void setCompleted()
    {
        transitRenderer(State.completed);
    }



    public Tween punch()
    {
        return transform.DOPunchScale(.2f.vector(), .2f);
    }
    public Tween scaleFromZero()
    {
        return transform.DOScale(1, transitionDuration).SetEase(Ease.OutBack);
    }
    public Tween spawnFromPoint(Vector2 point)
    {
        transform.localScale = Vector3.one * spawnAnimationScale;
        var targetPoint = transform.position;
        transform.position = point;
        return DOTween.Sequence().Append(transform.DOMoveCurvy(targetPoint, spawnAnimationDuration, spawnAnimationCurveHeight))
        .Append(transform.DOScale(1, .5f));
    }
    public void rotateByDistance()
    {
        diffMovement += pattern.movedDistance - movedOnRotation;
        movedOnRotation = pattern.movedDistance;



        var rotationSpeed = Time.deltaTime * this.rotationPerDistance * diffMovement;
        transform.Rotate(Vector3.forward * rotationSpeed);

        diffMovement = Mathf.Lerp(diffMovement, 0, Mathf.Max(Time.fixedDeltaTime, .02f) * rotationDamping);
    }
    public void rotateToOrigin()
    {
        transform.localEulerAngles = new Vector3();
    }






}