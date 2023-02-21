using UnityEngine;

public abstract class Manager<T> : MonoBehaviour where T : Manager<T>
{
    protected virtual bool defaultEnabled => false;
    public static T o;
    private void Awake()
    {
        o = (T)this;
        init();
        enabled = defaultEnabled;
    }


    protected virtual void init() { }
}