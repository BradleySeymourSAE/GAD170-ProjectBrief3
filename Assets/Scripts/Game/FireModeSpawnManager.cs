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
public class FireModeSpawnManager : MonoBehaviour
{

	#region Public Variables 

	/// <summary>
	///		The Main Player Prefab 
	/// </summary>
	public GameObject PlayerPrefab; 

	/// <summary>
	///		The Enemy AI Tank Prefab 
	/// </summary>
	public GameObject TankPrefab;

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
	public Transform PlayerSpawnPoint;

	/// <summary>
	///		 The Enemy AI's Spawn Point 
	/// </summary>
	public Transform AISpawnPoint;

	/// <summary>
	///		The Collectable Item Spawn 
	/// </summary>
	public Transform GameItemSpawnPoint;

	/// <summary>
	///		The wave spawn maximum range 
	/// </summary>
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
	public float itemSpawnRange = 100f;
	
	/// <summary>
	///		Maximum Item's Y Positional Offset 
	/// </summary>
	[Min(0.3f)]
	public float itemSpawnYPositionOffset = 0.35f;
	
	#endregion

	#region Private Variables 

	/// <summary>
	///		The minimum range for spawning enemies 
	/// </summary>
	private float minimumWaveSpawnRange = 50f;

	/// <summary>
	///		The minimum range for spawning items 
	/// </summary>
	private float minimumItemSpawnRange = 15f;

	/// <summary>
	///		The position of the wave spawn point 
	/// </summary>
	private Vector3 m_WaveSpawnPoint;

	/// <summary>
	///		The position of the item's spawn point 
	/// </summary>
	private Vector3 m_ItemSpawnPoint;

	/// <summary>
	///		The position to spawn the player 
	/// </summary>
	private Vector3 m_PlayerSpawnPoint;
	
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

	#region Unity References   
	
	/// <summary>
	///		Enemy Spawn Manager Event Listeners 
	/// </summary>
	private void OnEnable()
	{
		FireModeEvents.SpawnPlayerEvent += SpawnPlayer; // spawns the player into the game 
		FireModeEvents.SpawnAIEvent += SpawnEnemyWave; // spawns in the enemies 
		FireModeEvents.SpawnGameItemsEvent += SpawnGameItems; // spawns in the collectable items
	
		FireModeEvents.HardResetFireMode += HandleOnReset; // on game restart 
		FireModeEvents.ResetGameEvent += HandleOnReset; // Resets the current wave 
	}

	/// <summary>
	///		Enemy Spawn Manager Event Listener Cleanups 
	/// </summary>
	private void OnDisable()
	{
		FireModeEvents.SpawnPlayerEvent -= SpawnPlayer; // spawns player in the game 
		FireModeEvents.SpawnAIEvent -= SpawnEnemyWave; // removes enemy listener  
		FireModeEvents.SpawnGameItemsEvent -= SpawnGameItems; // removes event listener 
		
		FireModeEvents.HardResetFireMode -= HandleOnReset; // restarts the game 
		FireModeEvents.ResetGameEvent -= HandleOnReset; // Restarts the current wave 
	}

	private void Start()
	{
		m_ItemSpawnPoint = GameItemSpawnPoint.position;
		m_WaveSpawnPoint = AISpawnPoint.position;
		m_PlayerSpawnPoint = PlayerSpawnPoint.position;
	}

	#endregion

	#region Private Methods 

	/// <summary>
	///		Spawns a player into the game 
	/// </summary>
	private void SpawnPlayer()
	{
		GameObject SpawnedPlayer = Instantiate(PlayerPrefab, PlayerSpawnPoint.position, PlayerPrefab.transform.rotation);

		if (SpawnedPlayer.GetComponent<MainPlayerTank>())
		{
			Debug.Log("Spawning in the player!");
			m_PlayerReference = SpawnedPlayer.transform;
		
			// Check for audio listener 
			if (!m_PlayerReference.GetComponent<AudioListener>())
			{
				// Add one if it doesnt exist.
				Debug.Log("[FireModeSpawnManager.SpawnPlayer]: " + "Player does not have an audio listener - Adding one to the player!");
				SpawnedPlayer.AddComponent<AudioListener>();
			}
		
		}

		// Invoke the spawning of the player ref 
		FireModeEvents.HandleOnPlayerSpawnedEvent?.Invoke(m_PlayerReference);
	}

	/// <summary>
	///		Resets the enemy wave 
	/// </summary>
	private void HandleOnReset()
	{
		Debug.LogWarning("[FireModeSpawnManager.HandleOnReset]: " + "Handling Reset! Removing spawned in game objects from the scene");


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
	/// <param name="AmountOfEnemies"></param>
	private void SpawnEnemyWave(int AmountOfEnemies)
	{
		Debug.Log("[FieldOfFireSpawnManager.SpawnEnemyWave]: " + "Spawning Enemy AI " + AmountOfEnemies);
		
		// Repeat the process for the amount of tanks we want in the round 
		for (int i = 0; i < AmountOfEnemies; i++)
		{
			float xPosition = Mathf.Round(Random.Range(-minimumWaveSpawnRange, waveSpawnRange) * magnitude);
			float zPosition = Mathf.Round(Random.Range(-minimumWaveSpawnRange, waveSpawnRange) * magnitude);

			Vector3 tempSpawnPosition = new Vector3(m_WaveSpawnPoint.x + xPosition, 0f, m_WaveSpawnPoint.z + zPosition);

			GameObject tankClone = Instantiate(TankPrefab, tempSpawnPosition, TankPrefab.transform.rotation);

			m_SpawnedAIEnemies.Add(tankClone);
		}


		// Now we have all the enemy ai spawned in we then handle what to do with the new enemies that have been spawned 
		Debug.Log("[FieldOfFireSpawnManager.SpawnEnemyWave]: " + "Invoking Handle On AI Spawned Event!");
		FireModeEvents.HandleOnAISpawnedEvent?.Invoke(m_SpawnedAIEnemies);
	}

	/// <summary>
	///		Spawns in collectable items 
	/// </summary>
	/// <param name="HealthPacks">Amount of health packs to spawn in</param>
	/// <param name="AmmunitionPacks">Amount of ammunition packs to spawn in</param>
	private void SpawnGameItems(int HealthPacks, int AmmunitionPacks)
	{
		// Create health packs and set their spawn points randomly 
		for (int i = 0; i < HealthPacks; i++)
		{
			float xPosition = Random.Range(-minimumItemSpawnRange, itemSpawnRange);
			float zPosition = Random.Range(-minimumItemSpawnRange, itemSpawnRange);
			
			Vector3 newSpawnPoint = new Vector3(m_ItemSpawnPoint.x + xPosition, m_ItemSpawnPoint.y + itemSpawnYPositionOffset, m_ItemSpawnPoint.z + zPosition);

			GameObject _health = Instantiate(HealthPackPrefab, newSpawnPoint, HealthPackPrefab.transform.rotation);
			
			m_SpawnedCollectableItems.Add(_health);
		}

		// Create ammunition packs and set their spawn points randomly.
		for (int i = 0; i < AmmunitionPacks; i++)
		{
			float x = Random.Range(-minimumItemSpawnRange, itemSpawnRange);
			float z = Random.Range(-minimumItemSpawnRange, itemSpawnRange);

			Vector3 newSpawnPoint = new Vector3(m_ItemSpawnPoint.x + x, m_ItemSpawnPoint.y + itemSpawnYPositionOffset, m_ItemSpawnPoint.z + z);

			GameObject _ammo = Instantiate(AmmunitionPackPrefab, newSpawnPoint, AmmunitionPackPrefab.transform.rotation);

			m_SpawnedCollectableItems.Add(_ammo);
		}

		
		Debug.LogWarning("[FireModeSpawnManager.SpawnGameItems]: " + "Spawned items - Calling Handle Game Items Spawned Event!");
		FireModeEvents.HandleGameItemsSpawnedEvent?.Invoke(m_SpawnedCollectableItems);
	}

	#endregion

}
