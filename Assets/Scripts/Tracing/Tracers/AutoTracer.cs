namespace KidLetters.Tracing
{
    using System;
    using UnityEngine;

    public class AutoTracer : TracerBase
    {
        public float speed = 4;
        public float segmentDelay = 1;


        float segmentDelayTimer;

        // public void setup(TracingStage stage)
        // {
        //     this.stage = stage;

        //     stage.onSegmentChanged += onSegmentPatternChagned;

        // }

        private void onSegmentPatternChagned(Pattern obj)
        {
            segmentDelayTimer = segmentDelay;
        }

        private void FixedUpdate()
        {

        }


        public override void flush()
        {
            segmentDelayTimer = 0;
        }

        public override float getNewMovement(LetterSegmentFiller segment, float dt)
        {
            var currentMovement = segment.movedDistance;
            var movement = Mathf.Max(0, speed * dt - (segmentDelayTimer * speed));
            currentMovement += movement;
            segmentDelayTimer = Mathf.MoveTowards(segmentDelayTimer, 0, dt);
            return currentMovement;
        }
    }
}