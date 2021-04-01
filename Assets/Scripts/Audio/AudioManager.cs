using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Collections;
using System.Collections.Generic;


public class AudioManager : MonoBehaviour
{

	// Audio Manager Instance 
	public static AudioManager Instance; // reference to the audio manager instance 

	// Audio Mixer Group 
	public AudioMixerGroup MasterAudioMixer; // reference to the audio mixer

	private const string MasterVolumeParamKey = "Volume";

	// Array of sound effects 
	public SoundFX[] sounds;

	/// <summary>
	///		Called before the start method 
	/// </summary>
	private void Awake()
	{


		if (Instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}


		foreach (SoundFX s in sounds)
		{
				s.source = gameObject.AddComponent<AudioSource>();


				// set the source's clip, volume and pitch 
				s.source.clip = s.clip;
				s.source.loop = s.loop;

				s.source.outputAudioMixerGroup = MasterAudioMixer;
		
		}
	}

	/// <summary>
	///		Gets an audio source reference by the sound name 
	/// </summary>
	/// <param name="Sound"></param>
	/// <returns></returns>
	public AudioSource GetAudioSource(string Sound)
	{

		SoundFX s = Array.Find(sounds, item => item.name == Sound);

		if (s == null)
		{
			Debug.LogWarning("Sound " + name + " could not be found!");
			return null;
		}

		return s.source;
	}	

	/// <summary>
	///		Finds an audio clip 
	/// </summary>
	/// <param name="Sound"></param>
	/// <returns></returns>
	public AudioClip FindAudioClip(string Sound)
	{
		SoundFX s = Array.Find(sounds, item => item.name == Sound);

		if (s == null)
		{
			Debug.Log("Clip " + name + " couldn't be found!");
			return null;
		}

		return s.clip;
	}

	/// <summary>
	///		Handles playing an audio clip by its name  
	/// </summary>
	/// <param name="SoundEffectName"></param>
	public void PlaySound(string soundEffect)
	{
		SoundFX s = Array.Find(sounds, item => item.name == soundEffect);
	
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " could not be found!");
			return;
		}

		s.source.volume = s.volume;
		s.source.pitch = s.pitch;

		s.source.Play();
	}

	/// <summary>
	///		Handles stop playing an audio clip by its name 
	/// </summary>
	/// <param name="Sound"></param>
	public void StopPlaying(string Sound)
	{
		SoundFX s = Array.Find(sounds, item => item.name == Sound);

		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume;
		s.source.pitch = s.pitch;

		s.source.Stop();
	}

	// Sets the master volume 
	public void SetMasterVolume(float volume)
	{
		MasterAudioMixer.audioMixer.SetFloat(MasterVolumeParamKey, volume);
	}
	/// <summary>
	///		Starts the Fade out sound effect using a coroutine
	/// </summary>
	/// <param name="Sound">The SoundFX audiosource sound to fade</param>
	/// <param name="volume">The target volume to fade to</param>
	/// <param name="duration">The duration of the fade </param>
	public void FadeSoundEffect(string Sound, float volume, float duration)
	{
		StartCoroutine(Fade(Sound, volume, duration));
	}

	/// <summary>
	///		Fades out a audio sound by sound name 
	/// </summary>
	/// <param name="Sound"></param>
	/// <returns></returns>
	private IEnumerator Fade(string Sound, float p_targetVolume, float p_fadeSpeed)
	{
		AudioSource targetSource = GetAudioSource(Sound);

		if (targetSource == null)
		{
			Debug.LogWarning("Sound: " + name + " could not be found!");
			yield return null;
		}

		targetSource.volume = Mathf.Lerp(targetSource.volume, p_targetVolume, p_fadeSpeed * Time.deltaTime);
		targetSource.pitch = targetSource.pitch;
		

		yield return null;
	}
}
