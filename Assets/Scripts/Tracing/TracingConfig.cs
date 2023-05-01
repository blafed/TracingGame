using UnityEngine;

namespace KidLetters
{

    [CreateAssetMenu(fileName = "TracingConfig", menuName = "KidLetters/TracingConfig", order = 0)]
    public class TracingConfig : Config<TracingConfig>
    {
        public float edgePointRadius = .575f;
        public GameObject edgePointPrefab;
        public GameObject sketchPatternPrefab;
    }
}