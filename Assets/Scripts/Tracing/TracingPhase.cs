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

        //fields
        [SerializeField]
        TracingStageInfo[] overrideStages = new TracingStageInfo[0];


        //properties
        public int letterId { get; private set; }
        public LetterFiller letter { get; private set; }
        public WordInfo wordInfo { get; private set; }

        public int stageIndex { get; private set; }
        public List<TracingStageInfo> stageInfos { get; private set; }
        public TracingStage currentStage { get; private set; }
        public Vector2? spawnEdgesPointsFrom { get; private set; }



        //events
        public event System.Action<TracingStage> onStageChanged;

        //private
        TracingStage oldStage;

        //fields
        [SerializeField] GameObject stagePrefab;

        //overrides
        protected override void onEnter()
        {
            spawnEdgesPointsFrom = null;
            stageIndex = 0;
            List<PatternCode> patternCodes = Enumerable.Range(1, (int)PatternCode.sketch - 1).Select(x => (PatternCode)x).ToList();

            stageInfos = new();

            for (int i = 0; i < 3; i++)
            {
                var stg = new TracingStageInfo();
                if (i == 1)
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
                stg.disableIndicating = stg.patternCode == PatternCode.brush;
                stg.disableEdgePoints = stg.patternCode == PatternCode.sketch;
                stg.showThinLetter = !(stg.patternCode == PatternCode.sketch || stg.patternCode == PatternCode.brush);
                if (i < overrideStages.Length)
                    stg = overrideStages[i];
                stageInfos.Add(stg);
            }

            letter = LetterFiller.createStandardFiller(Home.LetterContainer.o.getLetter(letterId));
            StartCoroutine(cycle());
        }
        protected override void onExit()
        {
            if (oldStage)
            {
                Destroy(oldStage.gameObject);
            }
            letter.setEnabled(true);
            StopAllCoroutines();
            EdgePointDealer.o.clearEdgePoints();
            letter.setColor(Color.white);

            Destroy(letter.gameObject);


            IndicatingArrow.o.hide();
            IndicatingDot.o.hide();

        }




        //private functions
        IEnumerator cycle()
        {
            yield return Tracing.FocusOnLetter.o.play();
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
        public void playStage(int index, Vector2? spawnEdgePointsAt = null)
        {
            stageIndex = index;
            this.spawnEdgesPointsFrom = spawnEdgePointsAt;
            if (oldStage)
                Destroy(oldStage.gameObject);

            currentStage = Instantiate(stagePrefab).GetComponent<TracingStage>();
            currentStage.transform.position = letter.transform.position;
            currentStage.setup(this.stageInfos[index], letter.glyph);

            currentStage.transform.parent = transform;
            oldStage = currentStage;

            onStageChanged?.Invoke(currentStage);
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
}

