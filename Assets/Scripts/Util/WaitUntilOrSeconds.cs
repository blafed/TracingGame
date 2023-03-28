using System;
using UnityEngine;
public class WaitUntilOrSeconds : CustomYieldInstruction
{



    public float seconds;
    public Func<bool> predicate;
    public bool unscaled;


    double startTime;


    double getTime(bool unscaled)
    {
        return unscaled ? Time.unscaledTimeAsDouble : Time.timeAsDouble;
    }

    public WaitUntilOrSeconds(float seconds, Func<bool> predicate, bool unscaled = false)
    {
        this.seconds = seconds;
        this.predicate = predicate;
        this.unscaled = unscaled;
        startTime = getTime(unscaled);
    }

    public override bool keepWaiting => predicate() || getTime(unscaled) >= startTime + seconds;
}