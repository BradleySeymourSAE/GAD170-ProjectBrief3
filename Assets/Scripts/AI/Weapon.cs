using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///		Data class for holding weapon input's 
/// </summary>
public struct Weapon
{ 

	/// <summary>
	///		Should we switch to primary weapon 
	/// </summary>
	public bool SwitchToPrimary;
	/// <summary>
	///		Should we switch to secondary 
	/// </summary>
	public bool SwitchToSecondary;
	/// <summary>
	///		Should be fire the weapon 
	/// </summary>
	public bool FireWeapon;
	/// <summary>
	///		The Player Target's Position
	/// </summary>
	public Vector3 Target;

}
