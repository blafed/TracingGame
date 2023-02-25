using UnityEngine;
using DG.Tweening;
public class EdgePoint : MonoBehaviour
{




    public Pattern pattern { get; set; }
    PatternState state => pattern.state;

    [SerializeField] float rotationPerDistance = 30;
    [SerializeField] float transitionDuration = .4f;
    [SerializeField] PairList<PatternState, SpriteRenderer> stateRenderers = new PairList<PatternState, SpriteRenderer>();

    Tween currentTween;

    float diffMovement;
    float movedOnRotation;

    PatternState oldState;

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
        if (currentTween != null)
            currentTween.Kill();
        var seq = DOTween.Sequence();
        seq.Join(transitRenderer(state));
        currentTween = seq;
    }

    private void Update()
    {
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