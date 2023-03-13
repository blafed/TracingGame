using UnityEngine;
using System.Collections;
namespace KidLetters
{
    using Tracing;

    public class TracingPhase : Phase<TracingPhase>
    {
        public Letter letter { get; private set; }
        public WordInfo wordInfo { get; private set; }
        public int patternIndex { get; private set; }



        public void setArgs(Letter letter, WordInfo wordInfo)
        {
            this.letter = letter;
            this.wordInfo = wordInfo;
        }

        protected override void onEnter()
        {
            StartCoroutine(cycle());
        }


        IEnumerator cycle()
        {
            yield return Tracing.FocusOnLetter.o.play();

            for (int i = 0; i < 3; i++)
            {
                // yield return Tracing.TracingRunner.o.play();
            }
        }



    }
}