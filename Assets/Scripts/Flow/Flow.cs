using UnityEngine;


[System.Serializable]
public class Flow
{

    protected float lifeTime;
    public float progress => evaluateProgress(lifeTime);
    public virtual bool isFinished => progress >= 1;

    protected virtual float evaluateProgress(float progress) => progress;
    public virtual void setProgress(float t)
    {

    }
    public virtual void update(float dt)
    {
        lifeTime += dt;
    }


    public void play()
    {

    }
}



[System.Serializable]
public class FlowSerial : Flow
{

}


[System.Serializable]
public class FlowParallel : Flow
{

}