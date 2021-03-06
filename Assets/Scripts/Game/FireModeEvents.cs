using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Ctrl + m + o to close collapse class methods  


/// <summary>
///		Static Class for Handling the Field Of Fire Game Events 
/// </summary>
public static class FireModeEvents
{

	/// <summary>
	///		Handles No Params Delegates 
	/// </summary>
	public delegate void VoidDelegate();

	/// <summary>
	///		Integer Paramater Delegate 
	/// </summary>
	/// <param name="Amount"></param>
	public delegate void IntParameterDelegate(int Amount);

	/// <summary>
	///		Delegate for Float parameter events 
	/// </summary>
	/// <param name="Amount"></param>
	public delegate void FloatParameterDelegate(float Amount);

	/// <summary>
	///		Transform Float Parameter Delegate 
	/// </summary>
	/// <param name="Transform"></param>
	/// <param name="Amount"></param>
	public delegate void TransformFloatParameterDelegate(Transform Transform, float Amount);

	/// <summary>
	///		Transform & Int Parameter Delegate 
	/// </summary>
	/// <param name="Transform"></param>
	/// <param name="Amount"></param>
	public delegate void TransformIntParameterDelegate(Transform Transform, int Amount);

	/// <summary>
	///		Spawn a set amount of health packs and ammunition packs into the game 
	/// </summary>
	/// <param name="HealthPackCount"></param>
	/// <param name="AmmunitionPackCount"></param>
	public delegate void SpawnGameItems(int HealthPackCount, int AmmunitionPackCount);
	
	/// <summary>
	///		Handles the list of items we have spawned 
	/// </summary>
	/// <param name="SpawnedInPickups"></param>
	public delegate void HandleOnGameItemsSpawned(List<GameObject> SpawnedInPickups);

	/// <summary>
	///		Handle the AI that we have spawned in 
	/// </summary>
	/// <param name="TotalSpawnedAI"></param>
	public delegate void HandleOnAISpawned(List<GameObject> TotalSpawnedAI);

	/// <summary>
	///		Handles when a player has been spawned in 
	/// </summary>
	/// <param name="Player"></param>
	public delegate void HandleOnPlayerSpawned(Transform Player);

	/// <summary>
	///		Called to handle when an AI character has been destroyed 
	/// </summary>
	/// <param name="AI"></param>
	public delegate void HandleAIDestroyed(Transform AI);

	/// <summary>
	///		When a game object has been collected / destroyed 
	/// </summary>
	/// <param name="Item"></param>
	public delegate void HandleGameObjectDestroyed(GameObject Item);


	/// <summary>
	///		Handles the pre game event 
	/// </summary>
	public static VoidDelegate PreGameStartedEvent;

	/// <summary>
	///		Handles what happens after the game has started 
	/// </summary>
	public static VoidDelegate GameStartedEvent;

	/// <summary>
	///		Handles that happens after the wave is over and the player is still alive  
	/// </summary>
	public static VoidDelegate HandleNextWaveStarted;

	/// <summary>
	///		Calls wave reset event 
	/// </summary>
	public static VoidDelegate ResetGameEvent;

	/// <summary>
	///		Called when the game is restarted
	/// </summary>
	public static VoidDelegate HardResetFireMode;

	/// <summary>
	///		Handles when the player wins 
	/// </summary>
	public static VoidDelegate PlayerWinsEvent;

	/// <summary>
	///		Handles when the AI wins 
	/// </summary>
	public static VoidDelegate AIWinsEvent;



	/// <summary>
	///		Increases the current wave! 
	/// </summary>
	public static IntParameterDelegate IncreaseWaveEvent;

	/// <summary>
	///		Increase the current wave index UI 
	/// </summary>
	public static IntParameterDelegate IncreaseWaveEventUI;

	/// <summary>
	///		Increases the current wave IN GAME UI 
	/// </summary>
	public static IntParameterDelegate IncreaseCurrentWaveEventUI;

	/// <summary>
	///		Increases the players score! 
	/// </summary>
	public static IntParameterDelegate IncreasePlayerScoreEvent;

	/// <summary>
	///		Updates the players current kill count and sets the UI 
	/// </summary>
	public static IntParameterDelegate IncreasePlayerScoreEventUI;

	/// <summary>
	///		Increases or decreases the players health 
	/// </summary>
	public static TransformFloatParameterDelegate IncreasePlayerHealthEvent;

	/// <summary>
	///		Sets the players health ui 
	/// </summary>
	public static FloatParameterDelegate IncreasePlayerHealthEventUI;


	/// <summary>
	///		Increases / decreases the players ammunition 
	/// </summary>
	public static TransformIntParameterDelegate IncreasePlayerAmmunitionEvent;

	/// <summary>
	///		Updates the players ammunition UI 
	/// </summary>
	public static IntParameterDelegate IncreaseAmmunitionEventUI;

	/// <summary>
	///		Handles when our character has been granted or had a life removed 
	/// </summary>
	public static IntParameterDelegate IncreaseLivesEvent;

	/// <summary>
	///		Increases or decreases the current players lives remaining UI 
	/// </summary>
	public static IntParameterDelegate IncreaseLivesEventUI;

	/// <summary>
	///		Increases or decreases the amount of enemies remaining 
	/// </summary>
	public static IntParameterDelegate IncreaseEnemiesRemainingEvent;


	/// <summary>
	///		Handles changing the UI to reflect the enemies remaining in the game
	/// </summary>
	public static IntParameterDelegate IncreaseEnemiesRemainingEventUI;


	/// <summary>
	///		Handles the spawning of the player 
	/// </summary>
	public static VoidDelegate SpawnPlayerEvent; 

	/// <summary>
	///		Called after a player has spawned 
	/// </summary>
	public static HandleOnPlayerSpawned HandleOnPlayerSpawnedEvent; 

	/// <summary>
	///		Handle when a player receives damage from an enemy 
	/// </summary>
	public static TransformFloatParameterDelegate HandlePlayerDamageEvent;


	/// <summary>
	///		Called to spawn in a weapon pickup 
	/// </summary>
	public static SpawnGameItems SpawnGameItemsEvent; 
	
	/// <summary>
	///		Called when the weapon pickups have been spawned in 
	/// </summary>
	public static HandleOnGameItemsSpawned HandleGameItemsSpawnedEvent;

	/// <summary>
	///		Called when a game item has been destroyed 
	/// </summary>
	public static HandleGameObjectDestroyed HandleOnGameItemDestroyed;


	/// <summary>
	///		Spawns in a wave of enemies
	/// </summary>
	/// <param name="NumberOfInfantry">The amount of infantry AI to spawn in</param>
	/// <param name="NumberOfTanks">The amount of tank AI to spawn in</param>
	public static IntParameterDelegate SpawnAIEvent;

	/// <summary>
	///		Handles what happens after the enemy wave has spawned in 
	/// </summary>
	/// <param name="EnemyAIInfantry">List of spawned Enemy AI Infantry</param>
	/// <param name="EnemyAITanks">List of spawned in Enemy AI Tanks</param>
	public static HandleOnAISpawned HandleOnAISpawnedEvent;


	/// <summary>
	///		Handles what happens when an AI character has been damaged 
	/// </summary>
	public static TransformFloatParameterDelegate HandleAIDamageEvent;

	/// <summary>
	///		Handles when an AI character has been destroyed 
	/// </summary>
	public static HandleAIDestroyed HandleAIDestroyedEvent;

}