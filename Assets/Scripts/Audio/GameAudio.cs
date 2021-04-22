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

	/// <summary>
	///		Background Audio for the Starting Menu 
	/// </summary>
	public const string BackgroundThemeTrack = "BackgroundThemeTrack";

	/// <summary>
	///		Sound Key for when a player dies 
	/// </summary>
	public const string PlayerDeathOOFT = "PlayerDeathOOFT";

	/// <summary>
	///		When switching between game menus transition sound effect
	/// </summary>
	public const string GUI_MenuSwitch = "GUI_MenuSwitch";

	/// <summary>
	///		Sound effect for on click events 
	/// </summary>
	public const string GUI_Select = "GUI_Select";


	#endregion

	#region T90 Tank

	/// <summary>
	///		T90 Engine Audio Effect Keys
	///		Idle Engine Sound
	///		Driving Engine Sound
	///		Weapon Switching Sound 
	///		Primary Weapon Fire Sound 
	///		Primary Weapon Reload Sound
	///		Primary Weapon Aiming - When the turret it rotating on the Y axis 
	/// </summary>
	public const string T90_EngineIdle = "T90_EngineIdle";
	public const string T90_EngineDriving = "T90_EngineDriving";
	public const string T90_WeaponSwitch = "T90_WeaponSwitch"; // unsued 
	/// <summary>
	///		T90 Tank Primary Weapon Audio Effect Keys
	/// </summary>
	public const string T90_PrimaryWeapon_Fire = "T90_PrimaryWeapon_Fire"; 
	public const string T90_PrimaryWeapon_Reload = "T90_PrimaryWeapon_Reload";
	public const string T90_PrimaryWeapon_Aiming = "T90_PrimaryWeapon_Aiming";


	/// <summary>
	///  EDIT: REMOVED 
	///  Secondary Weapon Keys will be added in the near future once I have time to get around to it :) 
	/// </summary>
	public const string T90_SecondaryWeapon_Fire = "T90_SecondaryWeapon_Fire";
	public const string T90_SecondaryWeapon_DoubleShot = "T90_SecondaryWeapon_DoubleShot";
	public const string T90_SecondaryWeapon_Overheat = "T90_SecondaryWeapon_Overheat";

	#endregion





	// OLD CODE i was planning to use but never got around to - Will maybe use this in the future :) 
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