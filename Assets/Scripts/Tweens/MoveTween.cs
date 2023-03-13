using DG.Tweening;
using UnityEngine;

public abstract class MoveTween : TweenBase
{
    public bool local;
    public bool add;
    [Header("Movement")]
    public Vector3 position = new Vector3(1, 1, 1);
    public Transform targetTransform;




}