using System.Linq;
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
        public bool disableIndicating;
    }

    public class TracingStage : MonoBehaviour
    {

        //fields
        [SerializeField] float animationSpeed = 2;


        //public properties
        public TracingStageInfo info { get; private set; }
        public TracingState state { get; private set; }
        public bool isDone { get; private set; }
        public int segmentIndex { get; private set; } = 0;
        public int segmentCount => filler.segmentCount;
        public Pattern currentSegment => getPattern(segmentIndex);


        public System.Action onStartSegment;
        public System.Action onEndSegment;
        public System.Action onWrongTracing;

        //private variables
        TracerBase tracer;
        LetterFiller filler;


        Pattern getPattern(int segmentIndex)
        {
            return filler.getSegment(segmentIndex) as Pattern;
        }

        IEnumerable<Pattern> getPatterns()
        {
            foreach (var x in filler.segments)
                yield return x as Pattern;
        }

        public void setup(TracingStageInfo info, Glyph glyph)
        {
            this.info = info;
            tracer = info.autoTracing ? GetComponent<AutoTracer>() : GetComponent<HandTracer>();

            filler = LetterFiller.createFiller(glyph, LetterObjectConfig.o.blankLetterFillerPrefab);
            filler.setTotalMovedDistance(0);
            filler.transform.parent = transform;
            filler.transform.position = transform.position;
            filler.swapSegments(TracingConfig.o.getPatternPrefab(info.patternCode));

            foreach (var x in filler.segments)
            {
                (x as Pattern).onCreated();
                x.gameObject.SetActive(false);
            }
        }

        public IEnumerator play()
        {
            yield return tracingCycle();
            yield return animationCycle();
            yield return unitedCycle();

            state = TracingState.done;
            foreach (var x in filler.segments)
                (x as Pattern).onAllDone();
            isDone = true;

        }
        IEnumerator tracingCycle()
        {
            state = TracingState.tracing;
            for (int segmentIndex = 0; segmentIndex < segmentCount; segmentIndex++)
            {
                this.segmentIndex = segmentIndex;
                onStartSegment?.Invoke();
                tracer.flush();
                var pattern = getPattern(segmentIndex);
                pattern.gameObject.SetActive(true);

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

                    if (tracer.isWrongTracing)
                    {
                        onWrongTracing?.Invoke();
                    }
                }
                pattern.whileTracing(movedDistance);

                pattern.onEndTracing();
                if (!pattern.isDot)
                    EdgePointDealer.o.onEndSegment(segmentIndex);

                onEndSegment?.Invoke();
            }

        }
        IEnumerator animationCycle()
        {
            state = TracingState.animation;
            for (var segmentIndex = 0; segmentIndex < segmentCount; segmentIndex++)
            {

                var pattern = getPattern(segmentIndex);
                this.segmentIndex = segmentIndex;

                if (!pattern.useAnimation)
                    continue;

                onStartSegment?.Invoke();

                float movedDistance = 0;
                pattern.onStartAnimation();
                float animationTime = 0;

                if (!pattern.isDot)
                    EdgePointDealer.o.onStartSegment(segmentIndex);

                while (movedDistance < pattern.pathLength)
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


                onEndSegment?.Invoke();
            }

        }
        IEnumerator unitedCycle()
        {
            state = TracingState.united;

            int finishedCount = 0; //count of finished segments

            foreach (var x in getPatterns())
                x.onStartUnited();


            bool[] finishedSegments = new bool[segmentCount];
            float time = 0;
            while (finishedSegments.Any(x => !x))
            {

                for (var segmentIndex = 0; segmentIndex < segmentCount; segmentIndex++)
                {
                    var pattern = getPattern(segmentIndex);
                    this.segmentIndex = segmentIndex;

                    if (!pattern.useUnitedAnimation)
                    {
                        finishedSegments[segmentIndex] = true;
                        continue;
                    }

                    var isDone = pattern.whileUnited(time);
                    finishedSegments[segmentIndex] = isDone;
                    if (isDone)
                        finishedCount++;
                }
                time += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }


            foreach (var x in getPatterns())
                x.onEndUnited();

        }





    }
}