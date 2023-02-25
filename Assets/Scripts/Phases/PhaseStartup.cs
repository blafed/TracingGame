using UnityEngine;

public class PhaseStartup : MonoBehaviour
{
    [SerializeField] Phase initialPhase;

    private void Start()
    {
        Phase.change(initialPhase);
    }
}