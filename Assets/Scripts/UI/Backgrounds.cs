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


    int currentBackgroundIndex = 0;

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
        int randomIndex = 0;
        int _safeTries = 1000;
        do
        {
            if (_safeTries <= 0)
            {
                Debug.LogError("Backgrounds:changeRandomly: safe tries exceeded");
                break;
            }
            randomIndex = Random.Range(0, colors.Length);
            _safeTries--;
        } while (randomIndex == currentBackgroundIndex);
        currentBackgroundIndex = randomIndex;

        var randomColor = colors[randomIndex];
        colorImage.DOColor(randomColor, changeDuration);
    }


    public Color getBackgroundColor()
    {
        return colors[currentBackgroundIndex];
    }

}


public enum BackgroundsList
{
    ALL,
    forHome,
    forTracing,
}