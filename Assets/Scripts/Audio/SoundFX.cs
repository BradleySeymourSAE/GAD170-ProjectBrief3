using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class SoundFX
{
	/// <summary>
	///		The name of our sound effect 
	/// </summary>
	public string name;

	/// <summary>
	///		Set whether the audio clip is looping or not 
	/// </summary>
	public bool loop = false; // default to false  

	/// <summary>
	///		The volume for the sound effect 
	/// </summary>
	[Range(0f, 1f)]
	public float volume = 1f; // default to 1f 

	/// <summary>
	///		The Pitch for the sound fx 
	/// </summary>
	[Range(1f, 3f)]
	public float pitch = 1f; // default to 1f

	/// <summary>
	///		Sound Effect Clip
	/// </summary>
	public AudioClip clip;

	/// <summary>
	///		The game objects audio source to play the clip from 
	/// </summary>
	[HideInInspector] 
	public AudioSource source;

}
