using UnityEngine;
using System.Collections.Generic;


public enum TracingState
{
    initial,
    tracing,
    animation,
    united,
    done,
}
public class TracingManager : MonoBehaviour
{
    [System.Serializable]
    public class Options
    {
        public float speed = 2;
        public float cameraZoom = 5;
        public float autoTracingSegmentDelay = .5f;

        public EdgePoint edgePointPrefab;
        public List<Pattern> patternObjects = new List<Pattern>();


    }
    public static TracingManager o { get; private set; }

    public bool isTracingStarted { get; private set; }
    public Vector2? spawnEdgePointsFrom { get; set; }

    [SerializeField]
    public Options options = new Options();
    public bool autoTracing = true;

    Letter currentLetter;

    List<Pattern> segmentPatterns = new List<Pattern>();
    int segmentIndex;

    public Pattern currentSegmentPattern
    {
        get => segmentIndex >= segmentPatterns.Count ? null : segmentPatterns[segmentIndex];
    }
    public TracingState state { get; private set; }


    public event System.Action onDone;
    public event System.Action<Pattern> onSegmentPatternChanged;
    public event System.Action<TracingState> onStateChanged;



    public bool hasSegmentChanged = true;
    float initialTime = 0;
    float unitedTime;
    float autoTracingSegmentTimer;


    // public Letter letter;

    private void Awake()
    {
        o = this;
    }
    Pattern getPatternPrefab(PatternCode code)
    {
        return options.patternObjects.Find(x => x.code == code);
    }
    Pattern createPattern(PatternCode code)
    {
        var go = options.patternObjects.Find(x => x.code == code).gameObject;
        go = Instantiate(go);
        return go.GetComponent<Pattern>();
        // return Resources.Load<GameObject>("Patterns/" + code.ToString().capitalize() + "Pattern");
    }


    public void leave()
    {
        currentLetter.setTextEnabled(true);
        foreach (var x in segmentPatterns)
            Destroy(x.gameObject);
        segmentPatterns.Clear();
        segmentIndex = 0;
    }


    public void clean()
    {
        HandTracing.o.setEnabled(false);

        if (!isTracingStarted)
            return;
        currentLetter.text.alpha = 1;
        currentLetter.setTextEnabled(true);
        foreach (var x in segmentPatterns)
            Destroy(x.gameObject);
        segmentPatterns.Clear();
        currentLetter = null;
    }


    public void startTracing(Letter letter)
    {
        if (currentLetter)
        {
            leave();
        }
        // CameraControl.o.move(letter.transform.position);
        // CameraControl.o.zoom(options.cameraZoom);
        this.currentLetter = letter;
        this.currentLetter.setTextEnabled(false);
        isTracingStarted = true;
    }
    public void setTracingPattern(PatternCode patternCode)
    {
        if (!currentLetter)
        {
            Debug.LogError("Current Letter is not set", gameObject);
            return;
        }
        var patternPrefab = getPatternPrefab(patternCode);
        if (!patternPrefab)
        {
            Debug.LogError("No Pattern prefab " + patternCode);
            return;
        }
        unitedTime = 0;
        state = TracingState.initial;

        initialTime = patternPrefab.waitBeforeEnableTracing;
        spawnEdgePointsFrom = null;
        hasSegmentChanged = true;
        foreach (var x in segmentPatterns)
            Destroy(x.gameObject);
        segmentPatterns.Clear();
        segmentIndex = 0;
        currentLetter.setTextEnabled(false);
        for (int i = 0; i < currentLetter.segmentCount; i++)
        {
            var seg = currentLetter.get(i);
            var pattern = createPattern(patternCode);
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

    private void FixedUpdate()
    {
        if (segmentPatterns.Count == 0)
            return;
        if (initialTime > 0)
        {
            initialTime -= Time.fixedDeltaTime;
            return;
        }
        setHandTracing(!autoTracing);

        if (state == TracingState.initial)
        {
            state++;
            onStateChanged?.Invoke(state);
        }


        if (hasSegmentChanged)
        {
            autoTracingSegmentTimer = options.autoTracingSegmentDelay;
            HandTracing.o.reset();
            HandTracing.o.onSegmentPatternChange(currentSegmentPattern);
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
        if (state == TracingState.tracing)
        {

            currentSegmentPattern.whileTracing();
            if (currentSegmentPattern.isProgressCompleted)
            {
                hasSegmentChanged = true;
                currentSegmentPattern.onEndTracing();
                segmentIndex++;
            }
            else if (autoTracing)
            {
                currentSegmentPattern.movedDistance += Mathf.Max(0, options.speed * Time.fixedDeltaTime - (autoTracingSegmentTimer * options.speed));
                autoTracingSegmentTimer = Mathf.MoveTowards(autoTracingSegmentTimer, 0, Time.fixedDeltaTime);
            }
        }
        if (state == TracingState.animation)
        {
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
                currentSegmentPattern.movedDistance += options.speed * Time.fixedDeltaTime;
            }
        }
        if (state == TracingState.united)
        {
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
        }

        if (segmentIndex >= segmentPatterns.Count)
        {
            HandTracing.o.setEnabled(false);
            segmentIndex = 0;
            state++;
            onStateChanged?.Invoke(state);
        }
    }

    public void setHandTracing(bool value)
    {
        autoTracing = !value;
        HandTracing.o.setEnabled(value);
    }

}