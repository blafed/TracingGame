using DG.Tweening;
namespace KidLetters.Tracing
{
    using UnityEngine;
    public class IndicatingDot : PhaseSingletonEntity<IndicatingDot, TracingPhase>
    {
        [SerializeField]
        GameObject effect;


        Pattern pattern;

        protected override void Awake()
        {
            base.Awake();
            effect = transform.GetChild(0).gameObject;
            // circles.iterate(3, x =>
            // {
            //     // x.component.color = x.component.color.alpha(1);
            //     x.component.setAlpha(1);
            //     x.transform.localScale = Vector3.zero;
            //     x.component.setAlphaTween(0, animationDuration).SetLoops(-1).SetDelay(x.iterationIndex * animationDuration);
            //     x.transform.DOScale(1, animationDuration).SetLoops(-1).SetDelay(x.iterationIndex * animationDuration);
            //     x.transform.localPosition = new Vector3();
            // });
        }
        protected override void Start()
        {
            base.Start();
            TracingPhase.o.onStageChanged += (stage) =>
            {
                hide();
                stage.onSegmentChanged += (seg) =>
                {
                    if (!stage.info.autoTracing && seg.isDot)
                        showOnPattern(seg);
                };
            };
            hide();
        }

        void showOnPattern(Pattern pattern)
        {
            transform.position = pattern.transform.position;
            show();
            this.pattern = pattern;
        }
        protected override void onPhaseExit()
        {
            hide();
        }

        void show()
        {
            effect.gameObject.SetActive(true);
        }
        void hide()
        {
            effect.gameObject.SetActive(false);
            pattern = null;
        }


        private void Update()
        {
            if (pattern)
            {
                var stage = TracingPhase.o.currentStage;
                if (!stage || pattern.progress > .9f || stage.state > TracingState.tracing)
                    hide();
            }

        }
    }
}