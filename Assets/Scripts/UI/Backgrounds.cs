using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class Backgrounds : MonoBehaviour
{

    public static Backgrounds o;


    [SerializeField]
    Image colorImage;
    [SerializeField]
    Color[] colors;

    [SerializeField]
    float changeDuration = .5f;



    [SerializeField] PairList<BackgroundsList, Image[]> bgLists;

    private void Awake()
    {
        o = this;
    }

    private void Start()
    {
        changeRandomly(BackgroundsList.forHome);
    }

    public void changeRandomly(BackgroundsList list)
    {
        var randomColor = colors.getRandom();
        colorImage.DOColor(randomColor, changeDuration);
    }

}


public enum BackgroundsList
{
    ALL,
    forHome,
    forTracing,
}