using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePool : MonoBehaviour
{
    public static AudioSourcePool Instance;

    private List<AudioSource> sources = new List<AudioSource>();

    public AudioSource AudioSourcePrefab;

    public void Awake()
    {
        Instance = this;
    }

    public AudioSource GetSource()
    {
        foreach (AudioSource sr in sources)
        {
            if (!sr.isPlaying)
            {
                return sr;
            }
        }

        AudioSource source = Instantiate<AudioSource>(AudioSourcePrefab, transform);
        sources.Add(source);

        return source;
    }



}
