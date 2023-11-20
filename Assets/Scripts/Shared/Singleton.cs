using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T o { get; private set; }

    protected virtual void Awake()
    {
        o = (T)this;
    }
    
}