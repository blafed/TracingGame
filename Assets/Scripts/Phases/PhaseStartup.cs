using UnityEngine;

namespace KidLetters
{
    public class PhaseStartup : MonoBehaviour
    {
        [SerializeField] Phase initialPhase;

        private void Start()
        {
            if (GameManager.o.testTracing)
            {
                var letter = LetterContainer.o.findLetter(LetterUtility.charToLetterId(GameManager.o.testTracingLetter));
                TracingPhase.o.setArgs(letter, WordList.o.getWordByStartingLetter(letter.letterId));
                Phase.change(TracingPhase.o);
            }
            else
                Phase.change(HomePhase.o);
        }
    }
}