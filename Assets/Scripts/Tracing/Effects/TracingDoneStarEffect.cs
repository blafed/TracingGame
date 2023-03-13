using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
public class TracingDoneStarEffect : PhaseEntity
{
    public static TracingDoneStarEffect o;
    [SerializeField] CanvasGroup target;
    [SerializeField] float growDuration = .3f;
    [SerializeField] float stayDuration = .5f;
    [SerializeField] float moveDuration = .5f;
    [SerializeField] float stayScale = 1;
    [SerializeField] float moveScale = .25f;


    protected override void register()
    {
        TracingPhase.o.registerEntity(this);
    }

    protected override void Awake()
    {
        base.Awake();
        o = this;
    }


    public System.Action onDone;
    public void startAnimation()
    {

    }


    public override void onPhaseExit()
    {
        transform.DOKill();
        target
        .gameObject.SetActive(false);
    }



    public Tween startStarAnimation(System.Action onComplete)
    {
        target.gameObject.SetActive(true);
        var startPosition = TracingPanelUI.o.getFocusPosition();
        var endPosition = TracingPanelUI.o.playPatternButtons.get(TracingPhase.o.playingIndex).component.transform.position;

        var d = this;
        var t = d.target;
        t.transform.localPosition = startPosition;
        t.transform.localScale = Vector3.zero;
        t.transform.DOScale(d.stayScale, d.growDuration).SetEase(Ease.OutQuad);
        return DOTween.Sequence().AppendInterval(d.stayDuration)
        .Join(t.transform.DOMove(endPosition, d.moveDuration))
        .Join(t.transform.DOScale(d.moveScale, d.moveDuration / 2))
        .OnComplete(() => onComplete())
        ;
    }


}
