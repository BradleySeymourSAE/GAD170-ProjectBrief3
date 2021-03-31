using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		Static Class for Handling the Field Of Fire Game Events 
/// </summary>
public static class FieldOfFireEvents
{

	#region Event Delegates
	// Game Mode Events 
	public delegate void RestartGame(); // restart the game to first round  
	public delegate void ResetWave(); // Resets the current wave 

	public delegate void PreGame();
	public delegate void GameStarted();
	public delegate void PostGame(Player PlayerReference);

	public delegate void UpdatePlayerKillCount(Player PlayerScoreToUpdate, int Amount);

	// Player Spawn Events 
	public delegate void SpawnPlayer(Transform PlayerTank);
	public delegate void OnPlayerHasSpawned();

	// Tanks 
	public delegate void OnTankDestroyed(Transform Tank);
	public delegate void ReceiveDamage(Transform ObjectReceivedDamage, float Amount);

	// Tank Spawn Events 
	public delegate void SpawnEnemyWave(int NumberOfInfantry, int NumberOfEnemyTanks);
	public delegate void OnEnemyWaveSpawned(List<GameObject> EnemyAICharacters, List<GameObject> EnemyAITanks);

	#endregion


	// TODO: Need to complete these events
	#region Game Mode Events  




	#endregion

	#region Player Events | UI Events  



	#endregion

	#region Enemy AI events 

	#endregion
}