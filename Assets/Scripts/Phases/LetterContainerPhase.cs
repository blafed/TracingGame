using DG.Tweening;
using UnityEngine;

public class LetterContainerPhase : Phase<LetterContainerPhase>
{

    bool isReady;
    Letter letter;
    protected override void onEnter()
    {
        isReady = false;
        this.letter = null;
        LetterContainer.o.adjustCamera();
        LetterContainer.o.showAllNoTween();
        isReady = true;
        //.OnComplete(() => isReady = true);
        Backgrounds.o.changeRandomly(BackgroundsList.forHome);
        HomeUI.o.setBackButtonEnabled(false);
    }
    protected override void onExit()
    {
        LetterContainer.o.hideAllNoTween(x => x == letter);
        HomeUI.o.setBackButtonEnabled(true);
    }





    /// <summary>
    /// callback on select letter, being called from LetterContainer
    /// </summary>
    /// <param name="letter"></param>
    /// <returns>true if letter selection is success, false otherwise</returns>
    public bool onSelectLetter(Letter letter)
    {
        if (!isReady)
            return false;
        if (this.letter)
            return false;
        var word = WordList.o.getWordByStartingLetter(letter.letterId);
        if (word == null)
        {
            Debug.LogError("No Word for this letter  " + LetterUtility.letterToString(letter.letterId));
            return false;
        }
        this.letter = letter;
        SelectedLetterPhase.o.setArgs(letter, word);
        change(SelectedLetterPhase.o);

        return true;
    }

}