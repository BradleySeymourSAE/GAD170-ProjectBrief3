#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Ventiii.DevelopmentTools;
#endregion


/// <summary>
///		Handles spawning Enemy AI, Player & Collectable Items 
///		Events called from FireModeEvents
/// </summary>
[System.Serializable]
public class FireMode_SpawnManager : MonoBehaviour
{

	#region Static 
	/// <summary>
	///		The Spawn Manager instance 
	/// </summary>
	public static FireMode_SpawnManager FireModeSpawnManager;
	#endregion

	#region Public Variables 

	/// <summary>
	///		The Main Player Prefab 
	/// </summary>
	[Header("Player & Enemy AI Prefabs")]
	public GameObject PlayerPrefab; 

	/// <summary>
	///		The Enemy AI Tank Prefab 
	/// </summary>
	public GameObject TankPrefab;

	/// <summary>
	///		The Enemy AI Infantry Prefab 
	/// </summary>
	public GameObject InfantryPrefab; // Enemy Infantry AI Prefab 
	
	[Header("Collectable Items")]
	/// <summary>
	///		The Health Pack Prefab 
	/// </summary>
	public GameObject HealthPackPrefab;

	/// <summary>
	///		The Ammunition Pack Prefab 
	/// </summary>
	public GameObject AmmunitionPackPrefab;

	/// <summary>
	///		The player's Spawn Point 
	/// </summary>
	[Header("Spawn Point Locations")]
	public Transform playerSpawn;

	/// <summary>
	///		 The Enemy AI's Spawn Point 
	/// </summary>
	public Transform enemyAISpawn;

	/// <summary>
	///		The Collectable Item Spawn 
	/// </summary>
	public Transform itemPickupSpawn;

	/// <summary>
	///		The wave spawn maximum range 
	/// </summary>
	[Header("AI Wave Spawn Settings")]
	public float waveSpawnRange = 250f;
	/// <summary>
	///		Offset Y Spawn Position 
	/// </summary>
	[Range(0, 1)]
	public float waveSpawnYPositionOffset;

	/// <summary>
	///		Wave Spawn Scaling Magnitude  
	/// </summary>
	public float magnitude = 1;
	
	/// <summary>
	///		Maximum Item Spawn Range 
	/// </summary>
	[Header("Collectable Item Settings")]
	public float itemSpawnRange = 100f;
	
	/// <summary>
	///		Maximum Item's Y Positional Offset 
	/// </summary>
	[Range(0, 1)]
	public float itemSpawnYPositionOffset = 0.35f;
	#endregion

	#region Private Variables 

	/// <summary>
	///		The minimum range for spawning enemies 
	/// </summary>
	private float minimumWaveSpawnRange = 50f;

	/// <summary>
	///		The position of the wave spawn point 
	/// </summary>
	private Vector3 m_WaveSpawnPoint;


	/// <summary>
	///		The position of the item's spawn point 
	/// </summary>
	private Vector3 m_ItemSpawnPoint;

	/// <summary>
	///		The minimum range for spawning items 
	/// </summary>
	private float minimumItemSpawnRange = 15;
	
	/// <summary>
	///		List of spawned enemies 
	/// </summary>
	private List<GameObject> m_SpawnedAIEnemies = new List<GameObject>();
	
	/// <summary>
	///		List of spawned collectable items 
	/// </summary>
	private List<GameObject> m_SpawnedCollectableItems = new List<GameObject>();
	
	/// <summary>
	///		Reference to the current player instance 
	/// </summary>
	private Transform m_PlayerReference;

	#endregion

	#region Unity Methods  
	/// <summary>
	///		Enemy Spawn Manager Event Listeners 
	/// </summary>
	private void OnEnable()
	{
		FireModeEvents.SpawnPlayerEvent += SpawnPlayer; // spawns the player into the game 
		FireModeEvents.SpawnEnemyWaveEvent += SpawnEnemyWave; // spawns in the enemies 
		FireModeEvents.SpawnPickupsEvent += SpawnCollectableItems; // spawns in the collectable items


		FireModeEvents.OnGameRestartEvent += HandleOnReset; // on game restart 
		FireModeEvents.OnResetWaveEvent += HandleOnReset; // Resets the current wave 
	}

	/// <summary>
	///		Enemy Spawn Manager Event Listener Cleanups 
	/// </summary>
	private void OnDisable()
	{
		FireModeEvents.SpawnPlayerEvent -= SpawnPlayer; // spawns player in the game 
		FireModeEvents.SpawnEnemyWaveEvent -= SpawnEnemyWave; // removes enemy listener  
		FireModeEvents.SpawnPickupsEvent -= SpawnCollectableItems; // removes event listener 


		FireModeEvents.OnGameRestartEvent -= HandleOnReset; // restarts the game 
		FireModeEvents.OnResetWaveEvent -= HandleOnReset; // Restarts the current wave 
	}

	/// <summary>
	///		Gets references for the spawn manager instance and spawn points
	/// </summary>
	private void Awake()
	{
		if (FireModeSpawnManager != null)
		{
			Destroy(gameObject);
		}
		else
		{
			FireModeSpawnManager = this;
			DontDestroyOnLoad(gameObject);
		}

		//	Check for Item Pickup spawn transform 
		if (itemPickupSpawn.GetComponent<Transform>())
		{
			// Set the default spawn position here 
			m_ItemSpawnPoint = itemPickupSpawn.position;
		}
		else
		{
			Debug.LogError("[FireMode_SpawnManager.Awake]: " + "Could not find Item Pickup Spawn Point!");
		}

		//	Check for AI spawn transform 
		if (enemyAISpawn.GetComponent<Transform>())
		{
			m_ItemSpawnPoint = enemyAISpawn.position;
		}
		else
		{
			Debug.LogError("[FireMode_SpawnManager.Awake]: " + "Could not find Enemy Wave Spawn Point!");
		}
	}

	#endregion

	#region Spawn Manager Methods 
	/// <summary>
	///		Spawns a player into the game 
	/// </summary>
	private void SpawnPlayer()
	{
		GameObject SpawnedPlayer = Instantiate(PlayerPrefab, playerSpawn.position, PlayerPrefab.transform.rotation);

		if (SpawnedPlayer.GetComponent<MainPlayerTank>())
		{
			Debug.Log("Spawning in the player!");
			m_PlayerReference = SpawnedPlayer.transform;
		
			// Check for audio listener 
			if (!m_PlayerReference.GetComponent<AudioListener>())
			{
				// Add one if it doesnt exist.
				Debug.Log("Adding audio listener!");
				SpawnedPlayer.AddComponent<AudioListener>();
			}
		
		}

		// Invoke the spawning of the player ref 
		FireModeEvents.OnPlayerSpawnedEvent?.Invoke(m_PlayerReference);
	}

	/// <summary>
	///		Resets the enemy wave 
	/// </summary>
	private void HandleOnReset()
	{
		Debug.LogWarning("[FireMode_SpawnManager.HandleOnReset]: " + "Handling Reset! Removing spawned in game objects from the scene");


		// Destroy all the currently spawned in Enemy AI game objects 
		for (int i = 0; i < m_SpawnedAIEnemies.Count; i++)
		{
				Destroy(m_SpawnedAIEnemies[i]);
		}

			// Loop through and destroy all the currently spawned in item pickups 
		for (int i = 0; i < m_SpawnedCollectableItems.Count; i++)
		{
				Destroy(m_SpawnedCollectableItems[i]);
		}

			// Clear the spawned in enemy AI & item pickup lists. 
			m_SpawnedAIEnemies.Clear();
			m_SpawnedCollectableItems.Clear();
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
			float xPosition = Mathf.Round(Random.Range(-minimumWaveSpawnRange, waveSpawnRange) * magnitude);
			float zPosition = Mathf.Round(Random.Range(-minimumWaveSpawnRange, waveSpawnRange) * magnitude);

			Vector3 tempInfantrySpawn = new Vector3(m_WaveSpawnPoint.x + xPosition, waveSpawnYPositionOffset, m_WaveSpawnPoint.z + zPosition);

			// Clone the game object in preparation to spawn in 
			GameObject infantryClone = Instantiate(InfantryPrefab, tempInfantrySpawn, InfantryPrefab.transform.rotation);

			// Add the cloned character to the alive enemies list 
			m_SpawnedAIEnemies.Add(infantryClone);
		}

		// Repeat the process for the amount of tanks we want in the round 
		for (int i = 0; i < TanksAmount; i++)
		{
			float xPosition = Mathf.Round(Random.Range(-minimumWaveSpawnRange, waveSpawnRange) * magnitude);
			float zPosition = Mathf.Round(Random.Range(-minimumWaveSpawnRange, waveSpawnRange) * magnitude);

			Vector3 tempSpawnPosition = new Vector3(m_WaveSpawnPoint.x + xPosition, 0f, m_WaveSpawnPoint.z + zPosition);

			GameObject tankClone = Instantiate(TankPrefab, tempSpawnPosition, TankPrefab.transform.rotation);

			m_SpawnedAIEnemies.Add(tankClone);
		}


		// Now we have all the enemy ai spawned in we then handle what to do with the new enemies that have been spawned 
		FireModeEvents.OnEnemyAIWaveSpawnedEvent?.Invoke(m_SpawnedAIEnemies);
	}

	/// <summary>
	///		Spawns in collectable items 
	/// </summary>
	/// <param name="HealthPacks">Amount of health packs to spawn in</param>
	/// <param name="AmmunitionPacks">Amount of ammunition packs to spawn in</param>
	private void SpawnCollectableItems(int HealthPacks, int AmmunitionPacks)
	{
		// Create health packs and set their spawn points randomly 
		for (int i = 0; i < HealthPacks; i++)
		{
			float xPosition = Random.Range(-minimumItemSpawnRange, itemSpawnRange);
			float zPosition = Random.Range(-minimumItemSpawnRange, itemSpawnRange);
			
			Vector3 newSpawnPoint = new Vector3(m_ItemSpawnPoint.x + xPosition, itemSpawnYPositionOffset, m_ItemSpawnPoint.z + zPosition);

			GameObject _health = Instantiate(HealthPackPrefab, newSpawnPoint, HealthPackPrefab.transform.rotation);
			
			m_SpawnedCollectableItems.Add(_health);
		}

		// Create ammunition packs and set their spawn points randomly.
		for (int i = 0; i < AmmunitionPacks; i++)
		{
			float x = Random.Range(-minimumItemSpawnRange, itemSpawnRange);
			float z = Random.Range(-minimumItemSpawnRange, itemSpawnRange);

			Vector3 newSpawnPoint = new Vector3(m_ItemSpawnPoint.x + x, itemSpawnYPositionOffset, m_ItemSpawnPoint.z + z);

			GameObject _ammo = Instantiate(AmmunitionPackPrefab, newSpawnPoint, AmmunitionPackPrefab.transform.rotation);

			m_SpawnedCollectableItems.Add(_ammo);
		}

		
		Debug.LogWarning("[FireMode_SpawnManager.SpawnCollectableItems]: " + "Created collectable items! Initializing On Pickup Spawned Event!");
		FireModeEvents.OnPickupSpawnedEvent?.Invoke(m_SpawnedCollectableItems);
	}

	#endregion

}
