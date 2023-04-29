using System.Collections.Generic;
using UnityEngine;

namespace KidLetters
{
    public class LetterFiller : MonoBehaviour
    {


        [SerializeField] GameObject segmentFillerPrefab;
        public Letter letter { get; private set; }
        public float totalLength { get; private set; }
        public float progress
        {
            get => (movedDistance / totalLength).clamp01();
            private set => movedDistance = value.clamp01() * totalLength;
        }
        public float movedDistance { get; private set; }

        public int activeSegmentIndex
        {
            get
            {
                var index = 0;
                var d = movedDistance;
                foreach (var x in segmentFillers)
                {
                    if (d < x.pathLength)
                        return index;
                    d -= x.pathLength;
                    index++;
                }
                return index;
            }
        }
        public LetterSegmentFiller activeSegment => segmentFillers.getOrDefault(activeSegmentIndex);
        // public float activePathLength => activeSegment.pathLength;
        // public float activeMovedDistance => activeSegment.movedDistance;


        List<LetterSegmentFiller> segmentFillers = new List<LetterSegmentFiller>();

        public void setup(Letter letter)
        {
            this.letter = letter;
            totalLength = 0;
            var segFillerObj = segmentFillerPrefab;
            foreach (var x in letter.segments)
            {

                totalLength += x.totalLength;
                if (!segFillerObj)
                {
                    throw new System.Exception("LetterFiller: No LetterSegmentFiller found in children of LetterFiller");
                }
                var segFiller = Instantiate(segFillerObj).GetComponent<LetterSegmentFiller>();
                segFiller.transform.parent = transform;
                segFiller.transform.localPosition = x.transform.localPosition;
                // var filler = x.gameObject.AddComponent(segmentFillerType) as LetterSegmentFiller;
                segFiller.setup(this, x);
                segmentFillers.Add(segFiller);
            }

        }

        private void OnDestroy()
        {
            foreach (var x in segmentFillers)
                Destroy(x);
        }

        private void OnEnable()
        {
            foreach (var x in segmentFillers)
                x.gameObject.SetActive(true);
        }
        private void OnDisable()
        {
            foreach (var x in segmentFillers)
                x.gameObject.SetActive(false);
        }

        public void setTotalMovedDistance(float movedDistance)
        {
            foreach (var x in segmentFillers)
            {
                x.movedDistance = Mathf.Clamp(movedDistance, 0, x.pathLength);
                movedDistance -= x.pathLength;
            }
        }
        public void setTotalProgress(float totalProgress)
        {
            setTotalMovedDistance(totalProgress * totalLength);
        }

        public void setColor(Color color)
        {
            foreach (var x in segmentFillers)
                x.setColor(color);
        }
        public void setAlpha(float alpha)
        {
            foreach (var x in segmentFillers)
                x.setAlpha(alpha);
        }
    }
}