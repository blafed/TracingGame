using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TPhase = KidLetters.TracingPhase;
using UnityEngine.Rendering;

public class EdgePoint : MonoBehaviour
{
    protected enum State
    {
        paused,
        playing,
        completed,

    }
    public int indexInSegment { get; set; }
    public Pattern pattern { get; private set; }

    public bool isFirst { get; set; }

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

    [SerializeField] AudioSource startupAudio;
    [SerializeField] AudioSource collapseAudio;
    [SerializeField] AudioSource startTracingAudio;
    [SerializeField] AudioSource endTracingAudio;
    [SerializeField] AudioSource startAnimationAudio;
    [SerializeField] AudioSource endAnimationAudio;

    [SerializeField] GameObject startTracingEffect;
    [SerializeField] GameObject endTracingEffect;

    Tween currentTween;

    float diffMovement;
    float movedOnRotation;

    bool didStateComplete;

    Tween transitTween;

    public Transform wrapper => transform.GetChild(0);


    SortingGroup sortingGroup;



    private void Awake()
    {
        sortingGroup = GetComponent<SortingGroup>();
    }

    protected virtual void Start()
    {

    }

    public void startupPunch(float delay = 0)
    {
        transform.localScale = new Vector3();
        DOTween.Sequence().Append(transform.DOScale(1.2f, .3f)).
        Append(transform.DOPunchScale(Vector3.one * .5f, .25f, 1, 0)).
        Append(transform.DOScale(1f.vector(), .5f))
        .SetDelay(delay);
        if (startupAudio)
            startupAudio.Play();
        transitRenderer(State.paused);
    }

    public void startupTweening()
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
    }

    public Tween collapse(float duration = .5f)
    {
        if (collapseAudio)
            collapseAudio.Play();
        return transform.DOScale(0, duration).SetEase(Ease.InBack);
    }
    public void onStartTracing()
    {
        if (isFirst)
            if (startTracingAudio)
                startTracingAudio.Play();
        transitRenderer(State.playing);

        if (isFirst)
            if (startTracingEffect)
                startTracingEffect.myActive();



        sortingGroup.sortingOrder++;
    }
    public void onEndTracing()
    {
        if (!isFirst)
            if (endTracingAudio)
                endTracingAudio.Play();

        transitRenderer(State.completed);

        if (endTracingEffect)
            endTracingEffect.myActive();
        sortingGroup.sortingOrder--;
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


    public virtual void setup(Pattern pattern)
    {
        this.pattern = pattern;
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


    public float setDiameter(float diameter)
    {
        wrapper.transform.localScale = Vector3.one * diameter;
        return diameter;
    }





}