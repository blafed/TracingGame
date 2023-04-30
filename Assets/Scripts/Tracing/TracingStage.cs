using System.Collections.Generic;
namespace KidLetters.Tracing
{
    using System;
    using UnityEngine;

    public class TracingStage : MonoBehaviour
    {

        //static
        static TracingPhase phase => TracingPhase.o;
        static Letter letter => phase.letter;

        //public properties
        public TracingStageInfo info { get; private set; }
        public TracingState state { get; private set; }
        public bool hasSegmentChanged { get; private set; }
        public bool isDone { get; private set; }
        public int segmentIndex { get; private set; } = 0;
        public int tracedSegments { get; private set; }
        public int segmentCount => segments.Count;
        public Pattern oldSegment => segments.getOrDefault(segmentIndex - 1);
        public Pattern currentSegment => segmentIndex < segments.Count ? segments[segmentIndex] : null;


        //events
        public event Action<TracingState> onStateChanged;
        public event Action<Pattern> onSegmentChanged;
        public event Action onDone;
        public event Action<Pattern> onSegmentTracingDone;

        //private variables
        List<Pattern> segments = new List<Pattern>();
        float initialTime = 0;
        HandTracer handTracer;
        AutoTracer autoTracer;
        float unitedTime;

        //fields
        [SerializeField] float animationSpeed = 2;
        [SerializeField] List<Pattern> patternObjects = new List<Pattern>();


        public void setup(TracingStageInfo info)
        {
            this.info = info;
        }


        Pattern getPatternPrefab(PatternCode code)
        {
            return patternObjects.Find(x => x.code == code);
        }
        Pattern createPattern(PatternCode code)
        {
            var go = patternObjects.Find(x => x.code == code).gameObject;
            go = Instantiate(go);
            return go.GetComponent<Pattern>();
        }


        private void Start()
        {
            handTracer = GetComponent<HandTracer>();
            autoTracer = GetComponent<AutoTracer>();
            handTracer.setup(this);
            autoTracer.setup(this);
            handTracer.enabled = false;
            autoTracer.enabled = false;


            if (!letter)
            {
                Debug.LogError("Current Letter is not set", gameObject);
                return;
            }
            var patternPrefab = getPatternPrefab(info.patternCode);
            if (!patternPrefab)
            {
                Debug.LogError("No Pattern prefab " + info.patternCode);
                return;
            }
            unitedTime = 0;
            state = TracingState.initial;

            initialTime = patternPrefab.waitBeforeEnableTracing;
            hasSegmentChanged = true;
            foreach (var x in segments)
                Destroy(x.gameObject);
            segments.Clear();
            segmentIndex = 0;
            letter.setTextEnabled(false);
            for (int i = 0; i < letter.segmentCount; i++)
            {
                var seg = letter.get(i);
                var pattern = createPattern(info.patternCode);
                pattern.transform.parent = transform;
                pattern.transform.position = seg.transform.position;
                pattern.setup(seg);
                pattern.progress = 0;
                pattern.onCreated();
                segments.Add(pattern);
                pattern.gameObject.SetActive(false);
            }

        }

        void enableTracers()
        {
            autoTracer.enabled = info.autoTracing && state == TracingState.tracing;
            handTracer.enabled = !info.autoTracing && state == TracingState.tracing;
        }

        private void FixedUpdate()
        {
            enableTracers();
            if (segments.Count == 0)
                return;
            if (initialTime > 0)
            {
                initialTime -= Time.fixedDeltaTime;
                return;
            }

            if (state == TracingState.initial)
            {
                state++;
                onStateChanged?.Invoke(state);
            }


            if (hasSegmentChanged)
            {
                onSegmentChanged?.Invoke(currentSegment);
                if (state == TracingState.tracing)
                {
                    currentSegment.progress = 0;
                    currentSegment.gameObject.SetActive(true);
                    currentSegment.onStartTracing();
                }
                else if (state == TracingState.animation)
                {
                    currentSegment.progress = 0;
                    currentSegment.onStartAnimation();
                }
                else if (state == TracingState.united)
                {
                    foreach (var x in segments)
                    {
                        x.progress = 0;
                        x.onStartUnited();
                    }
                }
                else if (state == TracingState.done)
                {
                    foreach (var x in segments)
                        x.onAllDone();
                    onDone?.Invoke();
                }
                hasSegmentChanged = false;
            }


            switch (state)
            {
                case TracingState.tracing:


                    currentSegment.whileTracing();
                    if (currentSegment.isProgressCompleted)
                    {
                        hasSegmentChanged = true;
                        currentSegment.onEndTracing();
                        segmentIndex++;
                        onSegmentTracingDone?.Invoke(currentSegment);
                    }

                    break;
                case TracingState.animation:
                    currentSegment.whileAnimation();
                    if (currentSegment.isProgressCompleted)
                    {
                        hasSegmentChanged = true;
                        currentSegment.onEndAnimation();
                        currentSegment.onDone();
                        segmentIndex++;
                    }
                    else
                    {
                        currentSegment.movedDistance += animationSpeed * Time.fixedDeltaTime;
                    }
                    break;
                case TracingState.united:
                    unitedTime += Time.fixedDeltaTime;

                    foreach (var x in segments)
                    {
                        x.whileUnited(unitedTime);
                        if (unitedTime > x.unitedTime)
                        {
                            hasSegmentChanged = true;
                            foreach (var y in segments)
                            {
                                y.onEndUnited();
                                state = TracingState.done;
                            }
                            break;
                        }
                    }
                    break;
            }

            if (segmentIndex >= segments.Count)
            {
                segmentIndex = 0;
                state++;
                onStateChanged?.Invoke(state);
            }
        }
    }
}