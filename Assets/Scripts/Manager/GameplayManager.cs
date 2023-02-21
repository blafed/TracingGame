using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager o { get; private set; }

    private void Awake()
    {
        o = this;
    }


    public static void enter(int letter)
    {

    }
}