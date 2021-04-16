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


		foreach (SoundFX s in GameSoundEffects)
		{
				s.source = gameObject.AddComponent<AudioSource>();


				// set the source's clip, volume and pitch 
				s.source.clip = s.clip;
				s.source.loop = s.loop;

				s.source.outputAudioMixerGroup = AudioMixer;
		
		}

		foreach (SoundFX sound in HealthSoundEffects)
		{
			sound.source = gameObject.AddComponent<AudioSource>();
			sound.source.clip = sound.clip;
			sound.source.loop = sound.loop;


			sound.source.outputAudioMixerGroup = AudioMixer;
		}

		foreach (SoundFX sound in AmmoSoundEffects)
		{
			sound.source = gameObject.AddComponent<AudioSource>();
			sound.source.clip = sound.clip;
			sound.source.loop = sound.loop;


			sound.source.outputAudioMixerGroup = AudioMixer;
		}
	}

	/// <summary>
	///		Plays a random health pickup sound from the array of health sound effects 
	/// </summary>
	public void	PlayHealthPickupAudio()
	{
		Random rand = new Random(DateTime.Now.ToString().GetHashCode());

		int selected = rand.Next(HealthSoundEffects.Length);

		SoundFX s = HealthSoundEffects[selected];

		if (s == null)
		{
			Debug.LogWarning("[AudioManager.PlayAmmunitionPickupAudio]: " + "Health sound " + name + " could not be found!");
			return;
		}

		s.source.volume = s.volume;
		s.source.pitch = s.pitch;

		s.source.Play();
	}

	/// <summary>
	///		Plays a random ammunition pickup sound from the list of ammo sound effects 
	/// </summary>
	public void PlayAmmunitionPickupAudio()
	{
		Random rand = new Random(DateTime.Now.ToString().GetHashCode());

		int selectedIndex = rand.Next(AmmoSoundEffects.Length);

		SoundFX s = AmmoSoundEffects[selectedIndex];

		if (s == null)
		{
			Debug.LogWarning("[AudioManager.PlayAmmunitionPickupAudio]: " + "Ammunition sound " + name + " could not be found!");
		}

		s.source.volume = s.volume;
		s.source.pitch = s.pitch;

		s.source.Play();
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
		SoundFX s = Array.Find(GameSoundEffects, soundEffect => soundEffect.name == AudioSourceName);

		if (s == null)
		{

			Debug.LogWarning("Audio Source Sound " + name + " could not be found!");
			return null;
		}


		return s.source.clip;
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
