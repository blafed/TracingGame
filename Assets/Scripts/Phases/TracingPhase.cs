using System.Collections;
using UnityEngine;

public class TracingPhase : Phase<TracingPhase>
{
    Letter argLetter;

    PatternCode[] patternArray = new PatternCode[3];
    int nextPlayPatternIndex = 0;


    public void playCustomPattern(PatternCode pattern, Vector2? spawnEdgePointsFrom = null)
    {
        TracingManager.o.startTracing(argLetter);
        TracingManager.o.setTracingPattern(pattern);
        TracingManager.o.spawnEdgePointsFrom = spawnEdgePointsFrom;

    }
    public void playPattern(int index)
    {
        if (index >= patternArray.Length)
        {
            return;
        }

        StopAllCoroutines();
        playCustomPattern(patternArray[index]);
    }
    public void playNextPattern()
    {
        playPattern(nextPlayPatternIndex++);
    }


    public void setArgs(Letter letter)
    {
        argLetter = letter;


    }


    protected override void onEnter()
    {
        nextPlayPatternIndex = 0;
        for (int i = 0; i < patternArray.Length; i++)
        {
            patternArray[i] = (PatternCode)Random.Range(1, (int)PatternCode.COUNT);
        }
        StartCoroutine(cycle());
        TracingPanelUI.o.show();
    }
    protected override void onExit()
    {
        StopAllCoroutines();
        TracingPanelUI.o.hide();
    }




    IEnumerator cycle()
    {

        //TODO
        yield break;
        // tracer = TracingManager.o.startTracing(argLetter, PatternCode.sketch);
        // yield return new WaitUntil(() => tracer.isDone);
    }

}