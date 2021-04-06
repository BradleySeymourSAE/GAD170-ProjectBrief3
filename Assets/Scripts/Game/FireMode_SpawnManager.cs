using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using Ventiii.DevelopmentTools;


[System.Serializable]
public class FireMode_SpawnManager : MonoBehaviour
{

	/// <summary>
	///		Spawn Points & Starting Spawn Positions 
	/// </summary>
	public List<Transform> SpawnPoints = new List<Transform>(); // list of enemy spawn points 
	private List<Transform> possibleSpawnPoints = new List<Transform>(); // store starting spawn points 
	
	
	public GameObject TankPrefab;
	public GameObject InfantryPrefab;
	public List<GameObject> aliveEnemiesSpawned = new List<GameObject>();

	private int currentWave;
	/// <summary>
	///		Enemy Spawn Manager Event Listeners 
	/// </summary>
	private void OnEnable()
	{
		FireModeEvents.SpawnEnemyWaveEvent += SpawnEnemyWave; // spawns in the enemies 
		
		
		FireModeEvents.OnResetWaveEvent += ResetWave; // resets the current wave 
		FireModeEvents.OnRestartGameEvent += Restart; // restarts the game, sets wave to 1 
	}

	/// <summary>
	///		Enemy Spawn Manager Event Listener Cleanups 
	/// </summary>
	private void OnDisable()
	{
		FireModeEvents.SpawnEnemyWaveEvent -= SpawnEnemyWave; // removes enemy listener  
		
		
		FireModeEvents.OnResetWaveEvent -= ResetWave; // Restarts the current wave 
		FireModeEvents.OnRestartGameEvent -= Restart; // Restarts the game, sets wave to 1 
	}

	/// <summary>
	///		Debugging
	/// </summary>
	private void OnDrawGizmos()
	{
		for (int i = 0; i < SpawnPoints.Count; i++)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(SpawnPoints[i].position, 0.5f);
		}
	}

	/// <summary>
	///		Restarts the game - destroys current enemies! 
	/// </summary>
	private void Restart()
	{
		for (int i = 0; i < aliveEnemiesSpawned.Count; i++)
		{
			Destroy(aliveEnemiesSpawned[i]);
		}

		aliveEnemiesSpawned.Clear(); // clear enemies list 
		possibleSpawnPoints.Clear(); // 


		for (int i = 0; i < SpawnPoints.Count; i++)
		{
			possibleSpawnPoints.Add(SpawnPoints[i]);
		}
	}

	/// <summary>
	///		Restarts the wave 
	/// </summary>
	private void ResetWave()
	{

		if (FireMode_GameManager.Instance != null)
		{
			int currentWaveIndex = FireMode_GameManager.Instance.GetCurrentWave();

			currentWave = currentWaveIndex;

			Debug.Log("[FireMode_GameManager.DespawnEntity]: " + "Resetting the current wave " + currentWaveIndex);
		
		
			for (int i = 0; i < aliveEnemiesSpawned.Count; i++)
			{
				Destroy(aliveEnemiesSpawned[i]);
			}

			aliveEnemiesSpawned.Clear();
			possibleSpawnPoints.Clear();

			for (int i = 0; i < SpawnPoints.Count; i++)
			{
				possibleSpawnPoints.Add(SpawnPoints[i]);
			}
		}
	}


	/// <summary>
	///		Spawns in an enemy wave
	/// </summary>
	/// <param name="AmountOfInfantry"></param>
	/// <param name="AmountOfTanks"></param>
	private void SpawnEnemyWave(int AmountOfInfantry, int AmountOfTanks)
	{
		Debug.Log("[FieldOfFire_SpawnManager.SpawnEnemyWave]: " + "Spawning Enemy Wave. Enemy AI Characters: " + AmountOfInfantry + " Enemy AI Tanks: " + AmountOfTanks);
		
		// Loop through the amount of characters we need to spawn 
		for (int i = 0; i < AmountOfInfantry; i++)
		{
			// Get a random index 
			Random rand = Utility.GetRandomIndex();

			// random spawn point index 
			int randomSpawnPointIndex = rand.Next(possibleSpawnPoints.Count);

			// Create a temporary spawn point position within the possible spawn points array 
			Transform temporarySpawnPoint = possibleSpawnPoints[randomSpawnPointIndex];

			// Clone the game object in preparation to spawn in 
			GameObject cloneCharacter = Instantiate(InfantryPrefab, temporarySpawnPoint.position, InfantryPrefab.transform.rotation);

			// remove temp spawn point from the list 
			possibleSpawnPoints.Remove(temporarySpawnPoint);

			// Add the cloned character to the alive enemies list 
			aliveEnemiesSpawned.Add(cloneCharacter);
		}

		// Repeat the process for the amount of tanks we want in the round 
		for (int i = 0; i < AmountOfTanks; i++)
		{
			Random rand = Utility.GetRandomIndex();

			int randomSpawnIndex = rand.Next(possibleSpawnPoints.Count);

			Transform temporarySpawn = possibleSpawnPoints[randomSpawnIndex];

			GameObject clonedTank = Instantiate(TankPrefab, temporarySpawn.position, TankPrefab.transform.rotation);

			possibleSpawnPoints.Remove(temporarySpawn);

			aliveEnemiesSpawned.Add(clonedTank);
		}


		foreach (GameObject _enemy in aliveEnemiesSpawned)
		{
			// Debugging 
			Debug.Log("[FireMode_SpawnManager.SpawnEnemyWave]: " + _enemy.name);
		}


		// Tell the game our tanks have been spawned in, what do we do with them? 
		FireModeEvents.OnEnemyWaveSpawnedEvent?.Invoke(aliveEnemiesSpawned);
	}
}
