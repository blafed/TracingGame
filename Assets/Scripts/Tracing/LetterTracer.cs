using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete]
public class LetterTracer : MonoBehaviour
{

    [System.Serializable]
    class EdgePointAnimation
    {
        public float duration = .5f;
        public float delay = .2f;
    }

    public bool isDone => isPostProgress && segmentIndex >= letter.segmentCount;


    public float speed = 2;
    public bool autoProgress = true;

    public GameObject patternPrefab;
    public Letter letter;
    public GameObject edgePointPrefab;
    [SerializeField] EdgePointAnimation edgePointAnimation;

    /// <summary>
    /// position in world wher the edge points are coming from
    /// </summary>
    public Vector2 starterPosition { get; set; } = Vector2.down * 10;

    int segmentIndex { get; set; }
    // public float progress { get; set; }
    Pattern currentPattern { get; set; }


    LetterSegment segment => letter.get(segmentIndex);
    List<Pattern> segmentPatterns = new List<Pattern>();
    List<EdgePoint> edgePoints = new List<EdgePoint>();


    bool isPostProgress;

    float progressIncrement => speed / segment.totalLength;

    bool isEdgePointsCreated;

    // private void Start()
    // {
    //     transform.position = letter.transform.position;
    //     letter.text.DOFade(0, edgePointAnimation.duration);
    //     // letter.setTextEnabled(false);
    //     var edgePointNum = 2 * letter.segmentCount;
    //     for (int i = 0; i < letter.segmentCount; i++)
    //     {
    //         var seg = letter.get(i);
    //         for (int j = 0; j < 2; j++)
    //         {
    //             var e = Instantiate(this.edgePointPrefab).GetComponent<EdgePoint>();
    //             e.transform.parent = transform;
    //             var pos = seg.position + (j == 0 ? seg.path.startPoint : seg.path.endPoint);
    //             e.transform.position = starterPosition;
    //             var tw = e.transform.DOMove(pos, edgePointAnimation.duration).SetDelay(edgePointAnimation.delay)
    //             .SetEase(Ease.OutCubic);
    //             if (i == letter.segmentCount - 1 && j == 1)
    //                 tw.OnComplete(() => isEdgePointsCreated = true);
    //             edgePoints.Add(e);
    //             e.setPlaying(false);
    //         }
    //     }

    // }
    // private void Update()
    // {
    //     if (isEdgePointsCreated)
    //     {
    //         if (!isPostProgress)
    //         {
    //             updateProgress();
    //         }
    //         else
    //         {
    //             updatePostProgress();
    //         }
    //     }
    // }

    // void updateProgress()
    // {
    //     if (!currentPattern)
    //     {
    //         beginSegmentTracing();
    //     }
    //     else
    //     {
    //         currentPattern.transform.position = letter.transform.position;
    //     }

    //     // progress +=  progressIncrement * Time.deltaTime;
    //     if (autoProgress)
    //         currentPattern.progress += progressIncrement * Time.deltaTime;
    //     if (currentPattern.isFinished)
    //     {
    //         // progress = 0;

    //         setEdgePointsPlaying(segmentIndex, false);

    //         segmentIndex++;
    //         currentPattern = null;
    //         if (segmentIndex >= letter.segmentCount)
    //             beginPostProgress();

    //         // Destroy(currentPattern.gameObject);
    //     }
    // }
    // void beginPostProgress()
    // {
    //     segmentIndex = 0;
    //     isPostProgress = true;

    // }
    // void updatePostProgress()
    // {
    //     if (segmentIndex >= segmentPatterns.Count)
    //         return;
    //     currentPattern = segmentPatterns[segmentIndex];
    //     if (!currentPattern.isPostProgress)
    //     {
    //         currentPattern.progress = 0;
    //         currentPattern.isPostProgress = true;
    //         setEdgePointsPlaying(segmentIndex, true);
    //         currentPattern.onPostProgressStart();
    //     }
    //     currentPattern.progress += progressIncrement * Time.deltaTime;
    //     if (currentPattern.isFinished)
    //     {
    //         currentPattern.onPostProgressEnd();
    //         setEdgePointsPlaying(segmentIndex, false);
    //         segmentIndex++;
    //     }
    // }


    // void setEdgePointsPlaying(int segmentIndex, bool value)
    // {
    //     for (int i = 0; i < 2; i++)
    //     {
    //         var index = i + segmentIndex * 2;
    //         edgePoints[index].setPlaying(value);
    //     }
    // }


    // void beginSegmentTracing()
    // {
    //     currentPattern = Instantiate(patternPrefab).GetComponent<Pattern>();
    //     currentPattern.transform.position = letter.transform.position;
    //     currentPattern.segment = segment;
    //     currentPattern.progress = 0;
    //     currentPattern.transform.parent = transform;
    //     currentPattern.transform.position = Vector3.one * 10000;
    //     segmentPatterns.Add(currentPattern);

    //     setEdgePointsPlaying(segmentIndex, true);
    //     // currentPattern.initSegment(segment);
    // }


}