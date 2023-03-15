using System.Collections.Generic;
using UnityEngine;

public abstract class Phase : MonoBehaviour
{
    protected static List<Phase> allPhases = new List<Phase>();
    public static Phase current { get; private set; }
    protected static Phase last { get; private set; }


    public event System.Action onEnterEvent;
    public event System.Action onExitEvent;

    protected virtual void onEnter() { }
    protected virtual void onExit() { }

    public static void change(Phase other)
    {
        if (current)
        {
            current.onExit();
            current.clean();
            current.onExitEvent?.Invoke();
        }
        last = current;
        current = other;
        current.prepare();
        current.onEnter();
        current.onEnterEvent?.Invoke();
    }
    List<PhaseEntity> entities = new List<PhaseEntity>();


    public void registerEntity(PhaseEntity entity)
    {
        entities.Add(entity);
    }

    void clean()
    {
        foreach (var x in entities)
        {
            x.onPhaseExit();
        }
        for (int i = entities.Count - 1; i > -1; i--)
        {
            var x = entities[i];
            if (x)
                if (x is CreatedPhaseEntity)
                {
                    entities.RemoveAt(i);
                    Destroy(x.gameObject);
                }
        }

    }
    void prepare()
    {
        foreach (var x in entities)
            x.onPhaseEnter();
    }
    public T getEntity<T>() where T : PhaseEntity
    {
        foreach (var x in entities)
            if (x is T t)
                return t;
        return null;
    }


    public static T findPhase<T>() where T : Phase
    {
        return allPhases.Find(x => x is T) as T;
    }


}

public abstract class Phase<T> : Phase where T : Phase<T>
{
    public static T o { get; private set; }


    private void Awake()
    {
        o = (T)this;
        allPhases.Add(this);
    }
}