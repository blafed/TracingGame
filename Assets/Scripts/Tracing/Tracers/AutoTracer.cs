namespace KidLetters.Tracing
{
    using System;
    using UnityEngine;

    public class AutoTracer : TracerBase
    {
        public float speed = 4;
        public float segmentDelay = 1;


        float segmentDelayTimer;

        public TracingStage stage { get; private set; }
        public void setup(TracingStage stage)
        {
            this.stage = stage;

            stage.onSegmentPatternChanged += onSegmentPatternChagned;

        }

        private void onSegmentPatternChagned(Pattern obj)
        {
            segmentDelayTimer = segmentDelay;
        }

        private void FixedUpdate()
        {
            var currentSegmentPattern = stage.currentSegmentPattern;
            var movement = Mathf.Max(0, speed * Time.fixedDeltaTime - (segmentDelayTimer * speed));
            currentSegmentPattern.movedDistance += movement;
            segmentDelayTimer = Mathf.MoveTowards(segmentDelayTimer, 0, Time.fixedDeltaTime);
        }
    }
}