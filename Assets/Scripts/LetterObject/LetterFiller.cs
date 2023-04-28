using System.Collections.Generic;
using UnityEngine;

namespace KidLetters
{
    public class LetterFiller : MonoBehaviour
    {
        public Letter letter { get; private set; }
        public float totalLength { get; private set; }
        public float progress
        {
            get => (movedDistance / totalLength).clamp01();
            private set => movedDistance = value.clamp01() * totalLength;
        }
        public float movedDistance { get; private set; }

        public int activeFillerIndex
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
        public LetterSegmentFiller activeFiller => segmentFillers.getOrDefault(activeFillerIndex);

        List<LetterSegmentFiller> segmentFillers = new List<LetterSegmentFiller>();

        public void setup(Letter letter)
        {
            this.letter = letter;
            totalLength = 0;
            var segFillerObj = GetComponentInChildren<LetterSegmentFiller>().gameObject;
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

            segFillerObj.SetActive(false);

        }

        private void OnDestroy()
        {
            foreach (var x in segmentFillers)
                Destroy(x);
        }

        public void setTotalMovedDistance(float movedDistance)
        {
            foreach (var x in segmentFillers)
            {
                if (movedDistance < x.pathLength)
                {
                    x.movedDistance = Mathf.Clamp(movedDistance, 0, x.pathLength);
                    break;
                }
                movedDistance -= x.pathLength;
            }
        }
    }
}