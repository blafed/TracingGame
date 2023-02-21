using UnityEngine;

public class LetterContainer : MonoBehaviour
{
    public static LetterContainer o;


    public FlowList<LetterItem> letters;

    private void Awake()
    {
        o = this;

        letters.iterate(LetterUtility.sizeSet, x =>
        {
            x.component.init(x.iterationIndex);
        });
    }



    public void enter() { }
    public void exit() { }



}