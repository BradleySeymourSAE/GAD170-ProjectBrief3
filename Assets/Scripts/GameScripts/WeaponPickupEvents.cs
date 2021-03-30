using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class WeaponPickupEvents
{ 

	// Weapon Pickup Delegates 
	public delegate void SpawnWeaponPickupsIn(int AmountOfItems); // Handles when the item is destroyed 
	public delegate void OnWeaponPickupsSpawned(List<GameObject> allWeaponPickupsSpawnedIn); // handle the weapon pickups spawned in 


	// Collecting weapon pickups 
	public delegate void CollectWeaponPickup(Transform WeaponPickup); // Handles when the weapon pickup is collected 
	public delegate void OnWeaponPickupCollected(Transform WeaponPickupDestroyed); // Handles the weapon pickup event
		
	public delegate void PreGame();
	public delegate void GameStarted();



	/// <summary>
	///		Handles the spawning of weapon pickups 
	/// </summary>
	public static SpawnWeaponPickupsIn SpawnWeaponPickupsEvent;
	
	/// <summary>
	///		Handles what happens after a weapon pickup is spawned in 
	/// </summary>
	public static OnWeaponPickupsSpawned OnWeaponPickupSpawnedEvent;

	/// <summary>
	///		Handles what happens when a weapon is picked up 
	/// </summary>
	public static CollectWeaponPickup CollectWeaponPickupEvent;

	/// <summary>
	///		Handles what happens after a weapon is collected / destroyed 
	/// </summary>
	public static OnWeaponPickupCollected OnWeaponPickupCollectedEvent;

	/// <summary>
	///		Handles what happens before the game starts 
	/// </summary>
	public static PreGame OnPreGameEvent;

	/// <summary>
	///  Handles what happens when the game has started 
	/// </summary>
	public static GameStarted OnGameStartedEvent;

}