using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class TracingPhase : Phase<TracingPhase>
{
    [System.Serializable]
    public class PlayTracingOptions
    {
        public PatternCode code;
        public bool isAuto;
        public PlayTracingOptions() { }
        public PlayTracingOptions(PatternCode code, bool isAuto)
        {
            this.code = code;
            this.isAuto = isAuto;
        }
    }
    public enum State
    {
        ready,
        playing,
    }

    public State state { get; private set; }
    public int playingIndex { get; private set; }
    public int playingDoneIndex { get; private set; }



    public Letter argLetter { get; private set; }

    List<PlayTracingOptions> playTracingOptions = new List<PlayTracingOptions>(){
        new PlayTracingOptions(PatternCode.sketch, true),
        new PlayTracingOptions(PatternCode.chains, false),
        new PlayTracingOptions(PatternCode.rainbow, false),
    };


    public int playTracingOptionsCount => playTracingOptions.Count;



    [System.Obsolete]
    PatternCode[] patternArray = new PatternCode[3];
    [System.Obsolete]
    int nextPlayPatternIndex = 0;




    private void Start()
    {
        // TracingManager.o.onDone += onTracingDone;
        TracingManager.o.onSegmentPatternChanged += onSegmentPatternChanged;
        TracingManager.o.onStateChanged += onTracingStateChanged;
    }

    private void onTracingStateChanged(TracingState obj)
    {
        if (obj == TracingState.tracing)
            TracingPanelUI.o.showIndicating();
        else
            TracingPanelUI.o.hideIndicating();
        if (obj == TracingState.done)
        {
            TracingDoneStarEffect.o.startStarAnimation(() =>
            {
                if (playingDoneIndex < playingIndex)
                    playingDoneIndex = playingIndex;
            });

            nextPlayPatternIndex++;
        }
        TracingPanelUI.o.refresh();
    }

    private void onSegmentPatternChanged(Pattern obj)
    {

    }




    // private void onTracingDone()
    // {

    //     // TracingPanelUI.o.hideIndicating();
    // }

    public void setPlayTracingOptions(int index, PatternCode code, bool isAuto = false)
    {
        while (playTracingOptions.Count <= index)
            playTracingOptions.Add(new());
        playTracingOptions[index].code = code;
        playTracingOptions[index].isAuto = isAuto;
    }
    public PlayTracingOptions getPlayTracingOptions(int index)
    {
        return playTracingOptions[index];
    }


    public void playIndexedPattern(int index, Vector2? spawnEdgePointsFrom = null)
    {
        playingIndex = index;
        TracingManager.o.autoTracing = playTracingOptions[index].isAuto;
        playCustomPattern(playTracingOptions[index].code, spawnEdgePointsFrom);
    }
    public void playCustomPattern(PatternCode pattern, Vector2? spawnEdgePointsFrom = null)
    {
        TracingManager.o.startTracing(argLetter);
        TracingManager.o.setTracingPattern(pattern);
        TracingManager.o.spawnEdgePointsFrom = spawnEdgePointsFrom;

    }
    [System.Obsolete]
    public void playPattern(int index, Vector2? spawnEdgePointsFrom = null)
    {
        if (index >= patternArray.Length)
        {
            return;
        }

        StopAllCoroutines();
        playCustomPattern(patternArray[index], spawnEdgePointsFrom);
    }
    [System.Obsolete]
    public void playNextPattern(Vector2? spawnEdgePointsFrom = null)
    {
        playIndexedPattern(nextPlayPatternIndex, spawnEdgePointsFrom);
        nextPlayPatternIndex++;
    }


    public void setArgs(Letter letter)
    {
        argLetter = letter;
    }

    [System.Obsolete]
    protected override void onEnter()
    {

        playingIndex = -1;
        playingDoneIndex = -1;
        playTracingOptions.Clear();
        setPlayTracingOptions(0, PatternCode.sketch, true);
        List<PatternCode> availablePatterns = System.Linq.Enumerable.Range(1, (int)PatternCode.sketch - 1).Select(x => (PatternCode)x).ToList();

        for (int i = 0; i < 2; i++)
        {
            var patternCode = argLetter.getCustomPattern(i);
            if (patternCode == 0)
            {
                patternCode = availablePatterns.getRandom();
                availablePatterns.Remove(patternCode);
            }
            setPlayTracingOptions(1 + i, patternCode);
            // setPlayTracingOptions(1 + i, customPattern == 0 ? (PatternCode)Random.Range(1, (int)PatternCode.sketch) : customPattern);
        }

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
        TracingManager.o.clean();
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