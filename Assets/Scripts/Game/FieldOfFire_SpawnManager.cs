using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FieldOfFire_SpawnManager : MonoBehaviour
{

	/// <summary>
	///		Spawn Points & Starting Spawn Positions 
	/// </summary>
	public List<Transform> possibleEnemySpawnPoints = new List<Transform>(); // list of enemy spawn points 
	private List<Transform> startingPossibleEnemySpawnPoints = new List<Transform>(); // store starting spawn points 


	/// <summary>
	///		Enemy Character Prefabs (Soldiers) 
	/// </summary>
	public List<GameObject> enemyCharacterPrefabs = new List<GameObject>(); // list of possible enemy character prefabs 
	private List<GameObject> aliveEnemyCharactersSpawned = new List<GameObject>(); // The currently alive enemy characters spawned in 

	/// <summary>
	///		Enemy Tank Prefabs & Spawned in Tanks
	/// </summary>
	public List<GameObject> enemyTankPrefabs = new List<GameObject>(); // List of enemy tank prefabs to spawn in 
	private List<GameObject> aliveEnemyTanksSpawned = new List<GameObject>(); // currently alive enemy tanks spawned in 



	/// <summary>
	///		Enemy Spawn Manager Event Listeners 
	/// </summary>
	private void OnEnable()
	{
		FieldOfFireEvents.SpawnEnemyWaveEvent += SpawnEnemyWave;
		FieldOfFireEvents.OnResetWaveEvent += RestartWave;
		FieldOfFireEvents.OnRestartGameEvent += RestartGame;
	}

	/// <summary>
	///		Enemy Spawn Manager Event Listener Cleanups 
	/// </summary>
	private void OnDisable()
	{
		FieldOfFireEvents.SpawnEnemyWaveEvent -= SpawnEnemyWave;
		FieldOfFireEvents.OnResetWaveEvent -= RestartWave;
		FieldOfFireEvents.OnRestartGameEvent -= RestartGame;
	}

	/// <summary>
	///		Debugging Helper
	/// </summary>
	private void OnDrawGizmos()
	{
		for (int i = 0; i < possibleEnemySpawnPoints.Count; i++)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(possibleEnemySpawnPoints[i].position, 0.5f);
		}
	}

	/// <summary>
	///		Restarts the game 
	/// </summary>
	private void RestartGame()
	{

		for (int i = 0; i < aliveEnemyTanksSpawned.Count; i++)
		{
			Destroy(aliveEnemyTanksSpawned[i]);
		}

		for (int i = 0; i < aliveEnemyCharactersSpawned.Count; i++)
		{
			Destroy(aliveEnemyCharactersSpawned[i]);
		}

		aliveEnemyTanksSpawned.Clear(); // clear enemy tanks list 
		aliveEnemyCharactersSpawned.Clear(); // clear enemy characters list 
		startingPossibleEnemySpawnPoints.Clear(); // 


		for (int i = 0; i < possibleEnemySpawnPoints.Count; i++)
		{
			startingPossibleEnemySpawnPoints.Add(possibleEnemySpawnPoints[i]);
		}
	}

	/// <summary>
	///		Restarts the wave 
	/// </summary>
	private void RestartWave(int CurrentWaveIndex)
	{
		Debug.Log("[FieldOfFire_SpawnManager.RestartWave]: " + "Restarting Wave for index " + CurrentWaveIndex);
	}


	/// <summary>
	///		Spawns in an enemy wave
	/// </summary>
	/// <param name="AmountOfCharacters"></param>
	/// <param name="AmountOfTanks"></param>
	private void SpawnEnemyWave(int AmountOfCharacters, int AmountOfTanks)
	{
		Debug.Log("[FieldOfFire_SpawnManager.SpawnEnemyWave]: " + "Spawning Enemy Wave. Enemy AI Characters: " + AmountOfCharacters + " Enemy AI Tanks: " + AmountOfTanks);
		// Check if tank prefabs count is less than or equal to amountoftanks to spawn and that all spawnpoints.count is greater than or equal to amount of tanks to spawn 
		if (enemyCharacterPrefabs.Count >= AmountOfCharacters && enemyTankPrefabs.Count >= AmountOfTanks && possibleEnemySpawnPoints.Count >= (AmountOfCharacters + AmountOfTanks))
		{
			// Spawn in the enemy characters 
			for (int i = 0; i < AmountOfCharacters; i++)
			{		
				Transform tempCharacterSpawnPosition = startingPossibleEnemySpawnPoints[Random.Range(0, startingPossibleEnemySpawnPoints.Count)];

				GameObject clonedEnemy = Instantiate(enemyCharacterPrefabs[i], tempCharacterSpawnPosition.position, Quaternion.identity);

				startingPossibleEnemySpawnPoints.Remove(tempCharacterSpawnPosition);

				aliveEnemyCharactersSpawned.Add(clonedEnemy);
			}


			// Spawn in the enemy tanks 
			for (int i = 0; i < AmountOfTanks; i++)
			{
				Transform tempEnemyTankSpawnPosition = startingPossibleEnemySpawnPoints[Random.Range(0, startingPossibleEnemySpawnPoints.Count)];

				GameObject clonedEnemyTank = Instantiate(enemyTankPrefabs[i], tempEnemyTankSpawnPosition.position, Quaternion.identity);

				startingPossibleEnemySpawnPoints.Remove(tempEnemyTankSpawnPosition);

				aliveEnemyTanksSpawned.Add(clonedEnemyTank);
			}


		}
		else
		{
			// Error handling 
			Debug.LogError("[FieldOfFire_SpawnManager.SpawnEnemyWave]: " + "Enemy tanks and characters could not be spawned from the spawn points!");
		}


		// Tell the game our tanks have been spawned in!
		FieldOfFireEvents.OnEnemyWaveSpawnedEvent?.Invoke(aliveEnemyCharactersSpawned, aliveEnemyTanksSpawned);
	}
}
