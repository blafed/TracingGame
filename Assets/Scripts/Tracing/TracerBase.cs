using UnityEngine;


namespace KidLetters.Tracing
{
    public abstract class TracerBase : MonoBehaviour
    {
        public abstract float getIncrement(Pattern pattern);
    }
}