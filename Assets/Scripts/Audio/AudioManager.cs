using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = System.Random;

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

	public SoundFX[] HealthSoundEffects;
	
	public SoundFX[] AmmoSoundEffects;

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


		foreach (SoundFX s_SoundFX in GameSoundEffects)
		{
				s_SoundFX.source = gameObject.AddComponent<AudioSource>();


				// set the source's clip, volume and pitch 
				s_SoundFX.source.clip = s_SoundFX.clip;
				s_SoundFX.source.loop = s_SoundFX.loop;

				s_SoundFX.source.outputAudioMixerGroup = AudioMixer;
		
		}

		foreach (SoundFX s_SoundFX in HealthSoundEffects)
		{
			s_SoundFX.source = gameObject.AddComponent<AudioSource>();
			s_SoundFX.source.clip = s_SoundFX.clip;
			s_SoundFX.source.loop = s_SoundFX.loop;


			s_SoundFX.source.outputAudioMixerGroup = AudioMixer;
		}

		foreach (SoundFX s_SoundFX in AmmoSoundEffects)
		{
			s_SoundFX.source = gameObject.AddComponent<AudioSource>();
			s_SoundFX.source.clip = s_SoundFX.clip;
			s_SoundFX.source.loop = s_SoundFX.loop;


			s_SoundFX.source.outputAudioMixerGroup = AudioMixer;
		}
	}

	/// <summary>
	///		Plays a random health pickup sound from the array of health sound effects 
	/// </summary>
	public void	PlayHealthPickupAudio()
	{
		Random rand = new Random(DateTime.Now.ToString().GetHashCode());

		int selected = rand.Next(HealthSoundEffects.Length);

		SoundFX s_SoundFX = HealthSoundEffects[selected];

		if (s_SoundFX == null)
		{
			Debug.LogWarning("[AudioManager.PlayHealthPickupAudio]: " + "Health sound " + name + " could not be found!");
			return;
		}

		s_SoundFX.source.volume = s_SoundFX.volume;
		s_SoundFX.source.pitch = s_SoundFX.pitch;

		s_SoundFX.source.Play();
	}

	/// <summary>
	///		Plays a random ammunition pickup sound from the list of ammo sound effects 
	/// </summary>
	public void PlayAmmunitionPickupAudio()
	{
		Random rand = new Random(DateTime.Now.ToString().GetHashCode());

		int selectedIndex = rand.Next(AmmoSoundEffects.Length);

		SoundFX s_SoundFX = AmmoSoundEffects[selectedIndex];

		if (s_SoundFX == null)
		{
			Debug.LogWarning("[AudioManager.PlayAmmunitionPickupAudio]: " + "Ammunition sound " + name + " could not be found!");
		}

		s_SoundFX.source.volume = s_SoundFX.volume;
		s_SoundFX.source.pitch = s_SoundFX.pitch;

		s_SoundFX.source.Play();
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
	///		Get Audio Clip by the Audio Source name 
	/// </summary>
	/// <param name="AudioSourceName">The name of the audio source </param>
	/// <returns></returns>
	public AudioClip GetAudioClip(string AudioSourceName)
	{
		SoundFX s_SoundFX = Array.Find(GameSoundEffects, soundEffect => soundEffect.name == AudioSourceName);

		if (s_SoundFX == null)
		{

			Debug.LogWarning("Audio Source Sound " + name + " could not be found!");
			return null;
		}


		return s_SoundFX.source.clip;
	}

	/// <summary>
	///		Handles playing an audio clip by its name  
	/// </summary>
	/// <param name="SoundEffectName"></param>
	public void PlaySound(string soundEffect)
	{
		SoundFX s_SoundFX = Array.Find(GameSoundEffects, item => item.name == soundEffect);
	
		if (s_SoundFX == null)
		{
			Debug.LogWarning("Sound: " + name + " could not be found!");
			return;
		}

		s_SoundFX.source.volume = s_SoundFX.volume;
		s_SoundFX.source.pitch = s_SoundFX.pitch;

		s_SoundFX.source.Play();
	}

	/// <summary>
	///		Stops playing an audio clip by the audio sources name 
	/// </summary>
	/// <param name="soundEffect"></param>
	public void StopPlaying(string soundEffect)
	{
		SoundFX s_SoundFX = Array.Find(GameSoundEffects, item => item.name == soundEffect);

		if (s_SoundFX == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s_SoundFX.source.volume = s_SoundFX.volume;
		s_SoundFX.source.pitch = s_SoundFX.pitch;

		s_SoundFX.source.Stop();
	}

	/// <summary>
	///		Starts the Fade out sound effect using a coroutine
	/// </summary>
	/// <param name="soundEffect">The SoundFX audiosource sound to fade</param>
	/// <param name="volume">The target volume to fade to</param>
	/// <param name="duration">The duration of the fade </param>
	public void FadeSoundEffect(string soundEffect, float volume, float duration)
	{
		StartCoroutine(Fade(soundEffect, volume, duration));
	}

	/// <summary>
	///		Fades out a audio sound by an audio sources name name 
	/// </summary>
	/// <param name="soundEffect">The audio source name</param>
	/// <param name="volume">The target volume to fade to</param>
	/// <param name="fadingDuration">The speed of the fade to apply</param>
	/// <returns></returns>
	private IEnumerator Fade(string soundEffect, float volume, float fadingDuration)
	{
		AudioSource s_TargetAudioSource = Instance.GetAudioSource(soundEffect);

		if (s_TargetAudioSource == null)
		{
			Debug.LogWarning("Sound: " + name + " could not be found!");
			yield return null;
		}

		s_TargetAudioSource.volume = Mathf.Lerp(s_TargetAudioSource.volume, volume, fadingDuration * Time.deltaTime);
		s_TargetAudioSource.pitch = s_TargetAudioSource.pitch;
		

		yield return null;
	}



}
