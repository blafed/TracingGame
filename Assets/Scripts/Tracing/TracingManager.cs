using UnityEngine;

public class TracingManager : MonoBehaviour
{
    public static TracingManager o { get; private set; }

    [SerializeField]
    GameObject edgePointPrefab;
    [SerializeField]
    GameObject tracerPrefab;
    LetterTracer currentTracer;


    public Letter letter;

    private void Awake()
    {
        o = this;
    }

    GameObject getPatternPrefab(PatternCode code)
    {
        return Resources.Load<GameObject>("Patterns/" + code.ToString().capitalize() + "Pattern");
    }


    public void startTracing(PatternCode pattern)
    {
        if (currentTracer)
            Destroy(currentTracer.gameObject);
        currentTracer = Instantiate(tracerPrefab).GetComponent<LetterTracer>();
        currentTracer.patternPrefab = getPatternPrefab(pattern);
        currentTracer.edgePointPrefab = edgePointPrefab;
        currentTracer.letter = letter;
    }
}
