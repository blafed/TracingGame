using UnityEngine.UI;
using UnityEngine;

public class Backgrounds : MonoBehaviour
{

    public static Backgrounds o;




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

        var li = list == BackgroundsList.ALL ? bgLists.items.getRandom().value : bgLists.get(list);
        foreach (var x in bgLists)
            foreach (var y in x.value)
                y.gameObject.SetActive(false);
        li.getRandom().gameObject.SetActive(true);
    }
}


public enum BackgroundsList
{
    ALL,
    forHome,
    forTracing,
}