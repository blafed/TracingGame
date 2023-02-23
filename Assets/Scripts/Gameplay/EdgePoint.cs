using UnityEngine;
using DG.Tweening;
public class EdgePoint : MonoBehaviour
{


    enum State
    {
        paused,
        playing,
    }

    [SerializeField] float transitionDuration = .4f;
    [SerializeField] PairList<State, SpriteRenderer> states = new PairList<State, SpriteRenderer>();

    State state;
    Tween currentTween;


    void changeState(State state)
    {
        if (state != this.state)
        {
            var old = this.state;
            this.state = state;
            if (currentTween != null)
                currentTween.Complete();

            var oldRenderer = states.get(old);
            var newRenderer = states.get(state);

            oldRenderer.transform.localScale = Vector3.one;
            newRenderer.transform.localScale = Vector3.zero;

            var seq = DOTween.Sequence();
            seq.Join(oldRenderer.transform.DOScale(0, transitionDuration).SetEase(Ease.OutQuad));
            seq.Join(newRenderer.transform.DOScale(1, transitionDuration)).SetEase(Ease.OutQuad);
            seq.AppendInterval(transitionDuration);
            currentTween = seq;


        }
    }

    public void setPlaying(bool value)
    {
        changeState(value ? State.playing : State.paused);
    }

}