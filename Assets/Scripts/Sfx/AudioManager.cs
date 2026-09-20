using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioMixerGroup mixerGroup;

    public Sound[] sounds;

    private void Awake()
    {
        if(Instance != this) Instance = this;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;

            s.source.outputAudioMixerGroup = mixerGroup;
        }
    }

	private void Start()
	{
        Play("Music");
	}

	public void Play(string sound) 
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if(s == null)
        {
            Debug.LogWarning("Sound : " + name + " not found ! ");
            return;
        }

        s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        s.source.Play();
    }

    public void Stop(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound : " + name + " not found ! ");
            return;
        }

        s.source.Stop();
    }

    public void setSoundVolume(string sound, float volume, bool varia = true)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound : " + name + " not found ! ");
            return;
        }

        s.source.volume = volume * ( varia ? (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f)) : 1f);
    }

    public void setSoundVolume(int id, float volume, bool varia = true)
    {
        Sound s = sounds[id];

		s.source.volume = volume * (varia ? (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f)) : 1f);

	}

    public int GetSoundID(string sound)
    {
		Sound s = Array.Find(sounds, item => item.name == sound);
        return Array.IndexOf(sounds, s);
	}

    public void StopAllSounds()
    {
        foreach(Sound s in sounds)
        {
            s.source.Stop();
        }
    }
}
