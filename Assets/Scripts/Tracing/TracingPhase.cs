using DG.Tweening;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
namespace KidLetters
{
    using Tracing;
    public class TracingPhase : Phase<TracingPhase>
    {

        [SerializeField]
        TracingStageInfo[] overrideStages = new TracingStageInfo[0];
        public int letterId { get; private set; }
        //properties
        public LetterFiller letter { get; private set; }
        public WordInfo wordInfo { get; private set; }
        [System.Obsolete]
        public int doneStage { get; private set; } = 0;
        public TracingStageInfo[] tracingStages { get; private set; }
        public TracingStage currentStage { get; private set; }
        public Vector2 stageButtonPosition => stageButton.transform.position;
        public StageButton stageButton { get; private set; }


        public event System.Action onFocused;

        //events
        public event System.Action<TracingStage> onStageChanged;

        //private
        TracingStage oldStage;

        //fields
        [SerializeField] GameObject stagePrefab;

        //overrides
        protected override void onEnter()
        {
            stageButton = null;
            doneStage = 0;
            List<PatternCode> patternCodes = Enumerable.Range(1, (int)PatternCode.sketch - 1).Select(x => (PatternCode)x).ToList();

            tracingStages = new TracingStageInfo[3];

            for (int i = 0; i < tracingStages.Length; i++)
            {
                var stg = new TracingStageInfo();
                if (i > 0 && i < tracingStages.Length - 1)
                {
                    var patternCode = patternCodes.getRandom();
                    patternCodes.Remove(patternCode);
                    stg.patternCode = patternCode;
                    stg.showThinLetter = true;

                }
                else
                {
                    stg.autoTracing = i == 0;
                    stg.patternCode = i == 0 ? PatternCode.sketch : PatternCode.brush;
                }
                // stg.showThinLetter = !(stg.patternCode == PatternCode.sketch || stg.patternCode == PatternCode.brush);
                tracingStages[i] = i < overrideStages.Length ? overrideStages[i] : stg;
            }

            letter = LetterFiller.createStandardFiller(Home.LetterContainer.o.getLetter(letterId));
            StartCoroutine(cycle());
        }
        protected override void onExit()
        {
            // StageButtonContainer.o.hide();
            // MinorStarContainer.o.hide();
            if (oldStage)
            {
                Destroy(oldStage.gameObject);
            }
            letter.setEnabled(true);
            StopAllCoroutines();
            EdgePointDealer.o.clearEdgePoints();
            letter.setColor(Color.white);

        }




        //private functions
        IEnumerator cycle()
        {
            yield return Tracing.FocusOnLetter.o.play();
            onFocused?.Invoke();
            yield return Tracing.TracingController.o.play();
            PronouncingPhase.o.setArgsAfterTracing(letterId, wordInfo);
            Phase.change(PronouncingPhase.o);
        }

        //public functions
        public void setArgs(int letterId, WordInfo wordInfo)
        {
            this.letterId = letterId;
            this.wordInfo = wordInfo;
        }
        public void playStage(int index, StageButton stageButton)
        {
            this.stageButton = stageButton;
            if (oldStage)
                Destroy(oldStage.gameObject);

            currentStage = Instantiate(stagePrefab).GetComponent<TracingStage>();
            currentStage.transform.position = letter.transform.position;
            currentStage.setup(this.tracingStages[index]);

            currentStage.endSegmentOffset = TracingConfig.o.edgePointRadius;
            currentStage.transform.parent = transform;
            oldStage = currentStage;
            //call event

            // currentStage.onSegmentChanged += (x) =>
            // {
            //     if (currentStage.state == TracingState.tracing)
            //     {
            //         var segmentIndex = currentStage.segmentIndex;
            //         EdgePointDealer.o.onStartSegment(segmentIndex);
            //         if (segmentIndex > 0)
            //             EdgePointDealer.o.onEndSegment(segmentIndex - 1);
            //     }
            // };
            // currentStage.onDone += () =>
            // {
            // currentStage.transform.DOScale(0, .5f).SetEase(Ease.InBack);
            // EdgePointDealer.o.clearEdgePointsTween(.5f, .1f);
            // if (index >= doneStage)
            // {
            // doneStage++;
            // currentStage = null;
            // }
            // };

            onStageChanged?.Invoke(currentStage);
            //registering the removing event after calling onStageChanged because, so let the other entities register their events in an order before removing the reference of currentStage
            // currentStage.onDone += () =>
            // {
            // if (index >= doneStage)
            // {
            // currentStage = null;
            // }
            // };

        }
        public bool canPlayStage(int index)
        {
            return index <= doneStage;
        }

    }

    [System.Serializable]
    public struct TracingStageInfo
    {
        public PatternCode patternCode;
        public bool autoTracing;
        public bool showThinLetter;
    }
}

public enum TracingState
{
    initial,
    tracing,
    animation,
    united,
    done,
}