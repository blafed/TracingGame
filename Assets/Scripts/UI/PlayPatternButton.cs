using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
public class PlayPatternButton : MonoBehaviour
{
    public PatternCode code;
    Button _button;
    public Button button => this.getComponentCached(ref _button);


    private void Awake()
    {
        button.onClick.AddListener(() =>
        {
            transform.DOPunchScale(.2f.vector(), .2f);
            TracingPhase.o.playCustomPattern(code, transform.position);
        });
    }
}