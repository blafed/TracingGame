using UnityEngine;

namespace KidLetters
{

    [CreateAssetMenu(fileName = "TracingConfig", menuName = "KidLetters/TracingConfig", order = 0)]
    public class TracingConfig : Config<TracingConfig>
    {
        public float edgePointRadius = .575f;
        public GameObject edgePointPrefab;

        [SerializeField]
        PairList<PatternCode, GameObject> patternPrefabs = new PairList<PatternCode, GameObject>();


        public GameObject getPatternPrefab(PatternCode code)
        {
            return patternPrefabs.get(code);
        }
    }
}


public enum PatternCode
{
    none,
    chains,
    road,
    rainbow,
    butterfly,
    candy,
    sketch,
    brush,
    COUNT,
}