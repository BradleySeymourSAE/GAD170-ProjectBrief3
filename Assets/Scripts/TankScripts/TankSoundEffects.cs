using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TankSoundEffects
{
    [Header("Movement Audio")]
    public AudioClip EngineIdle; // the tank idling clip
    public AudioClip EngineMoving; // the tank moving clip
    public float pitchRangeMax = 0.2f; // the maximum amount our pitch can be changed by
    private float originalPitchLevel; // the starting pitch level before we modify it 
    private AudioSource audioSource; // a reference to our audio source component

    private AudioManager m_audioManager;

    /// <summary>
    /// Sets up the audio source by getting the reference from the tank
    /// </summary>
    /// <param name="Tank"></param>
    public void SetUp(Transform Tank)
    {
        if (Tank.GetComponent<AudioSource>() != null)
        {
            audioSource = Tank.GetComponent<AudioSource>(); // find a reference to the audio source
            originalPitchLevel = audioSource.pitch; // set the starting pitch
        }
        else
        {
            Debug.LogError("No Audio Source found on the tank");
        }


        
        if (Object.FindObjectOfType<AudioManager>() != null)
		{
            m_audioManager = Object.FindObjectOfType<AudioManager>();
		}
        else
		{
            Debug.LogError("No Audio Manager Instance could be found!");
		}


    }

    /// <summary>
    /// takes in the movement and the rotation, if either is moving, play the move sound effect, else play the idle sound effect
    /// </summary>
    /// <param name="MoveInput"></param>
    /// <param name="RotationInput"></param>
    public void PlayTankEngine(float MoveInput, float RotationInput)
    {
        if (Mathf.Abs(MoveInput) < 0.1f && Mathf.Abs(RotationInput) < 0.1f)
        {
            // there is no current input from moving or rotating
            if (audioSource.clip != EngineIdle)
            {
                audioSource.clip = EngineIdle; // set the audio to our idle sound
               //  audioSource.pitch = Random.Range(originalPitchLevel - pitchRangeMax, originalPitchLevel + pitchRangeMax); // get a random pitch level
                audioSource.Play(); // play our new clip
            }
        }
        else
        {
            // we must be moving or rotating
            if (audioSource.clip != EngineMoving)
            {
                audioSource.clip = EngineMoving; // set the audio to our move sound
                // audioSource.pitch = Random.Range(originalPitchLevel - pitchRangeMax, originalPitchLevel + pitchRangeMax); // get a random pitch level
                audioSource.Play(); // play our new clip
            }
        }
    }

    /// <summary>
    ///     Plays a sound from the audio manager instance 
    /// </summary>
    /// <param name="sound"></param>
    public void PlaySound(string sound)
	{
          m_audioManager.Play(sound);
	}
}
