using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour {

    public Sound[] sounds;
    public static AudioManager instance;
	// Use this for initialization
	void Awake () {

        if (instance == null)
            instance = this;
        else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

		foreach( Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();

            if (s.group.name == "Music")
                s.source.ignoreListenerPause = true;

            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            s.source.outputAudioMixerGroup = s.group;
        }
	}

    private void Start() {
        
    }

    public void Play (string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("Sound:" + name + "not found!");
            return;
        }
        s.source.volume = s.volume * (1 + UnityEngine.Random.Range(-s.randomVolume / 2f, s.randomVolume / 2f));
        s.source.pitch = s.pitch * (1 + UnityEngine.Random.Range(-s.randomPitch / 2f, s.randomPitch / 2f));
        s.source.Play();
    }

    public void Stop(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("Sound:" + name + "not found!");
            return;
        }
        s.source.Stop();
    }

    public void Pause(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("Sound:" + name + "not found!");
            return;
        }
        s.source.Pause();
    }

    public void UnPause(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("Sound:" + name + "not found!");
            return;
        }
        s.source.UnPause();
    }
}
