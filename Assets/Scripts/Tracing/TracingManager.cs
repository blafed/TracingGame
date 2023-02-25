using UnityEngine;
using System.Collections.Generic;
public class TracingManager : MonoBehaviour
{
    public static TracingManager o { get; private set; }

    [SerializeField] List<Pattern> patternInfos;

    [SerializeField]
    GameObject tracerPrefab;
    LetterTracer currentTracer;
    Letter currentLetter;


    // public Letter letter;

    private void Awake()
    {
        o = this;
    }

    GameObject getPatternPrefab(PatternCode code)
    {
        return patternInfos.Find(x => x.code == code).gameObject;
        // return Resources.Load<GameObject>("Patterns/" + code.ToString().capitalize() + "Pattern");
    }



    public LetterTracer startTracing(Letter letter, PatternCode pattern)
    {
        if (currentTracer)
            Destroy(currentTracer.gameObject);
        currentLetter = letter;
        currentTracer = Instantiate(tracerPrefab).GetComponent<LetterTracer>();
        currentTracer.patternPrefab = getPatternPrefab(pattern);
        currentTracer.letter = letter;
        return currentTracer;
    }
}
