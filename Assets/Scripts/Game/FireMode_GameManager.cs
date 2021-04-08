#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion



/// <summary>
///		Fire Mode Game Manager
///		Static top level instance that controls the events that occur with in the game mode 
/// </summary>
[System.Serializable]
public class FireMode_GameManager : MonoBehaviour
{ 
  
	/// <summary>
	///		Instance of the fire mode game manager 
	/// </summary>
	public static FireMode_GameManager Instance;

	/// <summary>
	///		The amount of seconds to wait during the pre game setup  
	/// </summary>
	[Header("Game Mode Timers")]
	public float preWaveSetupTimer = 15f; // seconds before game starts

	/// <summary>
	///		The amount of seconds to wait before the next wave begins 
	/// </summary>
	public float nextWaveTimer = 30f;

	/// <summary>
	///	  The amount of seconds before the wave is reset 
	/// </summary>
	public float resetRoundTimer = 5f; // seconds before the round is restarted    

	/// <summary>
	///		The amount of enemy tanks to spawn in to start with 
	/// </summary>
	[Header("Game Mode Settings")]
	public int startingEnemyTanks = 2;
	
	/// <summary>
	///		The amount of enemy infantry ai to spawn in to begin with 
	/// </summary>
	public int startingEnemyInfantry = 5;

	/// <summary>
	///		The starting amount of health packs to spawn into the game 
	/// </summary>
	[Header("Item Pickup Settings")]
	public int startingHealthPacksSpawned = 4;
	
	/// <summary>
	///		The starting amount of ammunition packs to spawn into the game 
	/// </summary>
	public int startingAmmunitionPacksSpawned = 5;
	
	
	/// <summary>
	///		The index at which more items will be spawned during a round 
	/// </summary>
	int spawnMoreItemsIndex = 4;

	int totalCollectableItemsRemaining; 
	int totalEnemiesRemaining; // total amount enemies remaining 
	int m_currentWaveCount = 1; // the current wave the player is on 						 

	// Testing adding both tank lists to this one 
	List<GameObject> aliveEnemiesRemaining = new List<GameObject>(); // list of currently alive enemies 
	List<GameObject> spawnedCollectableItemsRemaining = new List<GameObject>(); // list of remaining collectable items 

	/// <summary>
	///		A reference to the current player in the scene 
	/// </summary>
	Transform m_currentPlayerReference; // the player 

	int totalKillCount; // the total amount of player kills

	/// <summary>
	///		Current amont of spawned in enemy AI tanks 
	/// </summary>
	int m_spawnTankCount;
	
	/// <summary>
	///		Current total amount of spawned in AI infantry amount 
	/// </summary>
	int m_spawnInfantryCount;

	/// <summary>
	///		The currently spawned in weapon pickups amount 
	/// </summary>
	int spawnedHealthPickupsCount;
	int spawnedAmmunitionPickupsCount;



	#region Unity References 

	/// <summary>
	///		Handles spawning of the Main Player, Enemy AI & Collectable Items Events.
	/// </summary>
	private void OnEnable()
	{
		FireModeEvents.OnPreWaveEvent += PreWaveEvent;
		FireModeEvents.OnRestartGameEvent += RestartGameEvent;
		FireModeEvents.OnGameOverEvent += GameOver;
		FireModeEvents.OnPlayerSpawnedEvent += SpawnedPlayerEntity;
		FireModeEvents.OnEnemyAIWaveSpawnedEvent += SpawnedEntitys;
		FireModeEvents.OnPickupSpawnedEvent += SpawnedPickups;
		FireModeEvents.OnObjectDestroyedEvent += DespawnEntity;
	}

	/// <summary>
	///		Disable's the event listeners 
	/// </summary>
	private void OnDisable()
	{
		FireModeEvents.OnPreWaveEvent -= PreWaveEvent;
		FireModeEvents.OnRestartGameEvent -= RestartGameEvent;
		FireModeEvents.OnGameOverEvent -= GameOver;
		FireModeEvents.OnPlayerSpawnedEvent -= SpawnedPlayerEntity;
		FireModeEvents.OnEnemyAIWaveSpawnedEvent -= SpawnedEntitys;
		FireModeEvents.OnPickupSpawnedEvent -= SpawnedPickups;
		FireModeEvents.OnObjectDestroyedEvent -= DespawnEntity;
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

		// Reset the wave count to 1 
		m_currentWaveCount = 1;

	}


	/// <summary>
	///		Starts Field Of Fire Game Mode!
	/// </summary>
	private void Start()
	{
		StartCoroutine(StartFieldOfFire());
	}

	#endregion

	#region Helper Methods

	/// <summary>
	///		Gets the current wave index 
	/// </summary>
	public int GetCurrentWave
	{
		get
		{
			return m_currentWaveCount;
		}
	}

	#endregion

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
	///		Handles spawning of the enemy tanks & enemy infantry AI characters  
	/// </summary>
	/// <param name="EnemyTanksSpawned"></param>
	/// <param name="EnemyCharactersSpawned"></param>
	private void SpawnedEntitys(List<GameObject> enemiesSpawned)
	{

		aliveEnemiesRemaining.Clear();
		int tankIndex = 0;
		int infantryIndex = 0;

		for (int i = 0; i < enemiesSpawned.Count; i++)
		{

			if (enemiesSpawned[i].GetComponent<InfantryAI>())
			{
				infantryIndex++;
			}
			else if (enemiesSpawned[i].GetComponent<TankAI>())
			{
				tankIndex++;
			}
		
			GameObject _enemy = enemiesSpawned[i];

			aliveEnemiesRemaining.Add(_enemy);
		}

		totalEnemiesRemaining = enemiesSpawned.Count;

		m_spawnInfantryCount = infantryIndex;
		m_spawnTankCount = tankIndex;
	}

	/// <summary>
	///		Handles Despawning of an enemy AI entity 
	/// </summary>
	/// <param name="EnemyEntity"></param>
	private void DespawnEntity(Transform EnemyEntity)
	{
		Debug.Log("[FireMode_GameManager.DespawnEntity]: " + "Attempting to despawn Entity " + EnemyEntity.name);

		if (EnemyEntity.GetComponent<MainPlayerTank>())
		{
			MainPlayerTank player = EnemyEntity.GetComponent<MainPlayerTank>();
			
		
			if (player.tankHealth.Health <= 0)
			{
				FireModeEvents.OnGameOverEvent?.Invoke(player.transform);
				return;
			}
		}
		
		if (
			!EnemyEntity.GetComponent<TankAI>() && 
			!EnemyEntity.GetComponent<InfantryAI>()
			)
		{
			Debug.Log("[FireMode_GameManager.DespawnEntity]: " + "Entity not found " + EnemyEntity.name);
			return;
		}



			aliveEnemiesRemaining.Remove(EnemyEntity.gameObject);
			totalEnemiesRemaining = aliveEnemiesRemaining.Count;
			totalKillCount += 1;
			
			Debug.Log("[FireMode_GameManager.DespawnEntity]: " + "Enemies Remaining:  " + totalEnemiesRemaining);

			FireModeEvents.OnUpdatePlayerKillsEvent?.Invoke(totalKillCount);


		if (totalEnemiesRemaining <= 0)
		{
			Debug.Log("[FireMode_GameManager.DespawnEntity]: " + "Total Enemies Remaining: " + totalEnemiesRemaining + " Running next wave event!");
			
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
		int healthIndex = 0;
		int ammoIndex = 0;

		for (int i = 0; i < SpawnedPickups.Count; i++)
		{

			if (SpawnedPickups[i].GetComponent<BasicItemPickup>().CollectableItem.ItemType == SpecialItemPickup.Health)
			{
				healthIndex++;
			}
			else if (SpawnedPickups[i].GetComponent<BasicItemPickup>().CollectableItem.ItemType == SpecialItemPickup.Ammunition)
			{
				ammoIndex++;
			}

			GameObject item = SpawnedPickups[i];

			spawnedCollectableItemsRemaining.Add(item);
		}

		totalCollectableItemsRemaining = SpawnedPickups.Count;
		spawnedHealthPickupsCount = healthIndex;
		spawnedAmmunitionPickupsCount = ammoIndex;
	}

	/// <summary>
	///		Handles despawning of the pickup items 
	/// </summary>
	/// <param name="Pickup"></param>
	private void DespawnPickups(Transform Pickup)
	{
		if (!Pickup.GetComponent<BasicItemPickup>())
		{
			return;
		}

		if (Pickup.GetComponent<BasicItemPickup>().CollectableItem.ItemType == SpecialItemPickup.Health)
		{
			spawnedHealthPickupsCount--;
		}
		else if (Pickup.GetComponent<BasicItemPickup>().CollectableItem.ItemType == SpecialItemPickup.Ammunition)
		{
			spawnedAmmunitionPickupsCount--;
		}
	

		totalCollectableItemsRemaining--;

		spawnedCollectableItemsRemaining.Remove(Pickup.gameObject);

		if (totalCollectableItemsRemaining <= spawnMoreItemsIndex)
		{
			FireModeEvents.SpawnPickupEvent?.Invoke(startingHealthPacksSpawned, startingAmmunitionPacksSpawned);
		}
	}

	#endregion

	#region Game Methods 
	
	private void GameOver(Transform PlayerEntity)
	{
		if (PlayerEntity.GetComponent<MainPlayerTank>())
		{
			Destroy(PlayerEntity.gameObject);
		}


		Debug.Log("[FireMode_GameManager.GameOver]: " + " Restarting the game!");
		Invoke(nameof(RestartGameEvent), resetRoundTimer);
	}

	/// <summary>
	///		Calls and updates the UI related to the pre wave 
	/// </summary>
	private void PreWaveEvent()
	{
		Debug.Log("[FireMode_GameManager.PreWaveEvent]: " + "Pre wave game event has been called!");
	}
	
	/// <summary>
	///		Resets the game 
	/// </summary>
	private void RestartGameEvent()
	{
		Debug.Log("[FireMode_GameManager.RestartGameEvent]: " + "Restart Game Event called");

		// Respawns the player in 
		Debug.Log("[FireMode_GameManager.RestartGameEvent]: " + "Spawning player in...");
		FireModeEvents.SpawnPlayerEvent?.Invoke();

		Debug.Log("[FireMode_GameManager.RestartGameEvent]: " + "Attempting to spawn first enemy wave!");
		// Calls the first enemy wave! 
		Invoke(nameof(StartFirstEnemyWave), preWaveSetupTimer);

		Debug.Log("[FireMode_GameManager.RestartGameEvent]: " + "Calling on wave started event!");
		FireModeEvents.OnWaveStartedEvent?.Invoke();
	}

	/// <summary>
	///		Starts the first wave of the attack ( On game start event ) 
	/// </summary>
	private void StartFirstEnemyWave()
	{
		Debug.Log("[FireMode_GameManager.StartFirstEnemyWave]: " + "Starting first enemy wave!");
		m_currentWaveCount = 1;
		m_spawnTankCount = startingEnemyTanks;
		m_spawnInfantryCount = startingEnemyInfantry;



		Debug.Log("[FireMode_GameManager.StartFirstEnemyWave]: " + "Calling Enemy Wave Spawn Event...");
		FireModeEvents.SpawnEnemyWaveEvent?.Invoke(m_spawnInfantryCount, m_spawnTankCount);
	}

	/// <summary>
	///		Starts Field of Fire in a custom updated function. Allows you to control when / where to update the game events and what game events to run? 
	/// </summary>
	/// <returns></returns>
	private IEnumerator StartFieldOfFire()
	{
		// Invoke Game Reset Event 
		Debug.Log("[FireMode_GameManager.StartFieldOfFire]: " + "Calling on restart game event..");
		FireModeEvents.OnRestartGameEvent?.Invoke(); // invoke restart game 

		// Spawn the player in 
		// Debug.Log("[FireMode_GameManager.StartFieldOfFire]: " + "Calling player spawn event..");
		// FireModeEvents.SpawnPlayerEvent?.Invoke();

		Debug.Log("[FireMode_GameManager.StartFieldOfFire]: " + "Calling pre wave event!");
		FireModeEvents.OnPreWaveEvent?.Invoke(); // call pre wave event which will display wave count down timer

		// We use a pre game wait timer so we have time to setup the game events before
		yield return new WaitForSeconds(preWaveSetupTimer);

		Debug.Log("[FireMode_GameManager.StartFieldOfFire]: " + "Calling Spawn Pickup event!");
		FireModeEvents.SpawnPickupEvent?.Invoke(startingHealthPacksSpawned, startingAmmunitionPacksSpawned);


		Debug.Log("[FireMode_GameManager.StartFieldOfFire]: " + "Spawning enemy wave!");
		FireModeEvents.SpawnEnemyWaveEvent(startingEnemyInfantry, startingEnemyTanks); // spawns the enemies...

		// Starts the first round! 
		Debug.Log("[FireMode_GameManager.StartFieldOfFire]: " + "Calling on wave started event!");
		FireModeEvents.OnWaveStartedEvent?.Invoke(); // makes the enemies aggressive! 
		

		yield return null; // Tells coroutine when the next update is 
	}

	#endregion
}
