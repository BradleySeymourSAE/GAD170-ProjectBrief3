#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion




/// <summary>
///		Attempt at creating events for generating the environment at runtime
///		I think that using events could actually make this process alot easier in the long run 
///		Let's see how we go 
/// </summary>
public static class EnvironmentEvents
{
	
	/// <summary>
	///		Handles int parameter events  
	/// </summary>
	/// <param name="Amount"></param>
	public delegate void IntParameterDelegate(int Amount);


	/// <summary>
	///		Handles spawning trees on the terrain 
	/// </summary>
	public static IntParameterDelegate SpawnTreesEvent;

}