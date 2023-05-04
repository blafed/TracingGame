using System.Collections;
using System.Collections.Generic;
namespace KidLetters.Tracing
{
    using System;
    using UnityEngine;

    [System.Serializable]
    public struct TracingStageInfo
    {
        public PatternCode patternCode;
        public bool autoTracing;
        public bool showThinLetter;
        public bool disableEdgePoints;
    }

    public class TracingStage : MonoBehaviour
    {

        //public properties
        public TracingStageInfo info { get; private set; }
        public TracingState state { get; private set; }
        public bool isDone { get; private set; }
        public int segmentIndex { get; private set; } = 0;
        public int segmentCount => filler.segmentCount;


        //events
        public event Action<TracingState> onStateChanged;
        public event Action<Pattern> onSegmentChanged;
        [System.Obsolete]
        public event Action onDone;
        [System.Obsolete]
        public event Action<Pattern> onSegmentTracingDone;

        //private variables

        TracerBase tracer;
        LetterFiller filler;
        // List<Pattern> segments = new List<Pattern>();

        //fields
        [SerializeField] float animationSpeed = 2;


        public void setup(TracingStageInfo info, Glyph glyph)
        {
            this.info = info;
            tracer = info.autoTracing ? GetComponent<AutoTracer>() : GetComponent<HandTracer>();

            filler = LetterFiller.createFiller(glyph, LetterObjectConfig.o.blankLetterFillerPrefab);
            filler.transform.parent = transform;
            filler.transform.position = transform.position;
            filler.swapSegments(TracingConfig.o.getPatternPrefab(info.patternCode));

            foreach (var x in filler.segments)
            {
                (x as Pattern).onCreated();
                x.gameObject.SetActive(false);
            }
        }

        Pattern getPattern(int segmentIndex)
        {
            return filler.getSegment(segmentIndex) as Pattern;
        }


        public IEnumerator play()
        {
            yield return tracingCycle();
            yield return animationCycle();
            yield return unitedCycle();

            foreach (var x in filler.segments)
                (x as Pattern).onAllDone();
            isDone = true;

        }
        IEnumerator tracingCycle()
        {
            for (int segmentIndex = 0; segmentIndex < segmentCount; segmentIndex++)
            {

                tracer.flush();
                var pattern = getPattern(segmentIndex);
                pattern.gameObject.SetActive(true);
                this.segmentIndex = segmentIndex;

                var seg = filler.getSegment(segmentIndex);
                var movedDistance = pattern.movedDistance = 0f;
                pattern.onStartTracing();

                float tracingTime = 0;
                float targetTracingLength = pattern.pathLength - (pattern.isDot || info.disableEdgePoints ? 0 : TracingConfig.o.edgePointRadius / 2f);

                if (!pattern.isDot)
                    EdgePointDealer.o.onStartSegment(segmentIndex);
                while (!pattern.isProgressCompleted)
                {
                    pattern.newMovedDistance = movedDistance;
                    pattern.whileTracing(movedDistance);
                    movedDistance = pattern.newMovedDistance;

                    yield return new WaitForFixedUpdate();

                    tracingTime += Time.fixedDeltaTime;
                    movedDistance += tracer.getNewMovement(pattern, Time.fixedDeltaTime) - pattern.movedDistance;

                    if (movedDistance >= targetTracingLength)
                        movedDistance = pattern.pathLength;
                }
                pattern.whileTracing(movedDistance);

                pattern.onEndTracing();
                if (!pattern.isDot)
                    EdgePointDealer.o.onEndSegment(segmentIndex);
            }

        }
        IEnumerator animationCycle()
        {
            for (var segmentIndex = 0; segmentIndex < segmentCount; segmentIndex++)
            {
                var pattern = getPattern(segmentIndex);
                this.segmentIndex = segmentIndex;

                if (!pattern.useAnimation)
                    continue;


                float movedDistance = 0;
                pattern.onStartAnimation();
                float animationTime = 0;

                if (!pattern.isDot)
                    EdgePointDealer.o.onStartSegment(segmentIndex);

                while (movedDistance <= pattern.pathLength)
                {
                    pattern.newMovedDistance = movedDistance;
                    pattern.whileAnimation(movedDistance);
                    movedDistance = pattern.newMovedDistance;
                    yield return new WaitForFixedUpdate();
                    animationTime += Time.fixedDeltaTime;
                    movedDistance += Time.fixedDeltaTime * animationSpeed;
                }
                pattern.whileAnimation(movedDistance);

                pattern.onEndAnimation();
                if (!pattern.isDot)
                    EdgePointDealer.o.onEndSegment(segmentIndex);
            }

        }
        IEnumerator unitedCycle()
        {
            for (var segmentIndex = 0; segmentIndex < segmentCount; segmentIndex++)
            {
                var pattern = getPattern(segmentIndex);
                this.segmentIndex = segmentIndex;

                if (!pattern.useUnitedAnimation)
                    continue;

                pattern.onStartUnited();

                float time = 0;
                while (true)
                {
                    var isDone = pattern.whileUnited(time);
                    if (isDone)
                        break;
                    yield return new WaitForFixedUpdate();
                    time += Time.fixedDeltaTime;
                }

                pattern.onEndUnited();
            }
        }



    }
}