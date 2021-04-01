using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfantryAI_ParticleEffects { 

	private ParticleSystem[] WalkingDustTrailEffects = new ParticleSystem[] { }; // Array fo store our particle effects in 

	/// <summary>
	///		Setup particle effects for movement 
	/// </summary>
	/// <param name="AI"></param>
	public void SetupEffects(Transform AI)
	{
		if (AI.GetComponentInChildren<ParticleSystem>() != null)
		{ 
			WalkingDustTrailEffects = AI.GetComponentsInChildren<ParticleSystem>();
		}
		else
		{
			Debug.LogWarning("[InfantryAI_ParticleEffects.SetupEffects]: " + "Infantry AI Particle effects have not been setup correctly!");
		}
	}


	/// <summary>
	///	Shows / Hides the walking dust trail effects 
	/// </summary>
	/// <param name="ShowDustTrails"></param>
	public void ShowWalkingDustTrails(bool ShowDustTrails)
	{
		for (int i = 0; i < WalkingDustTrailEffects.Length; i++)
		{
			if (ShowDustTrails)
			{
				WalkingDustTrailEffects[i].Play();
			}
			else
			{
				WalkingDustTrailEffects[i].Stop();
			}
		}
	}
}
