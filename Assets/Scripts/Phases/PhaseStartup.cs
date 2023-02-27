using UnityEngine;

public class PhaseStartup : MonoBehaviour
{
    [SerializeField] Phase initialPhase;

    private void Start()
    {
        if (GameManager.o.testTracing)
        {
            var letter = LetterContainer.o.findLetter(LetterUtility.charToLetterId(GameManager.o.testTracingLetter));
            TracingPhase.o.setArgs(letter);
            Phase.change(TracingPhase.o);
        }
        else
            Phase.change(initialPhase);
    }
}