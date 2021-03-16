using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TankParticleEffects
{
    private ParticleSystem[] allDustTrails = new ParticleSystem[] { }; // an array to store all our particle effects

    /// <summary>
    /// takes in the transform of the tank and finds the particle effects to play for movement
    /// </summary>
    /// <param name="tank"></param>
    public void SetUpEffects(Transform Tank)
    {
        allDustTrails = Tank.GetComponentsInChildren<ParticleSystem>(); // find all the particle systems
    }

    /// <summary>
    /// Turns the dust trails on or off based on the Enabled parameter
    /// </summary>
    /// <param name="Enabled"></param>
    public void PlayDustTrails(bool Enabled)
    {
        // loop through all our dust trails
        for (int i = 0; i < allDustTrails.Length; i++)
        {
            if (Enabled)
            {
                // play the dust
                allDustTrails[i].Play();
            }
            else
            {
                // turn off the dust
                allDustTrails[i].Stop();
            }
        }
    }
}
