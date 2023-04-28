using UnityEngine;

public abstract class Config<T> : ScriptableObject where T : Config<T>
{
    private static T _o;

    public static T o
    {
        get
        {
            if (_o == null)
            {
                _o = Resources.Load<T>(typeof(T).Name);
            }

            return _o;
        }
    }
}