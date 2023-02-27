using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager o;

    public GameState state { get; }
    public bool testTracing = true;
    public char testTracingLetter = 'a';

    private void Awake()
    {
        o = this;
    }
    private void Start()
    {
    }





}

public enum GameState
{
    letterContainer,
    enteringLetter,
}