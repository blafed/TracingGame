using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace KidLetters.Tracing
{
    public class StageButton : MonoBehaviour
    {
        [SerializeField] Sprite autoButtonSprite;
        [SerializeField] Sprite doneButtonSprite;
        [SerializeField] Sprite initialButtonSprite;
        // public int index { get; set; }
        // Button _button;
        // public Button button => this.cachedComponent(ref _button);


        // public bool isDone { get; private set; }



        // bool lastEnabled = false;

        // private void Awake()
        // {
        //     button.onClick.AddListener(() =>
        //     {
        //         transform.DOPunchScale(.2f.vector(), .2f);
        //         TracingPhase.o.playStage(index, this);
        //     });
        // }


        // public void prepare(int index)
        // {
        //     this.index = index;
        //     setAutoSprite();
        //     isDone = false;
        // }


        // // public void init(int index)
        // // {
        // //     this.index = index;
        // //     setAutoSprite();
        // //     isDone = false;
        // // }


        // public void punch()
        // {
        //     TracingPhase.o.stageButton.transform.DOPunchScale(.2f.vector(), .2f);
        // }
        // [System.Obsolete]
        // public void setDone()
        // {
        //     punch();
        //     button.image.sprite = doneButtonSprite;
        //     isDone = true;
        // }
        // [System.Obsolete]
        // public void setAutoSprite()
        // {
        //     button.image.sprite = autoButtonSprite;
        // }
        // [System.Obsolete]
        // public void setNormalSprite()
        // {
        //     button.image.sprite = initialButtonSprite;
        // }

        // public void refresh()
        // {


        //     // var isCurrent = TracingPhase.o.playingDoneIndex == index;
        //     var isEnabled = TracingPhase.o.doneStage >= index;
        //     var isDone = TracingPhase.o.doneStage >= index + 1;
        //     gameObject.SetActive(TracingPhase.o.stageInfos.Length > index);
        //     var options = TracingPhase.o.stageInfos[index];
        //     var isAuto = options.autoTracing;
        //     button.interactable = isEnabled;
        //     button.image.sprite = !isEnabled ? initialButtonSprite : isDone ? doneButtonSprite : autoButtonSprite;

        //     if (lastEnabled != isEnabled)
        //     {
        //         if (isEnabled)
        //         {
        //             transform.DOPunchScale(.2f.vector(), .2f);
        //         }
        //     }


        //     lastEnabled = isEnabled;
        //     // button.image.sprite = isCurrent ? (isAuto ? autoButtonSprite : initialButtonSprite) : isDone ? doneButtonSprite : initialButtonSprite;
        // }
    }
}