using UnityEngine;

public abstract class PhaseEntity : MonoBehaviour
{
    protected virtual void Awake() { }



    public virtual void onPhaseEnter() { }
    public virtual void onPhaseExit() { }
}