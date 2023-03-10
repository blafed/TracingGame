using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
public class PlayPatternButton : MonoBehaviour
{
    [SerializeField] Sprite autoButtonSprite;
    [SerializeField] Sprite doneButtonSprite;
    [SerializeField] Sprite initialButtonSprite;
    public int index;
    Button _button;
    public Button button => this.getComponentCached(ref _button);

    private void Awake()
    {
        button.onClick.AddListener(() =>
        {
            transform.DOPunchScale(.2f.vector(), .2f);
            TracingPhase.o.playIndexedPattern(index, transform.position);
            // TracingPhase.o.playNextPattern(transform.position);
            //TracingPhase.o.playCustomPattern(code, transform.position);
            // TracingPhase.o.clickButon(this);
            // TracingPhase.o.playIndexedPattern(index, transform.position);
        });
    }


    // public void setAuto()
    // {
    //     button.image.sprite = autoButtonSprite;
    // }
    // public void setDone()
    // {
    //     button.image.sprite = doneButtonSprite;
    // }
    // public void setInitial()
    // {
    //     button.image.sprite = initialButtonSprite;
    // }


    public void init(int index)
    {
        this.index = index;
    }



    public void refresh()
    {
        print("refresh is called");

        // var isCurrent = TracingPhase.o.playingDoneIndex == index;
        var isEnabled = TracingPhase.o.playingDoneIndex >= index - 1;
        var isDone = TracingPhase.o.playingDoneIndex >= index;
        gameObject.SetActive(TracingPhase.o.playTracingOptionsCount > index);
        var options = TracingPhase.o.getPlayTracingOptions(index);
        var isAuto = options.isAuto;
        button.interactable = isEnabled;
        button.image.sprite = isDone ? doneButtonSprite : isAuto ? autoButtonSprite : initialButtonSprite;
        // button.image.sprite = isCurrent ? (isAuto ? autoButtonSprite : initialButtonSprite) : isDone ? doneButtonSprite : initialButtonSprite;
    }
}