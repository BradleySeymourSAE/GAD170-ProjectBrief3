using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfantryAI_SoundEffects
{

	[Header("Infantry Audio")]
	public float pitchMax = 0.2f;
	private float originalPitch;
		
	private AudioSource MovementAudioSource;
	public AudioClip Idling;
	public AudioClip Roaming;
	public AudioClip Running;

	/// <summary>
	///		Handles setting up the Infantry AI sound effects 
	/// </summary>
	/// <param name="AI"></param>
	public void Setup(Transform AI)
	{
		if (AI.GetComponent<AudioSource>() != null)
		{
			MovementAudioSource = AI.GetComponent<AudioSource>();
			originalPitch = MovementAudioSource.pitch;
		}
		else
		{
			Debug.LogError("[InfantryAI_SoundEffects]: " + "No audio source could be found for the AI character!");
		}
	}

	/// <summary>
	///		Plays a walking sound based on the movement of the AI character
	/// </summary>
	/// <param name="Movement"></param>
	public void PlayMovementAudio(float Movement)
	{
		// If the ai character is less than 0.1, should be set to idle 
		if (Mathf.Abs(Movement) < 0.1f)
		{
			if (MovementAudioSource.clip != Idling)
			{
				MovementAudioSource.clip = Idling;
				MovementAudioSource.Play();
			}
		}
		// Else, If movement is greater than or equal to moving ut less than half way 
		else if (Mathf.Abs(Movement) >= 0.1f && Mathf.Abs(Movement) < 0.5f)
		{
			if (MovementAudioSource.clip != Roaming)
			{
				MovementAudioSource.clip = Roaming;
				MovementAudioSource.Play();
			}
		}
		// Otherwise we are clearly moving so we should be playing the runnning audio instead 
		else
		{
			if (MovementAudioSource != Running)
			{
				MovementAudioSource.clip = Running;
				MovementAudioSource.Play();
			}
		}
	}
}
