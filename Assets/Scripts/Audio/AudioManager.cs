using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Collections;
using System.Collections.Generic;


public class AudioManager : MonoBehaviour
{
	/// <summary>
	///		Audio Manager Instance 
	/// </summary>
	public static AudioManager Instance;

	/// <summary>
	///		Audio Mixer Group 
	/// </summary>
	public AudioMixerGroup AudioMixer;
	
	/// <summary>
	///		Game sound effects, Tanks, Infantry etc
	/// </summary>
	public SoundFX[] GameSoundEffects;

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


		foreach (SoundFX s in GameSoundEffects)
		{
				s.source = gameObject.AddComponent<AudioSource>();


				// set the source's clip, volume and pitch 
				s.source.clip = s.clip;
				s.source.loop = s.loop;

				s.source.outputAudioMixerGroup = AudioMixer;
		
		}
	}


	/// <summary>
	///		Gets an audio source reference by the sound name 
	/// </summary>
	/// <param name="Sound"></param>
	/// <returns></returns>
	public AudioSource GetAudioSource(string Sound)
	{

		SoundFX s = Array.Find(GameSoundEffects, item => item.name == Sound);

		if (s == null)
		{
			Debug.LogWarning("Sound " + name + " could not be found!");
			return null;
		}

		return s.source;
	}	

	/// <summary>
	///		Handles playing an audio clip by its name  
	/// </summary>
	/// <param name="SoundEffectName"></param>
	public void PlaySound(string soundEffect)
	{
		SoundFX s = Array.Find(GameSoundEffects, item => item.name == soundEffect);
	
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
	///		Stops playing an audio clip by the audio sources name 
	/// </summary>
	/// <param name="Sound"></param>
	public void StopPlaying(string Sound)
	{
		SoundFX s = Array.Find(GameSoundEffects, item => item.name == Sound);

		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume;
		s.source.pitch = s.pitch;

		s.source.Stop();
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
	///		Fades out a audio sound by an audio sources name name 
	/// </summary>
	/// <param name="Sound">The audio source name</param>
	/// <param name="p_targetVolume">The target volume to fade to</param>
	/// <param name="p_fadeSpeed">The speed of the fade to apply</param>
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
