using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Ctrl + m + o to close collapse class methods  


/// <summary>
///		Static Class for Handling the Field Of Fire Game Events 
/// </summary>
public static class FireModeEvents
{

	#region Game Delegates 
	
	public delegate void RestartGame(); // restart the game to first round  
	public delegate void ResetWave(); // Resets the current wave 
	public delegate void GameOver(); // Called if the player dies, Restarts the game 

	#endregion

	#region Wave & Round Delegates 

	public delegate void PreWave(); // before the game starts 
	public delegate void WaveStarted(); // Called to start the game 
	public delegate void WaveOver(); // Called after the wave has finished 

	#endregion

	#region Spawn Delegates 
	
	public delegate void SpawnPlayer();
	public delegate void OnPlayerSpawned(Transform PlayerReference);

	public delegate void SpawnPickup(int Amount);
	public delegate void OnPickupSpawned(List<GameObject> SpawnedInPickups);

	public delegate void SpawnEnemyWave(int NumberOfInfantry, int NumberOfEnemyTanks);
	public delegate void OnEnemyWaveSpawned(List<GameObject> Enemies);

	#endregion

	#region Collision Delegates 
	
	public delegate void OnObjectDestroyed(Transform DestroyedObject);
	public delegate void ReceivedDamage(Transform DamagedObject, float DamageAmount);
	public delegate void OnObjectPickedUp(Transform Collected); // called once an item is picked up 
	
	#endregion

	#region UI Delegates 

	public delegate void UpdateWaveCount(int Wave); // update the current wave 
	public delegate void UpdatePlayerKills(int PlayerKills);
	public delegate void UpdatePlayerAmmunition(int AmmunitionLoaded, int TotalAmmunition);

	#endregion

	#region Game Mode Events 

	/// <summary>
	///		Handles the games on restart event 
	/// </summary>
	public static RestartGame OnRestartGameEvent;

	/// <summary>
	///		Called once a player dies. Restarts the game.
	/// </summary>
	public static GameOver OnGameOverEvent;

	#endregion

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
	///		Handles that happens after the wave is over 
	/// </summary>
	public static WaveOver OnWaveOverEvent;

	/// <summary>
	///		Calls wave reset event 
	/// </summary>
	public static ResetWave OnResetWaveEvent;

	#endregion

	#region UI Events 
	/// <summary>
	///		Updates the wave ui counter 
	/// </summary>
	/// <param name="PlayerReference">The current player enum</param>
	/// <param name="Amount">The kill count of the player</param>
	public static UpdateWaveCount OnUpdateWaveCountEvent;

	/// <summary>
	///		Once called - Updates the current players kill counter ui 
	/// </summary>
	public static UpdatePlayerKills OnUpdatePlayerKillsEvent;

	/// <summary>
	///		Updates the player ammunition UI
	/// </summary>
	public static UpdatePlayerAmmunition OnUpdatePlayerAmmunitionEvent;

	#endregion

	#region Spawn Events 

	/// <summary>
	///		Handles the spawning of the player 
	/// </summary>
	public static SpawnPlayer SpawnPlayerEvent;

	/// <summary>
	///		Called after a player has spawned 
	/// </summary>
	public static OnPlayerSpawned OnPlayerSpawnedEvent;

	/// <summary>
	///		Called to spawn in a weapon pickup 
	/// </summary>
	public static SpawnPickup SpawnPickupEvent;
	
	/// <summary>
	///		Called when the weapon pickups have been spawned in 
	/// </summary>
	public static OnPickupSpawned OnPickupSpawnedEvent;

	/// <summary>
	///		Spawns in a wave of enemies
	/// </summary>
	/// <param name="NumberOfInfantry">The amount of infantry AI to spawn in</param>
	/// <param name="NumberOfTanks">The amount of tank AI to spawn in</param>
	public static SpawnEnemyWave SpawnEnemyWaveEvent;

	/// <summary>
	///		Handles what happens after the enemy wave has spawned in 
	/// </summary>
	/// <param name="EnemyAIInfantry">List of spawned Enemy AI Infantry</param>
	/// <param name="EnemyAITanks">List of spawned in Enemy AI Tanks</param>
	public static OnEnemyWaveSpawned OnEnemyWaveSpawnedEvent;

	#endregion

	#region Collision Events 

	/// <summary>
	///		Event called once an object has been eliminated
	/// </summary>
	/// <param name="Tank">The enemy tank to destroy</param>
	public static OnObjectDestroyed OnObjectDestroyedEvent;

	/// <summary>
	///		Called when the tanks receives damage from an enemy 
	/// </summary>
	/// <param name="TankObject">The tank to apply the damage amount to</param>
	/// <param name="Amount">The amount of damage to apply to the tank</param>
	public static ReceivedDamage OnDamageReceivedEvent;

	public static OnObjectPickedUp OnObjectPickedUpEvent; // called when an item is picked up 

	#endregion
}