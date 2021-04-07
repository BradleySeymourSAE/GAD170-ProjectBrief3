using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Ventiii.DevelopmentTools;


[System.Serializable]
public class FireMode_SpawnManager : MonoBehaviour
{
	
	public GameObject PlayerPrefab; // Player Tank Prefab 
	public GameObject TankPrefab; // Enemy Tank AI Prefab 
	public GameObject InfantryPrefab; // Enemy Infantry AI Prefab 

	public Transform playerSpawnPosition;

	[Range(50, 500)]
	public float maximumXSpawnPosition = 50;
	[Range(50, 500)]
	public float maximumYSpawnPosition = 50;
	public List<GameObject> aliveEnemiesSpawned = new List<GameObject>();

	[SerializeField] private int currentWave;
	private Tank m_playerReference;
	private float waveScalingFactor = 3;
	/// <summary>
	///		Enemy Spawn Manager Event Listeners 
	/// </summary>
	private void OnEnable()
	{
		FireModeEvents.SpawnPlayerEvent += SpawnPlayer;
		FireModeEvents.SpawnEnemyWaveEvent += SpawnEnemyWave; // spawns in the enemies 
		
		
		FireModeEvents.OnResetWaveEvent += ResetWave; // resets the current wave 
		FireModeEvents.OnRestartGameEvent += Restart; // restarts the game, sets wave to 1 
	}

	/// <summary>
	///		Enemy Spawn Manager Event Listener Cleanups 
	/// </summary>
	private void OnDisable()
	{
		FireModeEvents.SpawnPlayerEvent -= SpawnPlayer;
		FireModeEvents.SpawnEnemyWaveEvent -= SpawnEnemyWave; // removes enemy listener  
		
		FireModeEvents.OnResetWaveEvent -= ResetWave; // Restarts the current wave 
		FireModeEvents.OnRestartGameEvent -= Restart; // Restarts the game, sets wave to 1 
	}


	private void Start()
	{
		if (FireMode_GameManager.Instance)
		{ 
			currentWave = FireMode_GameManager.Instance.GetCurrentWave;
		}
	}

	private void SpawnPlayer()
	{

		if (FindObjectOfType<Tank>() != null)
		{
			m_playerReference = FindObjectOfType<Tank>();

			GameObject player = m_playerReference.gameObject;

			Destroy(player);
		}

		Instantiate(PlayerPrefab, playerSpawnPosition.position, PlayerPrefab.transform.rotation);

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

		// Reset the wave to 1 
		FireModeEvents.OnUpdateWaveCountEvent?.Invoke(1);
	}

	/// <summary>
	///		Restarts the wave 
	/// </summary>
	private void ResetWave()
	{

		if (FireMode_GameManager.Instance)
		{
			int currentWaveIndex = FireMode_GameManager.Instance.GetCurrentWave;

			currentWave = currentWaveIndex;

			Debug.Log("[FireMode_GameManager.DespawnEntity]: " + "Resetting the current wave " + currentWaveIndex);
		
		
			for (int i = 0; i < aliveEnemiesSpawned.Count; i++)
			{
				Destroy(aliveEnemiesSpawned[i]);
			}

			aliveEnemiesSpawned.Clear();

			int newTanks = (int)(Random.Range(1f, currentWave) + waveScalingFactor);
			int newInfantry = (int)(Random.Range(1, currentWave) + waveScalingFactor);


			// Invoke the Wave 
			FireModeEvents.SpawnEnemyWaveEvent?.Invoke(newInfantry, newTanks);
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

			float xPosition = Random.Range(1f, maximumXSpawnPosition);
			float yPosition = Random.Range(1f, maximumYSpawnPosition);

			Vector3 tempSpawnPoint = new Vector3(xPosition, 0, yPosition);


			// Clone the game object in preparation to spawn in 
			GameObject cloneCharacter = Instantiate(InfantryPrefab, tempSpawnPoint, InfantryPrefab.transform.rotation);

			// Add the cloned character to the alive enemies list 
			aliveEnemiesSpawned.Add(cloneCharacter);
		}

		// Repeat the process for the amount of tanks we want in the round 
		for (int i = 0; i < AmountOfTanks; i++)
		{
			// Random rand = Utility.GetRandomIndex();

			// int randomSpawnIndex = rand.Next(possibleSpawnPoints.Count);

			// Transform temporarySpawn = possibleSpawnPoints[randomSpawnIndex];

		//	GameObject clonedTank = Instantiate(TankPrefab, temporarySpawn.position, TankPrefab.transform.rotation);

			float xPosition = Random.Range(1, maximumXSpawnPosition);
			float yPosition = Random.Range(1, maximumYSpawnPosition);

			Vector3 temporarySpawnPosition = new Vector3(xPosition, 0, yPosition);

			GameObject clonedTank = Instantiate(TankPrefab, temporarySpawnPosition, TankPrefab.transform.rotation);

			// possibleSpawnPoints.Remove(temporarySpawn);

			aliveEnemiesSpawned.Add(clonedTank);
		}


		// Tell the game our tanks have been spawned in, what do we do with them? 
		FireModeEvents.OnEnemyWaveSpawnedEvent?.Invoke(aliveEnemiesSpawned);
	}
}
