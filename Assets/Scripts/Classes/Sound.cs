using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound {

    public string name;

    public AudioClip clip;

    public AudioMixerGroup group;

    [Range(0f,1f)]
    public float volume;
    [Range(0.5f, 1.5f)]
    public float pitch=1f;

    public bool loop;

    
    [Range(0f, 0.5f)]
    public float randomVolume=0f;
    [Range(.0f, 0.5f)]
    public float randomPitch=0f;

    [HideInInspector]
    public AudioSource source;
}
