using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class HomeUI : MonoBehaviour
{
    public static HomeUI o;
    public Button backButton;


    private void Awake()
    {
        o = this;
        // backButton.gameObject.SetActive(false);
    }


    public void setBackButtonEnabled(bool value)
    {
        backButton.gameObject.SetActive(true);
        backButton.transform.localScale = !value ? Vector3.one : Vector3.zero;
        backButton.transform.DOScale(value ? 1 : 0, .5f).SetEase(Ease.InOutBack);
    }

}