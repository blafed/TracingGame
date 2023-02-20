using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.Sprites;
using System.Collections.Generic;


public class Letter : MonoBehaviour
{
    public List<Path> paths = new List<Path>();


    private void Awake()
    {
        var pathProviders = GetComponentsInChildren<IPathProvider>();
        for (int i = 0; i < pathProviders.Length; i++)
        {
            paths.Add(pathProviders[i].path);


        }
    }
}
