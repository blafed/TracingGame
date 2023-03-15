using UnityEngine;

namespace KidLetters
{
    using Home;
    public class HomePhase : Phase<HomePhase>
    {
        public Letter selectedLetter { get; private set; }
        Letter highlightCompletedLetter;


        public void setArgs(Letter highlightCompletedLetter)
        {
            this.highlightCompletedLetter = highlightCompletedLetter;
        }


        public void selectLetter(Letter letter)
        {
            if (this.selectedLetter)
                return;
            this.selectedLetter = letter;
            PronouncingPhase.o.setArgs(letter);
            Phase.change(PronouncingPhase.o);
        }


        protected override void onEnter()
        {
            selectedLetter = null;
            LetterContainer.o.adjustCamera();
            LetterContainer.o.setActiveLetters(true);
            Backgrounds.o.changeRandomly(BackgroundsList.forHome);
            HomeUI.o.setBackButtonEnabled(false);
        }
        protected override void onExit()
        {
            HomeUI.o.setBackButtonEnabled(true);
        }

    }
}