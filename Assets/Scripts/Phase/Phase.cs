using UnityEngine;

public class Phase : MonoBehaviour
{
    public static Phase current { get; private set; }
    protected static Phase last { get; private set; }
    protected virtual void onEnter() { }
    protected virtual void onExit() { }

    public static void change(Phase other)
    {
        if (current)
        {
            current.onExit();
        }
        last = current;
        current = other;
        current.onEnter();
    }



}

public abstract class Phase<T> : Phase where T : Phase<T>
{
    public static T o { get; private set; }

    private void Awake()
    {
        o = (T)this;
    }
}