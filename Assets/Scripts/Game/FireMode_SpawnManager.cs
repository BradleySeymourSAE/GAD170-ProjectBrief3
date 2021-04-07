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

	public float maximumXSpawnPosition = 50;
	public float maximumZSpawnPosition = 50;
	
	
	[SerializeField] private List<GameObject> aliveEnemiesSpawned = new List<GameObject>();


	private Vector3 spawnPosition;
	private Tank m_playerReference;

	[SerializeField] private int currentWave = 1;
	[SerializeField] private float waveScalingFactor = 3f;
	[SerializeField] private int nextRoundInfantry;
	[SerializeField] private int nextRoundTanks;
	
	
	/// <summary>
	///		Enemy Spawn Manager Event Listeners 
	/// </summary>
	private void OnEnable()
	{
		FireModeEvents.SpawnPlayerEvent += SpawnPlayer; // spawns the player into the game 
		FireModeEvents.SpawnEnemyWaveEvent += SpawnEnemyWave; // spawns in the enemies 
		
		
		FireModeEvents.OnResetWaveEvent += ResetWave; // resets the current wave 
		FireModeEvents.OnRestartGameEvent += Restart; // restarts the game, sets wave to 1 
	}

	/// <summary>
	///		Enemy Spawn Manager Event Listener Cleanups 
	/// </summary>
	private void OnDisable()
	{
		FireModeEvents.SpawnPlayerEvent -= SpawnPlayer; // spawns player in the game 
		FireModeEvents.SpawnEnemyWaveEvent -= SpawnEnemyWave; // removes enemy listener  
		
		FireModeEvents.OnResetWaveEvent -= ResetWave; // Restarts the current wave 
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

	private void SpawnPlayer()
	{

		if (FindObjectOfType<Tank>())
		{
			m_playerReference = FindObjectOfType<Tank>();
			GameObject playerRef = m_playerReference.gameObject;
			
			Destroy(playerRef);
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

			nextRoundTanks = Mathf.RoundToInt(currentWave * waveScalingFactor);
			nextRoundInfantry = Mathf.RoundToInt(currentWave * waveScalingFactor);


			FireModeEvents.OnUpdateWaveCountEvent?.Invoke(currentWave);
			// Invoke the Wave 
			FireModeEvents.SpawnEnemyWaveEvent?.Invoke(nextRoundInfantry, nextRoundTanks);
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
			float xPosition = Random.Range(-maximumXSpawnPosition, maximumXSpawnPosition);
			float zPosition = Random.Range(-maximumZSpawnPosition, maximumZSpawnPosition);

			Vector3 tempSpawnPosition = new Vector3(spawnPosition.x + xPosition, 0f, spawnPosition.z + zPosition);

			// Clone the game object in preparation to spawn in 
			GameObject cloneCharacter = Instantiate(InfantryPrefab, tempSpawnPosition, InfantryPrefab.transform.rotation);

			// Add the cloned character to the alive enemies list 
			aliveEnemiesSpawned.Add(cloneCharacter);
		}

		// Repeat the process for the amount of tanks we want in the round 
		for (int i = 0; i < AmountOfTanks; i++)
		{
			float xPosition = Random.Range(-maximumXSpawnPosition, maximumXSpawnPosition);
			float zPosition = Random.Range(-maximumZSpawnPosition, maximumZSpawnPosition);

			Vector3 tempSpawnPosition = new Vector3(spawnPosition.x + xPosition, 0f, spawnPosition.z + zPosition);

			GameObject clonedTank = Instantiate(TankPrefab, tempSpawnPosition, TankPrefab.transform.rotation);

			aliveEnemiesSpawned.Add(clonedTank);
		}


		// Tell the game our tanks have been spawned in, what do we do with them? 
		FireModeEvents.OnEnemyWaveSpawnedEvent?.Invoke(aliveEnemiesSpawned);
	}
}
