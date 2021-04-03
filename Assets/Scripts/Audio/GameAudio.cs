using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		Holds Audio Manager Sound Key Data
/// </summary>
[System.Serializable]
public static class GameAudio
{

	#region UI & Background Audio
	public const string BackgroundThemeTrack = "BackgroundThemeTrack";

	#endregion

	#region T90 Tank

	/// <summary>
	///		T90 Engine Audio Keys
	/// </summary>
	public const string T90_EngineIdle = "T90_EngineIdle";
	public const string T90_EngineDriving = "T90_EngineDriving";
	
	/// <summary>
	///		T90 Tank Primary Weapon Audio Keys 
	/// </summary>
	public const string T90_PrimaryWeapon_Fire = "T90_PrimaryWeapon_Fire";
	public const string T90_PrimaryWeapon_Reload = "T90_PrimaryWeapon_Reload";
	public const string T90_PrimaryWeapon_Aiming = "T90_PrimaryWeapon_Aiming";

	#endregion

}