using System.Collections.Generic;
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
            current.clean();
        }
        last = current;
        current = other;
        current.onEnter();
    }
    List<PhaseEntity> entities = new List<PhaseEntity>();


    public void registerEntity(PhaseEntity entity)
    {
        entities.Add(entity);
    }


    void clean()
    {
        for (int i = entities.Count - 1; i > -1; i--)
        {
            var x = entities[i];
            if (x is ActivatedPhaseEntity)
                x.gameObject.SetActive(false);
            else if (x is CleanablePhaseEntity)
                ((CleanablePhaseEntity)x).clean();
            else if (x is CreatedPhaseEntity)
            {
                entities.RemoveAt(i);
                Destroy(x.gameObject);
            }
        }
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