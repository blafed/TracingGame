using UnityEngine;

public abstract class PhaseSingletonEntity<T, P> : Singleton<T> where T : PhaseSingletonEntity<T, P> where P : Phase
{

    protected virtual void Start()
    {
        var phase = Phase.findPhase<P>();
        phase.onEnterEvent += onPhaseEnter;
        phase.onExitEvent += onPhaseExit;
    }

    protected virtual void onPhaseEnter() { }
    protected virtual void onPhaseExit() { }
}