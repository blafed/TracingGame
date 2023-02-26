using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EdgePoint : MonoBehaviour
{




    public Pattern pattern { get; set; }
    PatternState state => pattern.state;

    [SerializeField] float rotationPerDistance = 30;
    [SerializeField] float spawnAnimationDuration = .5f;
    [SerializeField] float spawnAnimationCurveHeight = 1.4f;
    [SerializeField] float transitionDuration = .4f;
    [SerializeField] PairList<PatternState, SpriteRenderer> stateRenderers = new PairList<PatternState, SpriteRenderer>();

    Tween currentTween;

    float diffMovement;
    float movedOnRotation;

    PatternState oldState;
    bool didStateComplete;




    private void Start()
    {
        if (TracingManager.o.spawnEdgePointsFrom.HasValue)
        {
            var targetPoint = transform.position;
            transform.position = TracingManager.o.spawnEdgePointsFrom.Value;
            transform.DOMoveCurvy(targetPoint, spawnAnimationDuration, spawnAnimationCurveHeight);
        }
        else
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(1, spawnAnimationDuration).SetEase(Ease.OutBack);
        }

        // transitRenderer(PatternState.unknown);
    }
    Tween transitRenderer(PatternState state)
    {
        var seq = DOTween.Sequence();
        foreach (var x in stateRenderers)
        {
            seq.Join(x.value.transform.DOScale(x.key == state ? 0 : 1, transitionDuration).SetEase(Ease.OutQuad));
        }
        return seq;
    }
    void onPatternStateChange()
    {
        onPatternStateChange(this.state);
    }

    void onPatternStateChange(PatternState customState)
    {
        if (currentTween != null)
            currentTween.Kill();
        var seq = DOTween.Sequence();
        seq.Join(transitRenderer(state));
        currentTween = seq;
    }

    private void Update()
    {
        didStateComplete = pattern.isStateCompleted;
        if (pattern.state != oldState)
            onPatternStateChange();
        oldState = pattern.state;
        if (pattern.isTracing)
        {
            currentTween = null;
            diffMovement += pattern.movedDistance - movedOnRotation;
            movedOnRotation = pattern.movedDistance;

            var rotationSpeed = Time.deltaTime * this.rotationPerDistance * Mathf.Min(1, diffMovement);
            transform.Rotate(Vector3.forward * rotationSpeed);

            diffMovement -= Time.deltaTime;
            diffMovement = Mathf.Max(0, diffMovement);
        }
        else
        {
            transform.localEulerAngles = default;
        }
    }




}