#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
///		Fire Mode Game Manager Instance
///		Handles Game Settings like Timers and starting spawn counts 
/// </summary>
[System.Serializable]
public class FireMode_GameManager : MonoBehaviour
{
	#region Static
	
	/// <summary>
	///		Instance of the fire mode game manager 
	/// </summary>
	public static FireMode_GameManager Instance;

	#endregion

	#region Public Variables 

	#region Game Timers 
	/// <summary>
	///		The amount of seconds to wait during the pre game setup  
	/// </summary>
	[Header("Game Mode Timers")]
	public float preWaveSetupTimer = 15f; // seconds before game starts

	/// <summary>
	///		The amount of seconds to wait before the next wave begins 
	/// </summary>
	public float startTimer = 30f;

	#endregion

	#region Game Mode Startup Settings 
	/// <summary>
	///		The amount of enemy tanks to spawn in to start with 
	/// </summary>
	[Header("Game Mode Settings")]
	public int startingTanks = 2;
	
	/// <summary>
	///		The amount of enemy infantry ai to spawn in to begin with 
	/// </summary>
	public int startingInfantry = 3;

	/// <summary>
	///		The starting amount of health packs to spawn into the game 
	/// </summary>
	[Header("Item Pickup Settings")]
	public int startingHealthPacks = 3;
	
	/// <summary>
	///		The starting amount of ammunition packs to spawn into the game 
	/// </summary>
	public int startingAmmunitionPacks = 5;

	#endregion

	#endregion

	#region Private Variables 
	
	#region Collectable Items

	/// <summary>
	///		The index at which more items will be spawned during a round 
	/// </summary>
	/// 
	[SerializeField] private int triggerItemRespawnCount = 2;
	
	/// <summary>
	///		The total amount of collectable items remaining 
	/// </summary>
	[SerializeField] private int totalCollectableItemsRemaining;
	#endregion

	#region Wave & Player References  

	/// <summary>
	///		The total amount of enemies remaining 
	/// </summary>
	[SerializeField] private int totalEnemiesRemaining;
	
	/// <summary>
	///		The current wave the player is on 
	/// </summary>
	[SerializeField] private int currentWaveIndex;

	/// <summary>
	///		List of currently spawned in enemies 
	/// </summary>
	[SerializeField] private List<GameObject> m_AliveEnemies = new List<GameObject>(); 
	
	/// <summary>
	///		List of currently spawned in collectable items 
	/// </summary>
	[SerializeField] private List<GameObject> spawnedCollectableItemsRemaining = new List<GameObject>();
	
	/// <summary>
	///		A reference to the current player in the scene 
	/// </summary>
	[SerializeField] private Transform m_currentPlayerReference;
	
	#endregion

	#region Spawner / UI Private Variables 
	
	/// <summary>
	///		The total player kills 
	/// </summary>
	[SerializeField] private int totalKillCount;

	/// <summary>
	///		The total amount of kills the player has achieved this round 
	/// </summary>
	[SerializeField] private int currentRoundKillCount;

	/// <summary>
	///		The total amount of spawned in tanks 
	/// </summary>
	[SerializeField] private int enemyAITanksRemaining;
	
	/// <summary>
	///		The total amount of spawned in infantry 
	/// </summary>
	[SerializeField] private int enemyInfantryRemaining;

	/// <summary>
	///		The total spawned in health pickups
	/// </summary>
	[SerializeField] private int healthPacksRemaining;

	/// <summary>
	///		The total spawned in ammunition pickups
	/// </summary>
	[SerializeField] private int ammunitionPacksRemaining;

	#endregion

	#endregion

	#region Unity References 

	/// <summary>
	///		Handles spawning of the Main Player, Enemy AI & Collectable Items Events.
	/// </summary>
	private void OnEnable()
	{
		FireModeEvents.OnPlayerSpawnedEvent += SpawnedPlayerEntity; // spawn player 
		FireModeEvents.OnEnemyAIWaveSpawnedEvent += SpawnedEntitys; // spawned enemy entities 
		FireModeEvents.OnPickupSpawnedEvent += SpawnedPickups; // spawned item pickups 

		FireModeEvents.OnNextWaveEvent += NextWave;
		FireModeEvents.OnResetWaveEvent += ResetWave;
		FireModeEvents.OnObjectDestroyedEvent += DespawnEntity; // despawn's an enemy 
		FireModeEvents.OnObjectPickedUpEvent += CollectedItem;  // collects an item pickup  
	}

	/// <summary>
	///		Disable's the event listeners 
	/// </summary>
	private void OnDisable()
	{
		FireModeEvents.OnPlayerSpawnedEvent -= SpawnedPlayerEntity;
		FireModeEvents.OnEnemyAIWaveSpawnedEvent -= SpawnedEntitys;
		FireModeEvents.OnPickupSpawnedEvent -= SpawnedPickups;

		FireModeEvents.OnNextWaveEvent -= NextWave;
		FireModeEvents.OnResetWaveEvent -= ResetWave;
		FireModeEvents.OnObjectDestroyedEvent -= DespawnEntity;
		FireModeEvents.OnObjectPickedUpEvent -= CollectedItem;
	}

	/// <summary>
	///		Destroys the game manager object and creates a new instance 
	/// </summary>
	private void Awake()
	{
		// Destroy the game manager and set a persistant instance 
		if (Instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	#endregion

	#region Private Methods 
	
	#region Spawning / Despawning Methods 

	/// <summary>
	///		Spawns in the player entity!
	/// </summary>
	/// <param name="PlayerEntity"></param>
	private void SpawnedPlayerEntity(Transform PlayerEntity)
	{
		if (PlayerEntity.GetComponent<MainPlayerTank>())
		{
			m_currentPlayerReference = PlayerEntity.GetComponent<MainPlayerTank>().transform;
		}

		
	}

	/// <summary>
	///		Handles updating the currently spawned in entities 
	/// </summary>
	/// <param name="EnemyTanksSpawned"></param>
	/// <param name="EnemyCharactersSpawned"></param>
	private void SpawnedEntitys(List<GameObject> enemiesSpawned)
	{

		m_AliveEnemies.Clear();

		for (int i = 0; i < enemiesSpawned.Count; i++)
		{

			if (enemiesSpawned[i].GetComponent<InfantryAI>())
			{
				enemyInfantryRemaining++;
			}
			else if (enemiesSpawned[i].GetComponent<TankAI>())
			{
				enemyAITanksRemaining++;
			}

			// Add the newly spawned enemy to the alive enemies list 
			m_AliveEnemies.Add(enemiesSpawned[i]);
		}

		totalEnemiesRemaining = m_AliveEnemies.Count;


		// Update the In game UI
		UpdateInGameUI();
		UpdatePlayerKillCount();
	}

	/// <summary>
	///		Handles updating the players kill count 
	/// </summary>
	private void UpdatePlayerKillCount()
	{
		FireModeEvents.UpdatePlayerKillsEvent?.Invoke(totalKillCount);
	}

	private void UpdateInGameUI()
	{
		FireModeEvents.UpdateWaveUIEvent?.Invoke(currentWaveIndex, totalEnemiesRemaining, currentRoundKillCount);
	}

	/// <summary>
	///		Handles Despawning of an enemy AI entity 
	/// </summary>
	/// <param name="EnemyEntity"></param>
	private void DespawnEntity(Transform EnemyEntity)
	{
		Debug.Log("[FireMode_GameManager.DespawnEntity]: " + "Attempting to despawn Entity " + EnemyEntity.name);

		// If the enemy entity is a player we probably want to return. 
		if (EnemyEntity.GetComponent<MainPlayerTank>())
		{
			Debug.Log("[FireMode_GameManager.DespawnEntity]: " + "Somehow found a player reference here! I should probably return..." + EnemyEntity.name);
			return;
		}
		
		// If the enemy tank doesn't have either the tank ai or infantry ai script 
		if (EnemyEntity.GetComponent<TankAI>() == null && EnemyEntity.GetComponent<InfantryAI>() == null)
		{
			// Then we want to return. 
			Debug.Log("[FireMode_GameManager.DespawnEntity]: " + "Entity not found " + EnemyEntity.name);
			return;
		}

		// If enemy is a tank 
		if (EnemyEntity.GetComponent<TankAI>())
		{
			// Remove 1 from the spawned in tanks remaining 
			enemyAITanksRemaining--;
		}
		// Otherwise if the enemy is an infantry character 
		else if (EnemyEntity.GetComponent<InfantryAI>())
		{
			// Remove 1 from the spawned in infantry remaining 
				enemyInfantryRemaining--;
		}
		

		
		m_AliveEnemies.Remove(EnemyEntity.gameObject);
			
			// Set the enemies remaining to the new alive enemies count 
		totalEnemiesRemaining = m_AliveEnemies.Count;
		Debug.Log("[FireMode_GameManager.DespawnEntity]: " + "Enemies Remaining:  " + totalEnemiesRemaining);
			
		currentRoundKillCount += 1;
		totalKillCount += 1; // increase the players kill count by 1 
		
		UpdatePlayerKillCount(); // Update the player's kills

		// Updates the players in game wave ui stats such as enemies remaining, wave current wave count 
		UpdateInGameUI(); 

		if (totalEnemiesRemaining <= 0)
		{
			// Invoke the next wave event 
			Debug.Log("[FireMode_GameManager.DespawnEntity]: " + "Starting Next Wave Event! - There are no enemies remaining - " + totalEnemiesRemaining);
			
			// Then we want to run the next round 
			FireModeEvents.OnNextWaveEvent?.Invoke();
		}
	}

	/// <summary>
	///		Handles spawning of the pickups 
	/// </summary>
	private void SpawnedPickups(List<GameObject> SpawnedPickups)
	{

		spawnedCollectableItemsRemaining.Clear();

		for (int i = 0; i < SpawnedPickups.Count; i++)
		{

			if (SpawnedPickups[i].GetComponent<BasicItemPickup>().CollectableItem.ItemType == SpecialItemPickup.Health)
			{
				healthPacksRemaining += 1;
			}
			else if (SpawnedPickups[i].GetComponent<BasicItemPickup>().CollectableItem.ItemType == SpecialItemPickup.Ammunition)
			{
				ammunitionPacksRemaining += 1;
			}

			GameObject item = SpawnedPickups[i];

			spawnedCollectableItemsRemaining.Add(item);
		}

		totalCollectableItemsRemaining = spawnedCollectableItemsRemaining.Count;

		UpdateInGameUI();
	}

	/// <summary>
	///		Handles despawning of the pickup items 
	/// </summary>
	/// <param name="Pickup"></param>
	private void CollectedItem(Transform Pickup)
	{
		if (!Pickup.GetComponent<BasicItemPickup>())
		{
			return;
		}

		if (Pickup.GetComponent<BasicItemPickup>().CollectableItem.ItemType == SpecialItemPickup.Health)
		{
			healthPacksRemaining--;
		}
		else if (Pickup.GetComponent<BasicItemPickup>().CollectableItem.ItemType == SpecialItemPickup.Ammunition)
		{
			ammunitionPacksRemaining--;
		}
	

		// Remove the collectable item from the scene 
		spawnedCollectableItemsRemaining.Remove(Pickup.gameObject);

		// Reset the total remaining collectable items count 
		totalCollectableItemsRemaining = spawnedCollectableItemsRemaining.Count;

		// If the total remaining collectable items count is less than or equal to the trigger item respawn count 
		if (totalCollectableItemsRemaining <= triggerItemRespawnCount)
		{
			// Add random amount of health packs 
			// Add random amount of ammo packs 

			startingHealthPacks = (int)Random.Range(1f, startingHealthPacks - healthPacksRemaining);
			startingAmmunitionPacks = (int)Random.Range(1f, startingAmmunitionPacks - ammunitionPacksRemaining);

			// Spawn the weapon pickups

			Debug.Log("[FireMode_GameManager.CollectedItem]: " + "Total items remaining " + totalCollectableItemsRemaining + " less than or equal to trigger respawn " + triggerItemRespawnCount + ". Spawning " + startingHealthPacks + " Health, " + startingAmmunitionPacks + " Ammunition!");
			FireModeEvents.SpawnPickupsEvent?.Invoke(startingHealthPacks, startingAmmunitionPacks);
		}
	}

	#endregion

	#region Game Methods 

	/// <summary>
	///		Starts Field Of Fire Game Mode!
	/// </summary>
	private void Start()
	{
		currentWaveIndex = 1;
		totalKillCount = 0;
		// Once the scene loads, Field of fire game is started!
		StartCoroutine(RunGameModeLogic());
	}

	/// <summary>
	///		Starts Field of Fire in a custom updated function. Allows you to control when / where to update the game events and what game events to run? 
	/// </summary>
	/// <returns></returns>
	private IEnumerator RunGameModeLogic()
	{
		Debug.Log("[FireMode_GameManager.RunGameModeLogic]: " + "Running game mode logic!");
		FireModeEvents.OnGameRestartEvent?.Invoke(); 

		FireModeEvents.SpawnPlayerEvent?.Invoke(); // spawn the player in

		FireModeEvents.OnPreWaveEvent?.Invoke(preWaveSetupTimer); 

		// Wait X amount of seconds before spawning enemies and weapon pickups 
		yield return new WaitForSeconds(preWaveSetupTimer);

		Debug.Log("[FireMode_GameManager.StartFieldOfFire]: " + "Spawn Pickups Event Called! Starting Health Packs: " + startingHealthPacks + " Starting Ammunition Packs: " + startingAmmunitionPacks);
		FireModeEvents.SpawnPickupsEvent(startingHealthPacks, startingAmmunitionPacks);

		Debug.Log("[FireMode_GameManager.StartFieldOfFire]: " + "Spawn Event Wave Event called! Starting Infantry: " + startingInfantry + " Starting Tanks: " + startingTanks);
		FireModeEvents.SpawnEnemyWaveEvent(startingInfantry, startingTanks); // spawns the enemies...

		// Starts the first round! 
		Debug.Log("[FireMode_GameManager.StartFieldOfFire]: " + "On Wave Started Event has been called!");
		FireModeEvents.OnWaveStartedEvent?.Invoke(); // Starts the game! 


		yield return null; // Tells coroutine when the next update is 
	}

	/// <summary>
	///		Runs the next wave in the sequence 
	/// </summary>
	private void NextWave()
	{
		Debug.Log("[FireMode_GameManager.NextWave]: " + "Starting next wave!! (TODO) WIP ");
		currentWaveIndex += 1; // increase the wave index 
		currentRoundKillCount = 0; // reset current round kills to 0 


		UpdateInGameUI(); // updates the in game ui. 
	

		Debug.Log("Current Wave Index: " + currentWaveIndex);
		
	}

	/// <summary>
	///		Resets the game 
	/// </summary>
	private void ResetWave()
	{
		FireModeEvents.OnResetWaveEvent?.Invoke(); 


		Debug.Log("[FireMode_GameManager.ResetWave]: " + "Invoking start wave event!");
		Invoke(nameof(StartWave), startTimer);
	}

	/// <summary>
	///		Starts the first wave (Game Started Event) 
	/// </summary>
	private void StartWave()
	{
		FireModeEvents.OnWaveStartedEvent?.Invoke();
	}


	#endregion

	#endregion
}