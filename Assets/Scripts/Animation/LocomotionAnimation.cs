using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class LocomotionAnimation
{


	#region Locomotion Animation Keys - Assault Characters

	public const string Walking = "Walking";
	public const string Sprinting = "Sprinting";
	public const string InAir = "InAir";
	public const string VelocityX = "MovementVelocityX";
	public const string VelocityZ = "MovementVelocityZ";
	#endregion

	#region Locomotions Constants 

	public const float StartingVelocityX = 0.0f;
	public const float StartingVelocityZ = 0.0f;
	public const float Acceleration = 2.0f;
	public const float Deceleration = 2.0f;
	public const float MaximumWalkVelocity = 0.5f;
	public const float MaximumSprintVelocity = 2.0f;

	#endregion
}