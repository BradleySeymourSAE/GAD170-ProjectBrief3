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
	///		T90 Engine Audio Effect Keys
	/// </summary>
	public const string T90_EngineIdle = "T90_EngineIdle";
	public const string T90_EngineDriving = "T90_EngineDriving";
	public const string T90_WeaponSwitch = "T90_WeaponSwitch";
	/// <summary>
	///		T90 Tank Primary Weapon Audio Effect Keys
	/// </summary>
	public const string T90_PrimaryWeapon_Fire = "T90_PrimaryWeapon_Fire";
	public const string T90_PrimaryWeapon_Reload = "T90_PrimaryWeapon_Reload";
	public const string T90_PrimaryWeapon_Aiming = "T90_PrimaryWeapon_Aiming";

	public const string T90_SecondaryWeapon_Fire = "T90_SecondaryWeapon_Fire";
	public const string T90_SecondaryWeapon_Overheat = "T90_SecondaryWeapon_Overheat";
	public const string T90_SecondaryWeapon_Aiming = "T90_SecondaryWeapon_Aiming";

	#endregion


	#region Weapons

	#region Primary Weapons

	/// <summary>
	///		AEK Audio Effect Keys
	/// </summary>
	public const string AEK971_PrimaryWeapon_Fire = "AEK971_PrimaryWeapon_Fire";
	public const string AEK971_PrimaryWeapon_FireShort = "AEK971_PrimaryWeapon_FireShort";
	public const string AEK971_PrimaryWeapon_Reload = "AEK971_PrimaryWeapon_Reload";
	public const string AEK971_PrimaryWeapon_Empty = "AEK971_PrimaryWeapon_Empty";

	/// <summary>
	///		M16A3 Audio Effect Keys
	/// </summary>
	public const string M16A3_PrimaryWeapon_Fire = "M16A3_PrimaryWeapon_Fire";
	public const string M16A3_PrimaryWeapon_FireShort = "M16A3_PrimaryWeapon_FireShort";
	public const string M16A3_PrimaryWeapon_Reload = "M16A3_PrimaryWeapon_Reload";
	public const string M16A3_PrimaryWeapon_Empty = "M16A3_PrimaryWeapon_Empty";

	#endregion

	#region Secondary Weapons

	/// <summary>
	///		M9 - Long arm of the law - Audio Effect Keys
	/// </summary>
	public const string M9_SecondaryWeapon_Fire = "M9_SecondaryWeapon_Fire";
	public const string M9_SecondaryWeapon_Reload = "M9_SecondaryWeapon_Reload";
	public const string M9_SecondaryWeapon_Empty = "M9_SecondaryWeapon_Empty";

	/// <summary>
	///		Magnum - 44 Magnum Audio Effect Keys
	/// </summary>
	public const string TAURUS_SecondaryWeapon_Fire = "TAURUS_SecondaryWeapon_Fire";
	public const string TAURUS_SecondaryWeapon_Reload = "TAURUS_SecondaryWeapon_Reload";
	public const string TAURUS_SecondaryWeapon_Empty = "TAURUS_SecondaryWeapon_Empty";


	#endregion

	#endregion
}