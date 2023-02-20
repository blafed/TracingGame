using System.Collections.Generic;
using UnityEngine;

public class LetterTracer : MonoBehaviour
{
    public Letter letter;
    public TracingData data;
    public Transform obj;
    public float speed = 2;



    private void Start()
    {
        startTracing(letter);
    }
    private void Update()
    {
        if (letter != null)
        {
            var splineCount = letter;

            // if (data.splineIndex < splineCount)
            // {
            //     if (data.completed < 1)
            //     {
            //         traceInput();
            //         moveObjAlongPath();
            //     }
            //     else
            //     {
            //         data.splineIndex++;
            //     }
            // }
        }
    }


    void traceInput()
    {

    }
    void moveObjAlongPath()
    {
    }


    public void startTracing(Letter target)
    {
        this.letter = target;
        data = new();
    }

}

[System.Serializable]
public class TracingData
{
    public int splineIndex;
    public float completed;
}