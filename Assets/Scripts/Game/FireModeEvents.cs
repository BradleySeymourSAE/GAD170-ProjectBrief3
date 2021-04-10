using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Ctrl + m + o to close collapse class methods  


/// <summary>
///		Static Class for Handling the Field Of Fire Game Events 
/// </summary>
public static class FireModeEvents
{

	#region DELEGATES 

	#region Wave & Round Delegates 

	/// <summary>
	///  What happens before the wave has started  
	/// </summary>
	public delegate void PreWave(float displayForSeconds);
	
	/// <summary>
	///		Called to start the game 
	/// </summary>
	public delegate void WaveStarted(); 
	
	/// <summary>
	///		Called once a successfully wave has been completed 
	/// </summary>
	public delegate void NextWave(); 
	
	/// <summary>
	///		Called when the game should reset.
	/// </summary>
	public delegate void ResetWave(); 

	/// <summary>
	///		Called to restart the game
	/// </summary>
	public delegate void RestartGame(); 


	#endregion

	#region Spawn Delegates 

	public delegate void SpawnPlayer();
	public delegate void OnPlayerSpawned(Transform PlayerReference);

	public delegate void SpawnPickup(int HealthPackCount, int AmmunitionPackCount);
	public delegate void OnPickupSpawned(List<GameObject> SpawnedInPickups);

	public delegate void SpawnEnemyWave(int InfantryCount, int TankCount);
	public delegate void OnEnemyAIWaveSpawned(List<GameObject> TotalSpawnedAI);

	#endregion

	#region Collision Delegates 
	
	public delegate void OnObjectDestroyed(Transform DestroyedObject);
	public delegate void ReceivedDamage(Transform DamagedObject, float DamageAmount);
	public delegate void OnObjectPickedUp(Transform Collected); // called once an item is picked up 
	
	#endregion

	#region UI Delegates 

	public delegate void UpdateWaveUI(int CurrentWave, int WaveEnemiesRemaining, int WaveEnemiesKilled); // update the current wave 
	public delegate void UpdatePlayerKills(int PlayerKills);
	public delegate void UpdatePlayerAmmunition(int AmmunitionLoaded, int TotalAmmunition);
	public delegate void UpdatePlayersHealthUI(float CurrentHealth);

	#endregion
	#endregion

	#region EVENTS 

	#region Wave Events 

	/// <summary>
	///		Handles the pre game event 
	/// </summary>
	public static PreWave OnPreWaveEvent;

	/// <summary>
	///		Handles what happens after the game has started 
	/// </summary>
	public static WaveStarted OnWaveStartedEvent;

	/// <summary>
	///		Handles that happens after the wave is over and the player is still alive  
	/// </summary>
	public static NextWave OnNextWaveEvent;

	/// <summary>
	///		Calls wave reset event 
	/// </summary>
	public static ResetWave OnResetWaveEvent;

	/// <summary>
	///		Called when the game is restarted
	/// </summary>
	public static RestartGame OnGameRestartEvent;

	#endregion

	#region UI Events 
	/// <summary>
	///		Updates the wave ui counter 
	/// </summary>
	/// <param name="PlayerReference">The current player enum</param>
	/// <param name="Amount">The kill count of the player</param>
	public static UpdateWaveUI UpdateWaveUIEvent;

	/// <summary>
	///		Once called - Updates the current players kill counter ui 
	/// </summary>
	public static UpdatePlayerKills UpdatePlayerKillsEvent;

	/// <summary>
	///		Updates the player ammunition UI
	/// </summary>
	public static UpdatePlayerAmmunition UpdatePlayerAmmunitionEvent;

	/// <summary>
	///		Updates the player's Health UI 
	/// </summary>
	public static UpdatePlayersHealthUI UpdatePlayerHealthEvent;

	#endregion

	#region Spawn Events 

	/// <summary>
	///		Handles the spawning of the player 
	/// </summary>
	public static SpawnPlayer SpawnPlayerEvent; // spawns the player 

	/// <summary>
	///		Called after a player has spawned 
	/// </summary>
	public static OnPlayerSpawned OnPlayerSpawnedEvent; // called after the player has spawned 

	/// <summary>
	///		Called to spawn in a weapon pickup 
	/// </summary>
	public static SpawnPickup SpawnPickupsEvent; // spawns the pickup item 
	
	/// <summary>
	///		Called when the weapon pickups have been spawned in 
	/// </summary>
	public static OnPickupSpawned OnPickupSpawnedEvent; // called after the pickups are spawned in 

	/// <summary>
	///		Spawns in a wave of enemies
	/// </summary>
	/// <param name="NumberOfInfantry">The amount of infantry AI to spawn in</param>
	/// <param name="NumberOfTanks">The amount of tank AI to spawn in</param>
	public static SpawnEnemyWave SpawnEnemyWaveEvent; // spawns the enemies 

	/// <summary>
	///		Handles what happens after the enemy wave has spawned in 
	/// </summary>
	/// <param name="EnemyAIInfantry">List of spawned Enemy AI Infantry</param>
	/// <param name="EnemyAITanks">List of spawned in Enemy AI Tanks</param>
	public static OnEnemyAIWaveSpawned OnEnemyAIWaveSpawnedEvent; // called after the enemies are spawned 

	#endregion

	#region Collision Events 

	/// <summary>
	///		Event called once an object has been eliminated
	/// </summary>
	/// <param name="Tank">The enemy tank to destroy</param>
	public static OnObjectDestroyed OnObjectDestroyedEvent; // called when a tank object is destroyed 

	/// <summary>
	///		Called when the tanks receives damage from an enemy 
	/// </summary>
	/// <param name="TankObject">The tank to apply the damage amount to</param>
	/// <param name="Amount">The amount of damage to apply to the tank</param>
	public static ReceivedDamage OnReceivedDamageEvent; // called when an object takes damage 

	/// <summary>
	///		Called when an object has been picked up 
	/// </summary>
	public static OnObjectPickedUp OnObjectPickedUpEvent; // called when an item is picked up 

	#endregion

	#endregion
}