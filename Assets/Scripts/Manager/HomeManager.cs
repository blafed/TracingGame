using UnityEngine;

public class HomeManager : MonoBehaviour
{
    public static HomeManager o { get; private set; }
    private void Awake()
    {
        o = this;
    }
}