using UnityEngine;
using System.Collections.Generic;
public class TracingManager : MonoBehaviour
{
    [System.Serializable]
    public class Options
    {
        public float speed = 2;
        public float cameraZoom = 5;
        public GameObject edgePointPrefab;
        public List<Pattern> patternObjects = new List<Pattern>();

    }
    public static TracingManager o { get; private set; }


    public Vector2? spawnEdgePointsFrom { get; set; }

    [SerializeField]
    public Options options = new Options();
    public bool manualProgress = false;

    Letter currentLetter;

    List<Pattern> segmentPatterns = new List<Pattern>();
    int segmentIndex;

    public Pattern currentSegmentPattern
    {
        get => segmentIndex >= segmentPatterns.Count ? null : segmentPatterns[segmentIndex];
    }
    public bool isDone
    {
        get
        {
            return segmentPatterns.Count == 0 || segmentPatterns.TrueForAll(x => x.isDone);
        }
    }




    // public Letter letter;

    private void Awake()
    {
        o = this;
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


    public void startTracing(Letter letter)
    {
        if (currentLetter)
        {
            leave();
        }
        CameraControl.o.move(letter.transform.position);
        CameraControl.o.zoom(options.cameraZoom);
        this.currentLetter = letter;
        this.currentLetter.setTextEnabled(false);
    }
    public void setTracingPattern(PatternCode patternCode)
    {
        if (!currentLetter)
        {
            Debug.LogError("Current Letter is not set", gameObject);
            return;
        }
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
            segmentPatterns.Add(pattern);
            pattern.gameObject.SetActive(false);
        }
    }

    bool hasSegmentChanged = true;
    private void FixedUpdate()
    {
        if (!isDone)
        {
            if (hasSegmentChanged)
            {
                currentSegmentPattern.gameObject.SetActive(true);
                currentSegmentPattern.progress = 0;
                currentSegmentPattern.state++;
                hasSegmentChanged = false;
            }
            if (!manualProgress)
                currentSegmentPattern.movedDistance += options.speed * Time.fixedDeltaTime;
            if (currentSegmentPattern.progress >= 1 || currentSegmentPattern.isDone)
            {
                hasSegmentChanged = true;
                segmentIndex++;
                if (segmentIndex >= segmentPatterns.Count)
                    segmentIndex = 0;
            }
        }
    }

}