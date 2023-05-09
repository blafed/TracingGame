using UnityEngine;

public class PlayAudioOnStart : MonoBehaviour
{
    public float delay = 0;

    public AudioSource audioSource;


    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        Invoke(nameof(play), delay);
    }

    void play()
    {
        audioSource.Play();
    }


}