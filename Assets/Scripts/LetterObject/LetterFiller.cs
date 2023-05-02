using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace KidLetters
{
    public class LetterFiller : MonoBehaviour
    {


        [SerializeField] GameObject segmentFillerPrefab;


        public Glyph glyph { get; private set; }

        public Rect relativeViewRect => glyph.relativeRect;
        public float width { get; private set; }
        public float totalLength => glyph.totalLength;
        public float progress
        {
            get => (totalMovedDistance / totalLength).clamp01();
        }
        public float totalMovedDistance
        {
            get
            {
                float d = 0;
                foreach (var x in segmentFillers)
                {
                    d += x.movedDistance;
                }
                return d;
            }
        }

        public int segmentCount => glyph.segmentCount;

        public int activeSegmentIndex
        {
            get
            {
                var index = 0;
                var d = totalMovedDistance;
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
        bool _didSetup;


        public void setup(Glyph glyph)
        {
            setup(glyph, segmentFillerPrefab);
        }
        public void setup(Glyph glyph, GameObject segmentFillerPrefab)
        {
            _didSetup = true;
            this.glyph = glyph;

            if (!segmentFillerPrefab)
            {
                Debug.LogError("LetterFiller: no segmentFillerPrefab is set", this);
                return;
            }
            swapSegments(segmentFillerPrefab);

            setNormalWidth();
            setColor(Color.white);
            setAlpha(1);
        }


        private void Start()
        {
            if (!_didSetup)
                Debug.LogError("LetterFiller: setup() was not called", this);
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

        [ContextMenu("Swap to Random pattern")]
        void swapTo()
        {
            var randomPattern = (PatternCode)UnityEngine.Random.Range(1, (int)PatternCode.COUNT - 2);
            randomPattern = PatternCode.sketch;
            swapSegments(TracingConfig.o.getPatternPrefab(randomPattern), true);
            print("swapped to " + randomPattern);
        }
        [ContextMenu("Unswapping")]
        void unsawp()
        {
            unswapping = true;
            swapSegments(this.segmentFillerPrefab, true);
        }

        bool unswapping;

        public void swapSegments(GameObject segmentFillerPrefab, bool removeCurrent = true)
        {
            float currentMovementDistance = totalMovedDistance;
            if (removeCurrent)
                foreach (var x in segmentFillers)
                {
                    Destroy(x.gameObject);
                }


            segmentFillers.Clear();
            foreach (var x in glyph)
            {
                var segFiller = Instantiate(segmentFillerPrefab).GetComponent<LetterSegmentFiller>();
                segFiller.transform.parent = transform;
                segFiller.transform.localPosition = x.offset;
                segFiller.setup(this, x);
                segmentFillers.Add(segFiller);
            }
            setTotalMovedDistance(currentMovementDistance);


            foreach (var x in segmentFillers)
            {
                x.onMoved();
            }
        }
        public List<LetterSegmentFiller> createBasedSegments(GameObject segmentFillerPrefab)
        {
            var segmentFillers = new List<LetterSegmentFiller>();
            foreach (var x in glyph)
            {
                var segFiller = Instantiate(segmentFillerPrefab).GetComponent<LetterSegmentFiller>();
                segFiller.transform.localPosition = x.offset;
                segFiller.setup(this, x);
                segmentFillers.Add(segFiller);
            }
            return segmentFillers;
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
        public LetterSegmentFiller getSegment(int index)
        {
            return segmentFillers[index];
        }

        public void setColor(Color color)
        {
            _tweenColor = color;
            foreach (var x in segmentFillers)
                x.setColor(color);
        }
        public void setAlpha(float alpha)
        {
            _tweenAlpha = alpha;

            foreach (var x in segmentFillers)
                x.setAlpha(alpha);
        }
        public void setWidth(float width)
        {
            _tweenWidth = width;
            this.width = width;
            foreach (var x in segmentFillers)
                x.onWidthChanged();
        }
        public void setNormalWidth()
        {
            setWidth(LetterObjectConfig.o.width);
        }
        public void setEnabled(bool value)
        {
            gameObject.SetActive(value);
        }







        Color _tweenColor = Color.white;
        float _tweenAlpha = 1;
        float _tweenWidth = .5f;



        Tween _doColorTween;
        Tween _doFadeTween;
        Tween _doWidthTween;



        public Tween doColor(Color color, float duration)
        {
            return _doColorTween = DOTween.To(() => _tweenColor, x => setColor(x), color, duration);
        }
        public Tween doFade(float alpha, float duration)
        {
            return _doFadeTween = DOTween.To(() => _tweenAlpha, x => setAlpha(x), alpha, duration);
        }
        public Tween doWidth(float width, float duration)
        {
            return _doWidthTween = DOTween.To(() => _tweenWidth, x => setWidth(x), width, duration);
        }
        public void doKill()
        {
            this.DOKill();

            if (_doColorTween != null)
                _doColorTween.Kill();

            if (_doFadeTween != null)
                _doFadeTween.Kill();

            if (_doWidthTween != null)
                _doWidthTween.Kill();
        }



        public static LetterFiller createFiller(LetterRaw basedLetter, GameObject prefab)
        {
            var letterFiller = Instantiate(prefab, basedLetter.transform.position, default).GetComponent<LetterFiller>();
            letterFiller.setup(basedLetter.generateGlyph());
            letterFiller.setColor(Color.white);
            letterFiller.setAlpha(1);
            letterFiller.setTotalProgress(1);
            letterFiller.setWidth(LetterObjectConfig.o.width);
            return letterFiller;
        }
        public static LetterFiller createStandardFiller(LetterRaw basedLetter)
        {
            return createFiller(basedLetter, LetterObjectConfig.o.standardLetterFillerPrefab);
        }


    }
}