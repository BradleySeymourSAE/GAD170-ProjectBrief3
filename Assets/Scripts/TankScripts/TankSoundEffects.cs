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
    private AudioSource EngineAudioSource; // a reference to our audio source component

    /// <summary>
    /// Sets up the audio source by getting the reference from the tank
    /// </summary>
    /// <param name="Tank"></param>
    public void Setup(Transform Tank)
    {
        if (Tank.GetComponent<AudioSource>() != null)
        {
            EngineAudioSource = Tank.GetComponent<AudioSource>(); // find a reference to the audio source
            originalPitchLevel = EngineAudioSource.pitch; // set the starting pitch
        }
        else
        {
            Debug.LogError("No Audio Source found on the tank");
        }
    }

    /// <summary>
    /// takes in the movement and the rotation, if either is moving, play the move sound effect, else play the idle sound effect
    /// </summary>
    /// <param name="MoveInput"></param>
    /// <param name="RotationInput"></param>
    public void PlayEngineSound(float MoveInput, float RotationInput)
    {
        // If we arent moving we should be setting the sources audio to idle 
        if (Mathf.Abs(MoveInput) < 0.1f && Mathf.Abs(RotationInput) < 0.1f)
        {
            if (EngineAudioSource.clip != EngineIdle)
            {
                EngineAudioSource.clip = EngineIdle; // set the audio to our idle sound
                EngineAudioSource.Play(); // play our new clip
            }
        }
        else
        {
            // Then we are moving and we should be playing this clip instead 
            if (EngineAudioSource.clip != EngineMoving)
            {
                EngineAudioSource.clip = EngineMoving; // set the audio to our move sound
                EngineAudioSource.Play(); // play our new clip
            }
        }
    }
}