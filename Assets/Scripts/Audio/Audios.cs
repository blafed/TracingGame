using UnityEngine;

public class Audios : MonoBehaviour
{
    public static Audios o;

    AudioSource[] sources;

    private void Awake()
    {
        o = this;
        sources = GetComponentsInChildren<AudioSource>();
    }

    public void play(string name)
    {
        var a = System.Array.Find(sources, x => x.name == name);
        if (!a)
        {
            Debug.LogError("no audio found");
            return;
        }
        a.Play();
    }
}