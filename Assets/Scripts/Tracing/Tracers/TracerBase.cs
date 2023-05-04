using UnityEngine;


namespace KidLetters.Tracing
{
    public abstract class TracerBase : MonoBehaviour
    {
        protected virtual void Awake() { }


        public abstract float getNewMovement(LetterSegmentFiller segment, float dt);
        public abstract void flush();
    }
}