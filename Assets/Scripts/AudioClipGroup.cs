using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AudioClipGroup")]
public class AudioClipGroup : ScriptableObject
{
    [Range(0f, 1f)]
    public float VolumeMin = 1f;
    [Range(0f, 1f)]
    public float VolumeMax = 1f;

    [Range(0f, 2f)]
    public float PitchMin = 1f;
    [Range(0f, 2f)]
    public float PitchMax = 1f;

    public float Cooldown = 0.1f;
    private float NextPlayTime;

    public List<AudioClip> AudioClips;

    public void OnEnable()
    {
        NextPlayTime = 0;
    }

    public void Play()
    {
        if (Time.time < NextPlayTime) return;

        NextPlayTime = Time.time + Cooldown;
        AudioSource source = AudioSourcePool.Instance.GetSource();


        source.volume = Random.Range(VolumeMin, VolumeMax);
        source.pitch = Random.Range(PitchMin, PitchMax);


        source.clip = AudioClips[Random.Range(0, AudioClips.Count)];

        source.Play();
    }


}