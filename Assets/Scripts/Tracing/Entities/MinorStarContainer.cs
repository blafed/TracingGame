using DG.Tweening;
using System.Collections;
using System.Collections.Generic;


namespace KidLetters.Tracing
{
    using UnityEngine;

    public class MinorStarContainer : PhaseSingletonEntity<MinorStarContainer, TracingPhase>
    {

        static TracingStage stage => TracingPhase.o.currentStage;

        //fields
        [SerializeField] GameObject walkingStarPrefab;
        [SerializeField] GameObject starExplosionPrefab;
        [Header("Options")]
        [SerializeField] float walkingDelay = .5f;
        [SerializeField] float walkingDuration = .5f;
        [SerializeField] float walkingToButtonDuration = .5f;
        [SerializeField] float walkingToButtonDelay = .4f;


        [SerializeField]
        FlowList<MinorStar> stars = new();


        int walkedStarsCount;

        protected override void Start()
        {
            base.Start();
            TracingPhase.o.onStageChanged += onStageChanged;
            hide();
        }

        protected override void onPhaseEnter()
        {
            stars.clear();
            walkedStarsCount = 0;
            show();
        }

        protected override void onPhaseExit()
        {
            hide();
        }

        void onStageChanged(TracingStage stage)
        {
            stage.onSegmentTracingDone += onSegmentTracingDone;

            stars.clear();
            walkedStarsCount = 0;
            refresh();
        }

        void refresh()
        {
            stars.iterate(stars.count, x => x.gameObject.SetActive(false));
            stars.iterate(stage.segmentCount, x =>
            {
                x.gameObject.SetActive(true);
                x.component.setDone(stage.segmentIndex > x.iterationIndex);
            });
        }


        void onSegmentTracingDone(Pattern segmentPattern)
        {
            var stage = TracingPhase.o.currentStage;
            var oldSegmentPattern = stage.oldSegment;

            if (!oldSegmentPattern)
                return;


            var point = oldSegmentPattern.getPoint(oldSegmentPattern.pathLength);

            var walkingStar = Instantiate(walkingStarPrefab, point, default);
            walkingStar.transform.parent = transform;
            if (starExplosionPrefab)
                Instantiate(starExplosionPrefab, point, default);

            var seq = DOTween.Sequence();
            seq.AppendInterval(walkingDelay);
            seq.Append(
                DOTween.Sequence()
                .Join(
                walkingStar.transform.DOMoveCurvy(
                stars.get(stage.segmentIndex - 1).transform.position,
                 walkingDuration))
                 .Join(walkingStar.transform.DOScale(Vector3.zero, walkingDuration))
                 );
            seq.OnComplete(() =>
            {
                Destroy(walkingStar.gameObject);
                walkedStarsCount++;
                if (walkedStarsCount == stars.count)
                {
                    onStageDone();
                }
                // TracingPhase.o.stageButton.setDone();
                // refresh();

                // if (stage.segmentIndex >= stage.segmentCount)
                // {
                //     onStageDone();
                // }
            });
        }

        void onStageDone()
        {

            var seq = DOTween.Sequence();

            for (int i = 0; i < stage.segmentCount; i++)
            {
                var star = Instantiate(walkingStarPrefab);
                star.transform.position = stars.get(i).transform.position;
                var t = star.transform.DOMove(TracingPhase.o.stageButtonPosition, walkingToButtonDuration)
                .SetDelay(walkingToButtonDelay * i)
                .OnComplete(() =>
                {
                    TracingPhase.o.stageButton.punch();
                });
                seq.Append(t);
                // seq.Append(t);
                // seq.AppendInterval(walkingToButtonDelay);
            }
            seq.OnComplete(() =>
            {
                TracingPhase.o.stageButton.setDone();
            });
        }


        void show()
        {
            gameObject.SetActive(true);
            transform.localScale = Vector3.zero;
            transform.DOScale(1, .5f).SetEase(Ease.OutBack);
        }
        void hide()
        {
            gameObject.SetActive(false);
        }

    }
}