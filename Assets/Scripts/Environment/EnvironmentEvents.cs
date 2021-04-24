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
	///		 Handles empty parameter delegate events 
	/// </summary>
	public delegate void VoidDelegate();

	/// <summary>
	///		Handles int parameter events  
	/// </summary>
	/// <param name="Amount"></param>
	public delegate void IntParameterDelegate(int Amount);

	/// <summary>
	///		Handles Game Object List Events 
	/// </summary>
	/// <param name="GameObjectList"></param>
	public delegate void GameObjectListDelegate(List<GameObject> GameObjectList);

	/// <summary>
	///		Handles Transform List Events 
	/// </summary>
	/// <param name="TransformListDelegate"></param>
	public delegate void TransformListDelegate(List<Transform> TransformListDelegate);

	/// <summary>
	///		Handles Vector3 List Events 
	/// </summary>
	/// <param name="Vector3ListDelegate"></param>
	public delegate void Vector3ListDelegate(List<Vector3> Vector3ListDelegate);


	public static IntParameterDelegate SpawnTreesEvent;

	public static GameObjectListDelegate HandleTreesSpawnedEvent;

	public static Vector3ListDelegate HandleTreeSpawnPositionsEvent;


}