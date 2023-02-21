using UnityEngine;
using DG.Tweening;
public class EnterGameplayManager : Manager<EnterGameplayManager>
{
    public float targetCameraAspectRatio = 5;
    public float focusTime = 1f;
    public float initialGlowTime = 1f;
    public float glowTimeRate = .5f;
    public int letter { get; set; }
    private void OnEnable()
    {

    }
    private void OnDisable()
    {

    }


    public void startLetter(int letter, Transform letterContainer)
    {
        enabled = true;

        if (letterContainer)
        {

        }
    }
}