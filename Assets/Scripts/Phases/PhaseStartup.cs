using UnityEngine;

using KidLetters.Home;
namespace KidLetters
{
    public class PhaseStartup : MonoBehaviour
    {
        [SerializeField] Phase initialPhase;

        private void Start()
        {
            if (GameManager.o.testTracing)
            {
                var letterId = LetterUtility.charToLetterId(GameManager.o.testTracingLetter);
                TracingPhase.o.setArgs(letterId, WordList.o.getWordByStartingLetter(letterId));
                Phase.change(TracingPhase.o);
            }
            else
                Phase.change(HomePhase.o);
        }
    }
}