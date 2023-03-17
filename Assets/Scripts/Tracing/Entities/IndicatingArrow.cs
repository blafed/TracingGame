using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace KidLetters.Tracing
{
    public class IndicatingArrow : PhaseSingletonEntity<IndicatingArrow, TracingPhase>
    {
        public enum Mode
        {
            yoyo,
            fade,
        }

        [SerializeField] Mode mode = Mode.fade;
        [SerializeField] float movingDistance = 1;
        [SerializeField] float movingSpeed = 1;
        [Space]
        [SerializeField] float moveDuration = .5f;
        [SerializeField] Ease moveEase;


        Pattern pattern;
        bool moveDone;
        float movedDistance;
        float movingDirection = 1;


        SpriteRenderer spriteRenderer;

        protected override void Awake()
        {
            base.Awake();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
        protected override void Start()
        {
            base.Start();
            TracingPhase.o.onStageChanged += (stage) =>
            {
                hide();
                stage.onSegmentChanged += (seg) =>
                {
                    if (!stage.info.autoTracing && !seg.isDot)
                        showOnPattern(seg);
                };
            };
            gameObject.SetActive(false);
        }

        protected override void onPhaseEnter()
        {
        }

        protected override void onPhaseExit()
        {
            hide();
        }

        public void showOnPattern(Pattern pattern)
        {
            gameObject.SetActive(true);
            movedDistance = 0;
            movingDirection = 1;
            moveDone = false;
            this.pattern = pattern;


            transform.localScale = Vector3.zero;
            transform.DOScale(1, .5f).SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                moveDone = true;
            });
            gameObject.SetActive(true);
            transform.position = pattern.getPoint(0);
            // move(pattern.getPoint(0));
        }



        public void hide()
        {
            transform.DOKill();
            transform.DOScale(0, .5f).SetEase(Ease.OutCubic);
            pattern = null;
        }

        public void move(Vector2 v)
        {
            moveDone = false;
            transform.DOMove(v, moveDuration).SetEase(moveEase)
            .OnComplete(() =>
            {
                moveDone = true;
            });
        }

        private void Update()
        {
            if (pattern)
            {
                var stage = TracingPhase.o.currentStage;
                if (!stage || pattern.movedDistance >= movingDistance || stage.state > TracingState.tracing)
                {
                    hide();
                    return;
                }

                if (mode == Mode.yoyo)
                {
                    movedDistance += movingSpeed * Time.deltaTime * movingDirection;


                    if (movedDistance > movingDistance || movedDistance > pattern.pathLength)
                    {
                        movingDirection = -1;
                    }
                    else if (movedDistance < 0)
                    {
                        movingDirection = 1;
                    }
                }
                else if (mode == Mode.fade)
                {
                    movedDistance += movingSpeed * Time.deltaTime;
                    var progress = movedDistance / movingDistance;
                    spriteRenderer.color = spriteRenderer.color.alpha(1f - progress);
                    if (progress >= 1)
                    {
                        movedDistance = 0;
                        transform.localScale = Vector3.zero;

                        transform.DOScale(1, .5f).SetEase(Ease.OutQuad);
                    }
                }

                movedDistance = Mathf.Max(movedDistance, .02f);

                transform.position = pattern.getPoint(movedDistance);
                transform.up = pattern.getDirection(movedDistance);
            }
        }

    }
}