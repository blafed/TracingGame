using System.Collections.Generic;
using UnityEngine;

public class LetterTracer : MonoBehaviour
{
    public float speed = 2;
    public bool autoProgress = true;
    public GameObject patternPrefab;
    public Letter letter;

    public int segmentIndex { get; set; }
    public float progress { get; set; }

    Pattern currentPattern { get; set; }


    public LetterSegment segment => letter.get(segmentIndex);



    private void Start()
    {
        startTracing(letter);
    }
    private void Update()
    {
        if (letter != null)
        {
            if (segmentIndex < letter.segmentCount)
            {
                if (!currentPattern)
                {
                    beginSegmentTracing();
                }
                whileSegmentTracing();

                if (autoProgress)
                    progress += speed / segment.totalLength * Time.deltaTime;
                if (progress >= 1)
                {
                    progress = 0;
                    segmentIndex++;
                    currentPattern = null;
                    // Destroy(currentPattern.gameObject);
                }
            }
        }
    }


    void beginSegmentTracing()
    {
        currentPattern = Instantiate(patternPrefab).GetComponent<Pattern>();
        currentPattern.transform.position = letter.transform.position;
        currentPattern.segment = segment;
        currentPattern.progress = 0;
        // currentPattern.initSegment(segment);


    }
    void whileSegmentTracing()
    {
        currentPattern.progress = progress;
        // currentPattern.setTrail(segment, progress);
    }


    public void startTracing(Letter target)
    {
        this.letter = target;
        this.segmentIndex = 0;
        this.progress = 0;
        target.setTextEnabled(value: false);
    }

}