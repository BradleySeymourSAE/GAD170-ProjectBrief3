﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TankSoundEffects
{
     [Header("Movement Audio Settings")]
     public bool ShouldLoop = true;


     private AudioClip EngineIdle; // the tank idling clip
     private AudioClip EngineDriving; // the tank moving clip
     private AudioSource EngineAudioSource; // a reference to our audio source component

    /// <summary>
    /// Sets up the audio source by getting the reference from the tank
    /// </summary>
    /// <param name="Tank"></param>
    public void Setup(Transform Tank)
    {
        if (Tank.GetComponent<AudioSource>())
		{
            EngineAudioSource = Tank.GetComponent<AudioSource>();
            EngineAudioSource.loop = ShouldLoop;
            Debug.Log("[TankSoundEffects.Setup]: " + "Audio Source Instance found!");
            EngineIdle = AudioManager.Instance.GetAudioClip(GameAudio.T90_EngineIdle);
            EngineDriving = AudioManager.Instance.GetAudioClip(GameAudio.T90_EngineDriving);
        }
        else
        {
            Debug.LogWarning("[TankSoundEffects.Setup]: " + "Audio Source Instance could not be found!");
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
                EngineAudioSource.clip = EngineIdle;
                EngineAudioSource.Play();
			}
        }
        else
        {
            // Then we are moving and we should be playing this clip instead 
            if (EngineAudioSource.clip != EngineDriving)
			{
                EngineAudioSource.clip = EngineDriving;
                EngineAudioSource.Play();
			}
        }
    }
}