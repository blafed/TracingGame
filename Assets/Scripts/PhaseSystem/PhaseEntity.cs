using UnityEngine;

public abstract class PhaseEntity : MonoBehaviour
{
    protected virtual void Awake() { }
    protected virtual void Start()
    {
        register();
    }


    protected abstract void register();
    public virtual void onPhaseEnter() { }
    public virtual void onPhaseExit() { }
}