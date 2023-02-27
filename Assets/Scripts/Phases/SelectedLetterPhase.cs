using UnityEngine;
using System.Collections.Generic;
public class SelectedLetterPhase : Phase<SelectedLetterPhase>
{
    Letter argLetter;
    WordInfo argWordInfo;

    private void Start()
    {
        LetterEnterAnimation.o.onFinish += finish;

    }
    public void finish()
    {
        LetterEnterAnimation.o.stop();
        TracingPhase.o.setArgs(argLetter);
        change(TracingPhase.o);
    }
    protected override void onEnter()
    {
        LetterEnterAnimation.o.play(argLetter, argWordInfo);
    }


    protected override void onExit()
    {
        LetterEnterAnimation.o.stop();
        LetterEnterAnimation.o.clean();
    }

    public void setArgs(Letter letter, WordInfo wordInfo)
    {
        argLetter = letter;
        argWordInfo = wordInfo;
    }
}