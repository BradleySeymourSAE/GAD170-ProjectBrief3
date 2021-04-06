using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Ctrl + m + o to close collapse class methods  


/// <summary>
///		Static Class for Handling the Field Of Fire Game Events 
/// </summary>
public static class FireModeEvents
{

	public delegate void RestartGame(); // restart the game to first round  
	public delegate void ResetWave(); // Resets the current wave 

	public delegate void PreWave(); // before the game starts 
	public delegate void WaveStarted(); // Called to start the game 
	public delegate void WaveOver(); // Called after the wave has finished 

	public delegate void UpdateWaveCount(int Wave); // update the current wave 
	public delegate void UpdatePlayerKills(int PlayerKills);
	
	public delegate void SpawnPlayer();
	public delegate void OnPlayerSpawned();

	public delegate void OnObjectDestroyed(Transform DestroyedObject);
	public delegate void ReceivedDamage(Transform DamagedObject, float DamageAmount);

	public delegate void SpawnEnemyWave(int NumberOfInfantry, int NumberOfEnemyTanks);
	public delegate void OnEnemyWaveSpawned(List<GameObject> Enemies);

 
	/// <summary>
	///		Handles the games on restart event 
	/// </summary>
	public static RestartGame OnRestartGameEvent;
	
	/// <summary>
	///		Calls wave reset evebt 
	/// </summary>
	public static ResetWave OnResetWaveEvent;
	
	/// <summary>
	///		Handles the pre game event 
	/// </summary>
	public static PreWave OnPreWaveEvent;

	/// <summary>
	///		Handles what happens after the game has started 
	/// </summary>
	public static WaveStarted OnWaveStartedEvent;

	/// <summary>
	///		Handles that happens after the game has started 
	/// </summary>
	public static WaveOver OnWaveOverEvent;
	
	/// <summary>
	///		Handles updated the players kill count 
	/// </summary>
	/// <param name="PlayerReference">The current player enum</param>
	/// <param name="Amount">The kill count of the player</param>
	public static UpdateWaveCount OnUpdateWaveCountEvent;

	public static UpdatePlayerKills OnUpdatePlayerKillsEvent;

	/// <summary>
	///		Handles the spawning of the player 
	/// </summary>
	public static SpawnPlayer SpawnPlayerEvent;

	/// <summary>
	///		Called after a player has spawned 
	/// </summary>
	public static OnPlayerSpawned OnPlayerSpawnedEvent;

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

}