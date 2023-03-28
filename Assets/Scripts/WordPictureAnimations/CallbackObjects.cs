using UnityEngine;
namespace KidLetters.WordPictureAnimations
{

    [System.Serializable]
    public class CallbackObjects
    {
        public CallbackActivation activation;
        public Object[] objects;


        static void setActive(Object obj, bool value)
        {
            if (obj is GameObject go)
                go.SetActive(value);
            else if (obj is Behaviour comp)
                comp.enabled = value;
            else Debug.LogError("UnSupported Object Type " + obj, obj);
        }
        static bool getActive(Object obj)
        {
            if (obj is GameObject go)
                return go.activeSelf;
            else if (obj is Behaviour comp)
                return comp.enabled;
            else Debug.LogError("UnSupported Object Type " + obj, obj);
            return false;
        }
        public void Invoke()
        {
            foreach (var x in objects)
            {
                switch (activation)
                {
                    case CallbackActivation.activate:
                        setActive(x, true);
                        break;
                    case CallbackActivation.reactivate:
                        setActive(x, false);
                        setActive(x, true);
                        break;
                    case CallbackActivation.deactivate:
                        setActive(x, false);
                        break;
                    case CallbackActivation.invertActivation:
                        setActive(x, getActive(x));
                        break;
                }
            }
        }
    }

    public enum CallbackActivation
    {
        reactivate,
        activate,
        deactivate,
        invertActivation,
        TERMINATE,
    }
}