using System.Collections;
using UnityEngine;

public class GeneralAudioPlayer : Singleton<GeneralAudioPlayer>
{
    public AudioSource audioSource;



    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }


    public void play(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }


    public IEnumerator playWaitFinish(AudioClip clip)
    {
        play(clip);
        yield return new WaitForSeconds(clip.length);
    }
}