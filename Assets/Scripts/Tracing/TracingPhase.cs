using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
namespace KidLetters
{
    using Tracing;
    public class TracingPhase : Phase<TracingPhase>
    {
        //properties
        public Letter letter { get; private set; }
        public WordInfo wordInfo { get; private set; }
        public int doneStage { get; private set; } = -1;
        public TracingStageInfo[] tracingStages { get; private set; } = new TracingStageInfo[3];
        public TracingStage currentStage { get; private set; }
        public Vector2 stageButtonPosition => stageButton.transform.position;
        public StageButton stageButton { get; private set; }

        //events
        public event System.Action<TracingStage> onStageChanged;

        //private
        TracingStage oldStage;

        //fields
        [SerializeField] GameObject stagePrefab;

        //overrides
        protected override void onEnter()
        {
            doneStage = 0;
            List<PatternCode> patternCodes = Enumerable.Range(1, (int)PatternCode.sketch - 1).Select(x => (PatternCode)x).ToList();

            for (int i = 0; i < tracingStages.Length; i++)
            {
                var stg = new TracingStageInfo();
                if (i != 0)
                {
                    var patternCode = patternCodes.getRandom();
                    patternCodes.Remove(patternCode);
                    stg.patternCode = patternCode;

                }
                else
                {
                    stg.autoTracing = true;
                    stg.patternCode = PatternCode.sketch;
                }
                tracingStages[i] = stg;
            }

            StartCoroutine(cycle());
        }
        protected override void onExit()
        {
            StageButtonContainer.o.hide();
            MinorStarContainer.o.hide();
            if (oldStage)
            {
                Destroy(oldStage.gameObject);
            }
            letter.setTextEnabled(true);
            StopAllCoroutines();

        }


        //private functions
        IEnumerator cycle()
        {
            yield return Tracing.FocusOnLetter.o.play();

            Tracing.MinorStarContainer.o.show();
            StageButtonContainer.o.show();


            yield return new WaitUntil(() => doneStage >= tracingStages.Length);
            PronouncingPhase.o.setArgsAfterTracing(this.letter, wordInfo);
            Phase.change(PronouncingPhase.o);
        }

        //public functions
        public void setArgs(Letter letter, WordInfo wordInfo)
        {
            this.letter = letter;
            this.wordInfo = wordInfo;
        }
        public void playStage(int index, StageButton stageButton)
        {
            this.stageButton = stageButton;
            if (oldStage)
                Destroy(oldStage.gameObject);

            currentStage = Instantiate(stagePrefab).GetComponent<TracingStage>();
            currentStage.setup(this.tracingStages[index]);

            currentStage.transform.parent = transform;
            oldStage = currentStage;
            //call event
            onStageChanged?.Invoke(currentStage);

            //registering the removing event after calling onStageChanged because, so let the other entities register their events in an order before removing the reference of currentStage
            currentStage.onDone += () =>
                {
                    if (index >= doneStage)
                    {
                        doneStage++;
                        currentStage = null;
                    }
                };
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
    }
}


