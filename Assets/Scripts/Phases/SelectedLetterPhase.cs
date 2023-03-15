using UnityEngine;
using System.Collections.Generic;
public class SelectedLetterPhase : Phase<SelectedLetterPhase>
{
    public bool skip = false;
    Letter argLetter;
    WordInfo argWordInfo;

    private void Start()
    {
        LetterEnterAnimation.o.onFinish += finish;

    }
    public void finish()
    {
        LetterEnterAnimation.o.stop();
        TracingPhase.o.setArgs(argLetter, null);
        change(TracingPhase.o);
    }
    protected override void onEnter()
    {
        LetterEnterAnimation.o.play(argLetter, argWordInfo);
        if (skip)
        {
            LetterEnterAnimation.o.stop();
            LetterEnterAnimation.o.clean();
            finish();
        }
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