using UnityEngine;

namespace KidLetters
{
    using Home;
    public class HomePhase : Phase<HomePhase>
    {
        public LetterRaw selectedLetter { get; private set; }
        LetterRaw highlightCompletedLetter;


        public void setArgs(int letterId)
        {
            this.highlightCompletedLetter = LetterContainer.o.getLetter(letterId);
        }


        public void selectLetter(LetterRaw letter)
        {
            if (this.selectedLetter)
                return;
            this.selectedLetter = letter;
            PronouncingPhase.o.setArgs(letter.letterId);
            Phase.change(PronouncingPhase.o);
        }


        protected override void onEnter()
        {
            selectedLetter = null;
            LetterContainer.o.adjustCamera();
            LetterContainer.o.setActiveLetters(true);
            Backgrounds.o.changeRandomly(BackgroundsList.forHome);
            HomeUI.o.setPreviewerButtonEnabled(true);
            HomeUI.o.setBackButtonEnabled(false);
        }
        protected override void onExit()
        {
            HomeUI.o.setBackButtonEnabled(true);
            HomeUI.o.setPreviewerButtonEnabled(false);
        }

    }
}