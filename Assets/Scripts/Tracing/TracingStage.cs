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
        public int segmentCount => segmentPatterns.Count;
        public Pattern oldSegmentPattern => segmentPatterns.getOrDefault(segmentIndex - 1);
        public Pattern currentSegmentPattern => segmentIndex < segmentPatterns.Count ? segmentPatterns[segmentIndex] : null;


        //events
        public event Action<TracingState> onStateChanged;
        public event Action<Pattern> onSegmentPatternChanged;
        public event Action onDone;
        public event Action<Pattern> onSegmentPatternDone;

        //private variables
        List<Pattern> segmentPatterns = new List<Pattern>();
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
            foreach (var x in segmentPatterns)
                Destroy(x.gameObject);
            segmentPatterns.Clear();
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
                segmentPatterns.Add(pattern);
                pattern.gameObject.SetActive(false);
            }
            Backgrounds.o.changeRandomly(BackgroundsList.forTracing);

        }

        void enableTracers()
        {
            autoTracer.enabled = info.autoTracing && state == TracingState.tracing;
            handTracer.enabled = !info.autoTracing && state == TracingState.tracing;
        }

        private void FixedUpdate()
        {
            enableTracers();
            if (segmentPatterns.Count == 0)
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
                onSegmentPatternChanged?.Invoke(currentSegmentPattern);
                if (state == TracingState.tracing)
                {
                    currentSegmentPattern.progress = 0;
                    currentSegmentPattern.gameObject.SetActive(true);
                    currentSegmentPattern.onStartTracing();
                }
                else if (state == TracingState.animation)
                {
                    currentSegmentPattern.progress = 0;
                    currentSegmentPattern.onStartAnimation();
                }
                else if (state == TracingState.united)
                {
                    foreach (var x in segmentPatterns)
                    {
                        x.progress = 0;
                        x.onStartUnited();
                    }
                }
                else if (state == TracingState.done)
                {
                    foreach (var x in segmentPatterns)
                        x.onAllDone();
                    onDone?.Invoke();
                }
                hasSegmentChanged = false;
            }


            switch (state)
            {
                case TracingState.tracing:


                    currentSegmentPattern.whileTracing();
                    if (currentSegmentPattern.isProgressCompleted)
                    {
                        hasSegmentChanged = true;
                        currentSegmentPattern.onEndTracing();
                        segmentIndex++;
                    }

                    break;
                case TracingState.animation:
                    currentSegmentPattern.whileAnimation();
                    if (currentSegmentPattern.isProgressCompleted)
                    {
                        hasSegmentChanged = true;
                        currentSegmentPattern.onEndAnimation();
                        currentSegmentPattern.onDone();
                        segmentIndex++;
                    }
                    else
                    {
                        currentSegmentPattern.movedDistance += animationSpeed * Time.fixedDeltaTime;
                    }
                    break;
                case TracingState.united:
                    unitedTime += Time.fixedDeltaTime;

                    foreach (var x in segmentPatterns)
                    {
                        x.whileUnited(unitedTime);
                        if (unitedTime > x.unitedTime)
                        {
                            hasSegmentChanged = true;
                            foreach (var y in segmentPatterns)
                            {
                                y.onEndUnited();
                                state = TracingState.done;
                            }
                            break;
                        }
                    }
                    break;
            }

            if (segmentIndex >= segmentPatterns.Count)
            {
                segmentIndex = 0;
                state++;
                onStateChanged?.Invoke(state);
            }
        }
    }
}