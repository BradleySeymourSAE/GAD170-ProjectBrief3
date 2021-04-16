#region Namespaces
using UnityEngine;
#endregion



/// <summary>
///		Object to hold type of item the collectable is
/// </summary>
public enum SpecialItemPickup { Health, Ammunition };

[CreateAssetMenu(menuName="Collectables/Items", fileName = "Add Item")]
/// <summary>
///		Collectable Item Object Data 
/// </summary>
public class CollectableItemData : ScriptableObject
{
	/// <summary>
	///		The amount of rotations the item will do per second 
	/// </summary>
	public float RotationsPerSecond = 150f;

	/// <summary>
	///		The amplitude of the Y axis bouncing 
	/// </summary>
	public float Amplitude = 0.3f;

	/// <summary>
	///		The frequency / Amount of times the object moves upwards and downwards 
	/// </summary>
	public float Frequency = 2f;
	
	/// <summary>
	///		Whether the item is enabled or not 
	/// </summary>
	public bool Enabled = false;

	/// <summary>
	///		The type of item the collectable is 
	/// </summary>
	public SpecialItemPickup ItemType;

	/// <summary>
	///		 Starting position offset for the item 
	/// </summary>
	public Vector3 startingPosition;
	
	/// <summary>
	///		The desired offset position to move to
	/// </summary>
	public Vector3 Offset;

	/// <summary>
	///		The amount of ammunition rounds to give to a player
	/// </summary>
	public int Rounds;

	/// <summary>
	///		The amount of hit points to give to a player
	/// </summary>
	public float HP;

}