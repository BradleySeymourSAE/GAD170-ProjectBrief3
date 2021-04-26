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
	public Transform playerSpawnPoint;

	/// <summary>
	///		 The Enemy AI's Spawn Point 
	/// </summary>
	public Transform aiWaveSpawnPoint;

	/// <summary>
	///		The Collectable Item Spawn 
	/// </summary>
	public Transform gameItemsSpawnPoint;

	/// <summary>
	///		The wave spawn maximum range 
	/// </summary>
	public float maximumWaveSpawnRange = 250f;
	
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

	public float maximumHealthSpawnX = 300;
	public float maximumHealthSpawnZ = 250;

	public float maximumAmmunitionSpawnX = 150;
	public float maximumAmmunitionSpawnZ = 300;

	
	/// <summary>
	///		Maximum Item's Y Positional Offset 
	/// </summary>
	[Min(0.3f)]
	public float itemSpawnYPositionOffset = 0.35f;
	
	#endregion

	#region Private Variables 
	
	/// <summary>
	///		List of spawned enemies 
	/// </summary>
	private List<GameObject> m_SpawnedAIEnemies = new List<GameObject>();
	
	/// <summary>
	///		List of spawned collectable items 
	/// </summary>
	private List<GameObject> m_SpawnedGameItems = new List<GameObject>();
	
	/// <summary>
	///		Reference to the current player instance 
	/// </summary>
	private Transform m_PlayerReference;

	/// <summary>
	///		Reference to the spawn items camera 
	/// </summary>
	private Transform m_SpawnCameraReference;

	#endregion


	float spawnHealthXPosition;
	float spawnHealthZPosition;

	float spawnRoundsXPosition;
	float spawnRoundsZPosition;


	#region Unity References   
	
	/// <summary>
	///		Enemy Spawn Manager Event Listeners 
	/// </summary>
	private void OnEnable()
	{
		FireModeEvents.SpawnPlayerEvent += SpawnPlayer; // spawns the player into the game 
		FireModeEvents.SpawnAIEvent += SpawnWave; // spawns in the enemies 
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
		FireModeEvents.SpawnAIEvent -= SpawnWave; // removes enemy listener  
		FireModeEvents.SpawnGameItemsEvent -= SpawnGameItems; // removes event listener 
		
		FireModeEvents.HardResetFireMode -= HandleOnReset; // restarts the game 
		FireModeEvents.ResetGameEvent -= HandleOnReset; // Restarts the current wave 
	}


	private void Awake()
	{
		if (GameObject.Find("SpawnCamera").GetComponent<Camera>())
		{
			m_SpawnCameraReference = GameObject.Find("SpawnCamera").transform;
		}
		else
		{
			Debug.LogWarning("[FireModeSpawnManager.Awake]: " + "Could not find a reference to the spawn object's camera!");
		}
	}


	#endregion

	#region Private Methods 

	/// <summary>
	///		Spawns a player into the game 
	/// </summary>
	private void SpawnPlayer()
	{
		

		GameObject spawnedPlayer = Instantiate(PlayerPrefab, playerSpawnPoint.transform.position, PlayerPrefab.transform.rotation);

		if (spawnedPlayer.GetComponent<MainPlayerTank>())
		{
			Debug.Log("Spawning in the player!");
			m_PlayerReference = spawnedPlayer.transform;
		
			// Check for audio listener 
			if (!m_PlayerReference.GetComponent<AudioListener>())
			{
				// Add one if it doesnt exist.
				Debug.LogWarning("[FireModeSpawnManager.SpawnPlayer]: " + "Player does not have an audio listener - Adding one to the player!");
				spawnedPlayer.AddComponent<AudioListener>();
			}
			else
			{
				Debug.LogWarning("[FireModeSpawnManager.SpawnPlayer]: " + "Player already has an audio listener - Ignoring!");
			}

			// Invoke the spawning of the player reference 
			FireModeEvents.HandleOnPlayerSpawnedEvent?.Invoke(m_PlayerReference);
		}
		else
		{
			Debug.LogError("[FireModeSpawnManager.SpawnPlayer]: " + "Player does not have the MainPlayerTank script attached! Please attach the script and try again!");
		}
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
		for (int i = 0; i < m_SpawnedGameItems.Count; i++)
		{
				Destroy(m_SpawnedGameItems[i]);
		}

		// Clear the spawned in enemy AI & item pickup lists. 
		m_SpawnedAIEnemies.Clear();
		m_SpawnedGameItems.Clear();
	}

	/// <summary>
	///		Spawns in an enemy wave
	/// </summary>
	/// <param name="InfantryAmount"></param>
	/// <param name="AISpawnAmount"></param>
	private void SpawnWave(int AISpawnAmount)
	{
		Debug.Log("[FieldOfFireSpawnManager.SpawnEnemyWave]: " + "Spawning Enemy AI " + AISpawnAmount);
		
		// Repeat the process for the amount of tanks we want in the round 
		for (int i = 0; i < AISpawnAmount; i++)
		{
			float xPosition = Random.Range(-maximumWaveSpawnRange, maximumWaveSpawnRange);
			float zPosition = Random.Range(-maximumWaveSpawnRange, maximumWaveSpawnRange);

			Vector3 tempSpawnPosition = new Vector3(aiWaveSpawnPoint.transform.position.x + xPosition, 0f, aiWaveSpawnPoint.transform.position.z + zPosition);


			// Raycast to get the right Y Axis for the spawned in AI  


			GameObject tankClone = Instantiate(TankPrefab, tempSpawnPosition, TankPrefab.transform.rotation);

			m_SpawnedAIEnemies.Add(tankClone);
		}

		FireModeEvents.IncreaseEnemiesRemainingEvent?.Invoke(AISpawnAmount);



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


			spawnHealthXPosition = Random.Range(-maximumHealthSpawnX, maximumHealthSpawnX);
			spawnHealthZPosition = Random.Range(-maximumHealthSpawnZ, maximumHealthSpawnZ);
		


			Vector3 newSpawnPoint = new Vector3(gameItemsSpawnPoint.transform.position.x + spawnHealthXPosition, 0 + itemSpawnYPositionOffset, gameItemsSpawnPoint.transform.position.z + spawnHealthZPosition);

			GameObject s_Health = Instantiate(HealthPackPrefab, newSpawnPoint, HealthPackPrefab.transform.rotation);
			
			m_SpawnedGameItems.Add(s_Health);
		}

		// Create ammunition packs and set their spawn points randomly.
		for (int i = 0; i < AmmunitionPacks; i++)
		{
			spawnRoundsXPosition = Random.Range(-maximumAmmunitionSpawnX, maximumAmmunitionSpawnX);
			spawnRoundsZPosition = Random.Range(-maximumAmmunitionSpawnZ, maximumAmmunitionSpawnZ);

			Vector3 newSpawnPoint = new Vector3(gameItemsSpawnPoint.transform.position.x + spawnRoundsXPosition, 0 + itemSpawnYPositionOffset, gameItemsSpawnPoint.transform.position.z + spawnRoundsZPosition);

			GameObject s_Ammunition = Instantiate(AmmunitionPackPrefab, newSpawnPoint, AmmunitionPackPrefab.transform.rotation);

			m_SpawnedGameItems.Add(s_Ammunition);
		}

		
		Debug.LogWarning("[FireModeSpawnManager.SpawnGameItems]: " + "Spawned items - Calling Handle Game Items Spawned Event!");
		FireModeEvents.HandleGameItemsSpawnedEvent?.Invoke(m_SpawnedGameItems);
	}

	#endregion

}
