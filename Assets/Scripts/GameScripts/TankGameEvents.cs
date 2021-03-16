using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TankGameEvents 
{
    public delegate void OnObjectDestroyed(Transform TankDestroyed);
    public delegate void ObjectTakeDamage(Transform ObjectDamaged, float amountOfDamage);

    public delegate void SpawnTanksIn(int NumberToSpawn);
    public delegate void OnTanksSpawned(List<GameObject> allTanksSpawnedIn);

    public delegate void ResetGame();
    public delegate void ResetRound();

    public delegate void PreGame();
    public delegate void GameStarted();
    public delegate void PostRound(PlayerNumber playerNumber);

    public delegate void UpdateScore(PlayerNumber playerNumber, int Amount);

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
    public static PostRound OnRoundEnededEvent;

    /// <summary>
    /// Called when a player has scored a point
    /// </summary>
    public static UpdateScore OnScoreUpdatedEvent;
   
    /// <summary>
    /// Called when the round is reset
    /// </summary>
    public static ResetRound OnRoundResetEvent;
}
