using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TankGameEvents 
{

    // Tank Delegates 
    public delegate void OnObjectDestroyed(Transform TankDestroyed); // Handles the Tank being Destroyed 
    public delegate void ObjectTakeDamage(Transform ObjectDamaged, float amountOfDamage); // Handles the Tank Taking Damage 

    // Tank Spawn Event Delegates 
    public delegate void SpawnTanksIn(int NumberToSpawn); // Handles the Tank Spawn event 
    public delegate void OnTanksSpawned(List<GameObject> allTanksSpawnedIn); // Handles the tanks that have been spawning in 

    // Game & Round Delegates 
    public delegate void ResetGame(); // Reset the game 
    public delegate void ResetRound(); // Reset the round 

    public delegate void PreGame(); // Pre game 
    public delegate void GameStarted(); // Handles when the game launches 
    public delegate void PostRound(PlayerNumber playerNumber); // Handle after a round has completed 

    public delegate void UpdateScore(PlayerNumber playerNumber, int Amount); // Update a players score 


	#region Tank Events
	/// <summary>
	/// Called when a tank has been destroyed
	/// </summary>
	public static OnObjectDestroyed OnObjectDestroyedEvent;

    /// <summary>
    /// Called whenever damage is applied to a tank
    /// </summary>
    public static ObjectTakeDamage OnObjectTakeDamageEvent;

	/// <summary>
	/// Called when the tanks should be spawned in
	/// </summary>
	public static SpawnTanksIn SpawnTanksEvent;

    /// <summary>
    /// Called after the tanks have been spawned in
    /// </summary>
    public static OnTanksSpawned OnTanksSpawnedEvent;
	#endregion

	#region Game Mode Events  
	/// <summary>
	/// Called when the game should be reset
	/// </summary>
	public static ResetGame OnResetGameEvent;

    /// <summary>
    /// Called before our game starts might be good for set up stuff
    /// </summary>
    public static PreGame OnPreGameEvent;

    /// <summary>
    /// Called when the game begins
    /// </summary>
    public static GameStarted OnGameStartedEvent;

    /// <summary>
    /// Called when the round is over
    /// </summary>
    public static PostRound OnRoundEndedEvent;

    /// <summary>
    /// Called when a player has scored a point
    /// </summary>
    public static UpdateScore OnScoreUpdatedEvent;
   
    /// <summary>
    /// Called when the round is reset
    /// </summary>
    public static ResetRound OnRoundResetEvent;
	#endregion
}
