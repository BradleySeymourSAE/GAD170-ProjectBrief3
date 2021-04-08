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
	public Transform waveSpawnPosition;

	[Range(0, 10)]
	public float Magnitude = 1f;
	public float maximumXSpawnPosition = 50;
	public float maximumZSpawnPosition = 50;
	
	
	private List<GameObject> aliveEnemyAISpawned = new List<GameObject>();
	Vector3 spawnPosition;
	private Transform m_playerReference;
	int currentWave = 1;
	private float waveScalingFactor = 3f;
	int nextRoundInfantry;
	int nextRoundTanks;
	
	
	/// <summary>
	///		Enemy Spawn Manager Event Listeners 
	/// </summary>
	private void OnEnable()
	{
		FireModeEvents.SpawnPlayerEvent += SpawnPlayer; // spawns the player into the game 
		FireModeEvents.SpawnEnemyWaveEvent += SpawnEnemyWave; // spawns in the enemies 
		
		
		FireModeEvents.OnResetWaveEvent += ResetCurrentEnemyWave; // resets the current wave 
		FireModeEvents.OnRestartGameEvent += Restart; // restarts the game, sets wave to 1 
	}

	/// <summary>
	///		Enemy Spawn Manager Event Listener Cleanups 
	/// </summary>
	private void OnDisable()
	{
		FireModeEvents.SpawnPlayerEvent -= SpawnPlayer; // spawns player in the game 
		FireModeEvents.SpawnEnemyWaveEvent -= SpawnEnemyWave; // removes enemy listener  
		
		FireModeEvents.OnResetWaveEvent -= ResetCurrentEnemyWave; // Restarts the current wave 
		FireModeEvents.OnRestartGameEvent -= Restart; // Restarts the game, sets wave to 1 
	}


	private void Start()
	{
		spawnPosition = waveSpawnPosition.position;

		if (FireMode_GameManager.Instance)
		{ 
			currentWave = FireMode_GameManager.Instance.GetCurrentWave;
		}
	}


	/// <summary>
	///		Spawns a player into the game 
	/// </summary>
	private void SpawnPlayer()
	{
		GameObject currentPlayer = Instantiate(PlayerPrefab, playerSpawnPosition.position, PlayerPrefab.transform.rotation);

		if (currentPlayer.GetComponent<Transform>())
		{
			m_playerReference = currentPlayer.GetComponent<Transform>();
		}
	}

	/// <summary>
	///		Restarts the game - destroys current enemies! 
	/// </summary>
	private void Restart()
	{
		for (int i = 0; i < aliveEnemyAISpawned.Count; i++)
		{
			Destroy(aliveEnemyAISpawned[i]);
		}

		aliveEnemyAISpawned.Clear(); // clear enemies list 

		

		// Reset the wave to 1 
		currentWave = 1;
		FireModeEvents.OnUpdateWaveCountEvent?.Invoke(1);
	}

	/// <summary>
	///		Resets the enemy wave 
	/// </summary>
	private void ResetCurrentEnemyWave()
	{

		if (FireMode_GameManager.Instance)
		{
			currentWave = FireMode_GameManager.Instance.GetCurrentWave;


			Debug.Log("[FireMode_GameManager.DespawnEntity]: " + "Resetting the current wave " + currentWave);
		
		
			for (int i = 0; i < aliveEnemyAISpawned.Count; i++)
			{
				Destroy(aliveEnemyAISpawned[i]);
			}

			aliveEnemyAISpawned.Clear();

			nextRoundTanks = Mathf.RoundToInt(currentWave * waveScalingFactor);
			nextRoundInfantry = Mathf.RoundToInt(currentWave * waveScalingFactor);


			// Update the wave counter ui 
			FireModeEvents.OnUpdateWaveCountEvent?.Invoke(currentWave);
			

			// Respawn an enemy wave 
			FireModeEvents.SpawnEnemyWaveEvent?.Invoke(nextRoundInfantry, nextRoundTanks);
		}
	}

	/// <summary>
	///		Spawns in an enemy wave
	/// </summary>
	/// <param name="InfantryAmount"></param>
	/// <param name="TanksAmount"></param>
	private void SpawnEnemyWave(int InfantryAmount, int TanksAmount)
	{
		Debug.Log("[FieldOfFire_SpawnManager.SpawnEnemyWave]: " + "Spawning Enemy Wave. Enemy AI Characters: " + InfantryAmount + " Enemy AI Tanks: " + TanksAmount);
		
		// Loop through the amount of characters we need to spawn 
		for (int i = 0; i < InfantryAmount; i++)
		{
			float xPosition = Random.Range(-maximumXSpawnPosition, maximumXSpawnPosition);
			float zPosition = Random.Range(-maximumZSpawnPosition, maximumZSpawnPosition);

			Vector3 tempSpawnPosition = new Vector3(spawnPosition.x + xPosition, 0f, spawnPosition.z + zPosition);

			// Clone the game object in preparation to spawn in 
			GameObject cloneCharacter = Instantiate(InfantryPrefab, tempSpawnPosition, InfantryPrefab.transform.rotation);

			// Add the cloned character to the alive enemies list 
			aliveEnemyAISpawned.Add(cloneCharacter);
		}

		// Repeat the process for the amount of tanks we want in the round 
		for (int i = 0; i < TanksAmount; i++)
		{
			float xPosition = Random.Range(-maximumXSpawnPosition, maximumXSpawnPosition);
			float zPosition = Random.Range(-maximumZSpawnPosition, maximumZSpawnPosition);

			Vector3 tempSpawnPosition = new Vector3(spawnPosition.x + xPosition, 0f, spawnPosition.z + zPosition);

			GameObject clonedTank = Instantiate(TankPrefab, tempSpawnPosition, TankPrefab.transform.rotation);

			aliveEnemyAISpawned.Add(clonedTank);
		}


		// Tell the game our tanks have been spawned in, what do we do with them? 
		FireModeEvents.OnEnemyAIWaveSpawnedEvent?.Invoke(aliveEnemyAISpawned);
	}
}
