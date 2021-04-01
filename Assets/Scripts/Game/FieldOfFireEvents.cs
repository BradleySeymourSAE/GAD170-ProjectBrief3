using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Ctrl + m + o to close collapse class methods  


/// <summary>
///		Static Class for Handling the Field Of Fire Game Events 
/// </summary>
public static class FieldOfFireEvents
{

	public delegate void RestartGame(); // restart the game to first round  
	public delegate void ResetWave(int CurrentWaveIndex); // Resets the current wave 

	public delegate void PreGame(); // before the game starts 
	public delegate void GameStarted(); // Called to start the game 
	public delegate void PostGame(Player PlayerReference); // Called after the game has finished 

	public delegate void UpdatePlayerKillCount(Player PlayerReference, int Amount);
	public delegate void SpawnPlayer(Transform PlayerTank);
	public delegate void OnPlayerHasSpawned();

	public delegate void OnEnemyAIInfantryDeath(Transform InfantryCharacter);
	public delegate void EnemyAIRecievedDamage(Transform EnemyAIInfantry, float Damage);

	public delegate void OnTankDestroyed(Transform Tank);
	public delegate void TankReceivedDamage(Transform TankObject, float Amount);

	public delegate void SpawnEnemyWave(int NumberOfInfantry, int NumberOfEnemyTanks);
	public delegate void OnEnemyWaveSpawned(List<GameObject> EnemyAIInfantry, List<GameObject> EnemyAITanks);

 
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
	public static PreGame OnPreGameEvent;

	/// <summary>
	///		Handles what happens after the game has started 
	/// </summary>
	public static GameStarted OnGameStartedEvent;

	/// <summary>
	///		Handles that happens after the game has started 
	/// </summary>
	public static PostGame OnPostGameEvent;
	
	/// <summary>
	///		Handles updated the players kill count 
	/// </summary>
	/// <param name="PlayerReference">The current player enum</param>
	/// <param name="Amount">The kill count of the player</param>
	public static UpdatePlayerKillCount UpdatePlayerKillCountEvent;

	/// <summary>
	///		Handles the spawning of the player 
	/// </summary>
	public static SpawnPlayer SpawnPlayerEvent;

	/// <summary>
	///		Called after a player has spawned 
	/// </summary>
	public static OnPlayerHasSpawned OnPlayerHasSpawnedEvent;

	/// <summary>
	///		Event called one a tank has been eliminated 
	/// </summary>
	/// <param name="Tank">The enemy tank to destroy</param>
	public static OnTankDestroyed OnTankDestroyedEvent;

	/// <summary>
	///		Called when the tanks receives damage from an enemy 
	/// </summary>
	/// <param name="TankObject">The tank to apply the damage amount to</param>
	/// <param name="Amount">The amount of damage to apply to the tank</param>
	public static TankReceivedDamage TankReceivedDamageEvent;

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

	/// <summary>
	///		Handles the event for when an enemy ai character receives damage 
	/// </summary>
	public static EnemyAIRecievedDamage EnemyAIReceivedDamageEvent;

	/// <summary>
	///		Handles what occurs after an enemy AI Infantry character dies
	/// </summary>
	public static OnEnemyAIInfantryDeath OnEnemyAIInfantryDeathEvent;
}