using UnityEngine;

namespace KidLetters
{
    using Home;
    public class HomePhase : Phase<HomePhase>
    {

        Letter highlightCompletedLetter;


        public void setArgs(Letter highlightCompletedLetter)
        {
            this.highlightCompletedLetter = highlightCompletedLetter;
        }


        protected override void onEnter()
        {
            LetterContainer.o.adjustCamera();
            LetterContainer.o.setActiveLetters(true);
            Backgrounds.o.changeRandomly(BackgroundsList.forHome);
        }

    }
}