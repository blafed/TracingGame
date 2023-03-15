using DG.Tweening;
using System.Collections;
using System.Collections.Generic;


namespace KidLetters.Tracing
{
    using UnityEngine;

    public class MinorStarContainer : Singleton<MinorStarContainer>
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
        private void Start()
        {
            TracingPhase.o.onStageChanged += onStageChanged;
            TracingPhase.o.onEnterEvent += () =>
            {
                stars.clear();
            };

            hide();
        }
        void onStageChanged(TracingStage stage)
        {
            stage.onDone += onStageDone;
            stage.onSegmentPatternChanged += onSegmentChange;

            stars.clear();
            refresh();
        }

        void refresh()
        {
            stars.iterate(stars.count, x => x.gameObject.SetActive(false));
            stars.iterate(stage.segmentCount, x =>
            {
                x.gameObject.SetActive(false);
                x.component.setDone(stage.segmentIndex > x.iterationIndex);
            });
        }

        void onSegmentChange(Pattern segmentPattern)
        {
            var stage = TracingPhase.o.currentStage;
            var oldSegmentPattern = stage.oldSegmentPattern;

            if (!oldSegmentPattern)
                return;

            var walkingStar = Instantiate(walkingStarPrefab, oldSegmentPattern.transform.position, default);
            if (starExplosionPrefab)
                Instantiate(starExplosionPrefab, oldSegmentPattern.transform.position, default);


            var seq = DOTween.Sequence();
            seq.AppendInterval(walkingDelay);
            seq.Append(walkingStar.transform.DOMoveCurvy(stars.get(stage.segmentIndex).transform.position, walkingDuration));
            seq.OnComplete(() =>
            {
                refresh();
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


        public void show()
        {
            gameObject.SetActive(true);
            transform.localScale = Vector3.zero;
            transform.DOScale(1, .5f).SetEase(Ease.OutBack);
        }
        public void hide()
        {
            gameObject.SetActive(false);
        }

    }
}