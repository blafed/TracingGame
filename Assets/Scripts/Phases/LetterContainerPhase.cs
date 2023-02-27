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
        LetterContainer.o.showAll().OnComplete(() => isReady = true);
        Backgrounds.o.changeRandomly(BackgroundsList.forHome);
    }
    protected override void onExit()
    {
        LetterContainer.o.hideAll(x => x == letter);
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
        this.letter = letter;
        var word = WordList.o.getRandomContains(letter.letterId);
        if (word == null)
        {
            Debug.LogError("No Word for this letter  " + LetterUtility.letterToString(letter.letterId));
            return false;
        }
        SelectedLetterPhase.o.setArgs(letter, word);
        change(SelectedLetterPhase.o);

        return true;
    }

}