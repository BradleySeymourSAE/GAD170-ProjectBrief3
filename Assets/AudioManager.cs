using System;
using UnityEngine;
using UnityEngine.Audio;



public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;

	public AudioMixerGroup mixerGroup;

	public Sound[] sounds;

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.outputAudioMixerGroup = mixerGroup;
		}
	}

	public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariation / 2f, s.volumeVariation / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariation / 2f, s.pitchVariation / 2f));

		s.source.Play();
	}
}

[System.Serializable]
public class Sound
{
	public string name;
	public AudioClip clip;

	[Range(0f, 1f)]
	public float volume = .75f;
	
	[Range(0f, 1f)]
	public float volumeVariation = .1f;

	[Range(0.1f, 0.3f)]
	public float pitch = 1f;

	[Range(0f, 1f)]
	public float pitchVariation = .1f;

	public bool loop = false;

	public AudioMixerGroup Mixer;

	[HideInInspector]
	public AudioSource source;
}