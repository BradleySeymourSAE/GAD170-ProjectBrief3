#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
///		Fire Mode Game Manager
///		Handles Game Mode Settings, Timing & Event Calling  
/// </summary>
[System.Serializable]
public class FireModeGameManager : MonoBehaviour
{

	public static FireModeGameManager Instance;

	#region Public Variables 

	/// <summary>
	///		The amount of seconds to wait during the pre game setup  
	/// </summary>
	public float preWaveSetupTimer = 15f; // seconds before game starts

	/// <summary>
	///		The amount of seconds to wait before the next wave begins 
	/// </summary>
	public float nextWaveStartTimer = 3f; // seconds before next wave starts 

	/// <summary>
	///		The amount of enemy tanks to spawn in to start with 
	/// </summary>
	[Min(1)] public int startingTanks = 1; // starting amount of tanks 
	
	/// <summary>
	///		The starting amount of health packs to spawn into the game 
	/// </summary>
	[Min(5)] public int startingHealthPacks = 5; // starting amount of health packs to spawn 
	
	/// <summary>
	///		The starting amount of ammunition packs to spawn into the game 
	/// </summary>
	[Min(5)] public int startingAmmunitionPacks = 5; // starting amount of ammunition packs to spawn 

	/// <summary>
	///		The index at which more items will be spawned during a round 
	/// </summary>
	/// 
	[Min(3)] [HideInInspector] public int triggerItemRespawnAmount = 3; // How many items remaining respawning more? 

	/// <summary>
	///		The starting amount of lives for a player 
	/// </summary>
	[Min(3)] public int startingLives = 3; // starting lives for a player 

	#endregion

	#region Private Variables 

	/// <summary>
	///		The total amount of collectable items remaining 
	/// </summary>
	[Header("Debugging")]
	[SerializeField] private int totalGameItemsRemaining;

	/// <summary>
	///		The total amount of enemies remaining 
	/// </summary>
	[SerializeField] private int totalAITanksRemaining;
	
	/// <summary>
	///		The current wave the player is on 
	/// </summary>
	[SerializeField] private int currentWaveIndex;

	/// <summary>
	///		List of currently spawned in enemies 
	/// </summary>
	[SerializeField] private List<AI> m_spawnedAITanks = new List<AI>(); 
	
	/// <summary>
	///		List of currently spawned in collectable items 
	/// </summary>
	[SerializeField] private List<GameObject> m_spawnedGameItems = new List<GameObject>();
	
	/// <summary>
	///		A reference to the current player in the scene 
	/// </summary>
	[SerializeField] private MainPlayerTank m_currentPlayerReference;

	/// <summary>
	///		The total player kills 
	/// </summary>
	[SerializeField] private int totalPlayerScore;

	/// <summary>
	///		The total amount of player lives remaining 
	/// </summary>
	[SerializeField] private int currentLives;

	/// <summary>
	///		The amount of health packs to spawn into the game 
	/// </summary>
	[SerializeField] private int healthPackSpawnAmount;

	/// <summary>
	///		The amount of ammunition packs to spawn into the game 
	/// </summary>
	[SerializeField] private int ammunitionPackSpawnAmount;
	
	/// <summary>
	///		The amount of AI Tanks to spawn into the game 
	/// </summary>
	[SerializeField] private int tankAISpawnAmount;

	#endregion

	#region Unity Events  

	/// <summary>
	///		Handles spawning of the Main Player, Enemy AI & Collectable Items Events.
	/// </summary>
	private void OnEnable()
	{
		FireModeEvents.HandleOnAISpawnedEvent += SpawnedAI;
		FireModeEvents.HandleGameItemsSpawnedEvent += SpawnedGameItems;
		FireModeEvents.ResetGameEvent += ResetGame;
		FireModeEvents.HardResetFireMode += ResetGame;
		FireModeEvents.HandleOnPlayerSpawnedEvent += SpawnedPlayerEntity;
		FireModeEvents.HandleAIDestroyedEvent += DespawnEnemyAI;
		FireModeEvents.HandleOnGameItemDestroyed += DespawnGameItem;

		FireModeEvents.HandleNextWaveStarted += BeginNextWave;
		FireModeEvents.IncreaseLivesEvent += IncreaseLives;
		FireModeEvents.IncreasePlayerScoreEvent += IncreaseScore;
		FireModeEvents.IncreaseEnemiesRemainingEvent += IncreaseEnemies;
		FireModeEvents.IncreaseWaveEvent += IncreaseWave;
		FireModeEvents.PlayerWinsEvent += HandlePlayerWins;
		FireModeEvents.AIWinsEvent += HandleAIWins;
	}

	/// <summary>
	///		Disable's the event listeners 
	/// </summary>
	private void OnDisable()
	{
		FireModeEvents.HandleOnAISpawnedEvent -= SpawnedAI;
		FireModeEvents.HandleGameItemsSpawnedEvent -= SpawnedGameItems;
		FireModeEvents.ResetGameEvent -= ResetGame;
		FireModeEvents.HardResetFireMode -= ResetGame;
		FireModeEvents.HandleOnPlayerSpawnedEvent -= SpawnedPlayerEntity;
		FireModeEvents.HandleAIDestroyedEvent -= DespawnEnemyAI;
		FireModeEvents.HandleOnGameItemDestroyed -= DespawnGameItem;
		

		FireModeEvents.HandleNextWaveStarted -= BeginNextWave;
		FireModeEvents.IncreaseLivesEvent -= IncreaseLives;
		FireModeEvents.IncreasePlayerScoreEvent -= IncreaseScore;
		FireModeEvents.IncreaseEnemiesRemainingEvent -= IncreaseEnemies;
		FireModeEvents.IncreaseWaveEvent -= IncreaseWave;
		FireModeEvents.PlayerWinsEvent -= HandlePlayerWins;
		FireModeEvents.AIWinsEvent -= HandleAIWins;
	}

	#endregion




	#region Private Methods 

	/// <summary>
	///		What do we want to do with the spawned in player entity? 
	/// </summary>
	/// <param name="PlayerEntity"></param>
	private void SpawnedPlayerEntity(Transform PlayerEntity)
	{
		if (PlayerEntity.GetComponent<MainPlayerTank>())
		{
			m_currentPlayerReference = PlayerEntity.GetComponent<MainPlayerTank>();
			Debug.Log("[FireModeGameManager.SpawnedPlayerEntity]: " + "Spawned player entity!");
		}
	}


	/// <summary>
	///		Handles the newly spawned in wave of enemies 
	/// </summary>
	/// <param name="EnemyTanksSpawned"></param>
	/// <param name="EnemyCharactersSpawned"></param>
	private void SpawnedAI(List<GameObject> spawnedAITanks)
	{
		Debug.Log("[FireModeGameManager.SpawnedAI]: " + "AI Have been Spawned in!");
		m_spawnedAITanks.Clear();

		for (int i = 0; i < spawnedAITanks.Count; i++)
		{

			// Add the newly spawned enemy to the alive enemies list 
			m_spawnedAITanks.Add(spawnedAITanks[i].GetComponent<AI>());
		}


		Debug.Log("[FireModeGameManager.SpawnedAI]: " + "Total AI Tanks Remaining: " + totalAITanksRemaining);
	}


	/// <summary>
	///		Handles Despawning of an enemy AI entity 
	/// </summary>
	/// <param name="AIEntity"></param>
	private void DespawnEnemyAI(Transform AIEntity)
	{
		Debug.Log("[FireModeGameManager.DespawnEnemyAI]: " + "Attempting to despawn Entity " + AIEntity);


		if (AIEntity.GetComponent<AI>() == null)
		{
			Debug.Log("[FireModeGameManager.DespawnEnemyAI]: " + "That's not an enemy Entity!");
			return;
		}

		// Remove the AI Entity from the list of AI Entities
		m_spawnedAITanks.Remove(AIEntity.GetComponent<AI>());

		if (totalAITanksRemaining <= 0)
		{
			totalAITanksRemaining = 0;

			// Invoke the next wave event 
			Debug.Log("[FireModeGameManager.DespawnEnemyAI]: " + "Starting Next Wave Event! - There are no enemies remaining");

			
			// Then we want to run the next round 
			FireModeEvents.HandleNextWaveStarted?.Invoke();
		}
	}

	/// <summary>
	///		Handles adding the currently spawned collectable items to the game manager's private list 
	/// </summary>
	private void SpawnedGameItems(List<GameObject> gameItems)
	{

		m_spawnedGameItems.Clear();

		for (int i = 0; i < gameItems.Count; i++)
		{

			m_spawnedGameItems.Add(gameItems[i]);
		}


		totalGameItemsRemaining = m_spawnedGameItems.Count;
	}

	/// <summary>
	///		Handles despawning of the pickup items 
	/// </summary>
	/// <param name="pickupToDestroy"></param>
	private void DespawnGameItem(GameObject pickupToDestroy)
	{
	
		// If the pickup item does not have the basic item pickup class 
		if (pickupToDestroy.GetComponent<BasicItemPickup>() == null)
		{
			// Then we want to return. 
			return;
		}

		// Remove the item from the list 
		m_spawnedGameItems.Remove(pickupToDestroy);

		totalGameItemsRemaining = m_spawnedGameItems.Count; 

		// If the total remaining items count is less than or equal to the trigger item respawn count 
		if (totalGameItemsRemaining <= triggerItemRespawnAmount)
		{
			
			Debug.Log("[FireModeGameManager.DespawnGameItem]: " + "Triggering Respawn of Items!");

			// respawn more items 
			FireModeEvents.SpawnGameItemsEvent?.Invoke(healthPackSpawnAmount, ammunitionPackSpawnAmount);

			Debug.Log("[FireModeGameManager.DespawnGameItem]: " + "Total items remaining " + totalGameItemsRemaining + " less than or equal to trigger respawn " + triggerItemRespawnAmount + ". Spawning " + startingHealthPacks + " Health, " + startingAmmunitionPacks + " Ammunition!");
			
		}
	}

	/// <summary>
	///		Starts Field Of Fire Game Mode!
	/// </summary>
	private void Start()
	{
		// Once the scene loads, Field of fire game is started!
		StartCoroutine(RunGameModeLogic());
	}

	/// <summary>
	///		Resets the game from the beginning! 
	/// </summary>
	private void ResetGame()
	{
		currentWaveIndex = 1;
		currentLives = startingLives;
		totalPlayerScore = 0;
		totalAITanksRemaining = 0;
		healthPackSpawnAmount = startingHealthPacks;
		ammunitionPackSpawnAmount = startingAmmunitionPacks;
		tankAISpawnAmount = startingTanks;

		Debug.Log("[FireModeGameManager.ResetGame]: " + "Resetting current wave index, current lives and total players score!");
	}

	/// <summary>
	///		Next Wave Event calls this function to start the next wave
	/// </summary>
	private void BeginNextWave() => StartCoroutine(NextWave());

	/// <summary>
	///		Starts Field of Fire in a custom updated function. Allows you to control when / where to update the game events and what game events to run? 
	/// </summary>
	/// <returns></returns>
	private IEnumerator RunGameModeLogic()
	{
		Debug.Log("[FireModeGameManager.RunGameModeLogic]: " + "Running game mode logic!");
		FireModeEvents.ResetGameEvent?.Invoke(); 

		FireModeEvents.SpawnPlayerEvent?.Invoke(); // spawn the player in

		FireModeEvents.PreGameStartedEvent?.Invoke();

		Debug.Log("[FireModeGameManager.RunGameModeLogic]: " + "Spawn Pickups Event Called! Starting Health Packs: " + healthPackSpawnAmount + " Starting Ammunition Packs: " + ammunitionPackSpawnAmount);
		FireModeEvents.SpawnGameItemsEvent(healthPackSpawnAmount, ammunitionPackSpawnAmount);

		// Wait X amount of seconds before spawning enemies and weapon pickups 
		yield return new WaitForSeconds(preWaveSetupTimer);

		Debug.Log("[FireModeGameManager.RunGameModeLogic]: " + "Spawn Event Wave Event called! Spawning AI Tanks: " + tankAISpawnAmount);
		FireModeEvents.SpawnAIEvent(tankAISpawnAmount); // spawns the enemies...

		// Starts the first round! 
		Debug.Log("[FireModeGameManager.RunGameModeLogic]: " + "On Game Started Event has been called!");
		
		

		// Starts the game after 3 seconds! 
		Invoke(nameof(GameStarted), 3f);
		yield return null; // Tells coroutine when the next update is 
	}

	/// <summary>
	///		Runs the next wave in the sequence 
	/// </summary>
	private IEnumerator NextWave()
	{
		Debug.Log("[FireModeGameManager.NextWave]: " + "Next Wave has been called! ");
		int s_previousWaveIndex = currentWaveIndex;
		
		totalAITanksRemaining = 0;

		//	 Invoke Increase Wave Event by 1 
		FireModeEvents.IncreaseWaveEvent(1);
			
		Debug.Log("[FireModeGameManager.NextWave]: " + " Previous: " + s_previousWaveIndex + " Next: " + currentWaveIndex);

		// Increase the next wave health packs & ammunition pack count 
		healthPackSpawnAmount = Mathf.RoundToInt(Random.Range(startingHealthPacks, startingHealthPacks * 2));
		ammunitionPackSpawnAmount = Mathf.RoundToInt(Random.Range(startingAmmunitionPacks, startingAmmunitionPacks * 2));

		// Increase the amount of tanks to spawn into the next wave 
		tankAISpawnAmount = startingTanks + currentWaveIndex;

		// Spawn the items into the game
		FireModeEvents.SpawnGameItemsEvent(healthPackSpawnAmount, ammunitionPackSpawnAmount);

		// Wait X Amount of Seconds before the enemies are spawned
		Debug.LogWarning("[FireModeGameManager.NextWave]: " + "Waiting " + nextWaveStartTimer + " before running next wave...");
		yield return new WaitForSeconds(nextWaveStartTimer);

		// Spawn more AI tanks into the game 
		FireModeEvents.SpawnAIEvent(tankAISpawnAmount);

		Debug.Log("[FireModeGameManager.NextWave]: " + "Running the next wave!");
		
		// Starts the game again and enables the Enemy AI tanks and Game Items  
		Invoke(nameof(GameStarted), 3f);
	}

	/// <summary>
	///		Starts the first wave (Game Started Event) 
	/// </summary>
	private void GameStarted()
	{
		// Starts the wave! 
		FireModeEvents.GameStartedEvent?.Invoke();
	}

	/// <summary>
	///		Increases the players lives - Invokes the LivesEventUI
	/// </summary>
	/// <param name="amount"></param>
	private void IncreaseLives(int amount)
	{
		currentLives += amount;

		if (currentLives <= 0)
		{
			Debug.Log("[FireModeGameManager.IncreaseLives]: " + "Player lives are less than or equal to 0! Lives: " + currentLives);


			Debug.Break();
			// Hard Reset the Game 
			FireModeEvents.ResetGameEvent?.Invoke();
			
		}
		
			// Update the UI to reflect the lives remaining 
			Debug.Log("[FireModeGameManager.IncreaseLives]: " + "Increase Lives Event UI Invoked! Lives Remaining: " + currentLives);

			// FireModeEvents.IncreaseLivesEventUI?.Invoke(currentLives);
	}

	/// <summary>
	///		Increases the current wave index - Invokes IncreaseWaveEventUI
	/// </summary>
	/// <param name="amount"></param>
	private void IncreaseWave(int amount)
	{
		currentWaveIndex += amount;

		Debug.Log("[FireModeGameManager.IncreaseWave]: " + "WAVE INCREASED: " + currentWaveIndex);
		
		if (currentWaveIndex >= 10)
		{
			Debug.Log("[FireModeGameManager.IncreaseWave]: " + "PLAYER HAS WON THE GAME!");

			// Invoke Player Won Event, Or just Call the function.. 


			Debug.Break();
		}
		else
		{
			// Increase the current wave index by 1 
			FireModeEvents.IncreaseWaveEventUI?.Invoke(currentWaveIndex);
		}
	}

	/// <summary>
	///		Increases the current players score - Invokes IncreasePlayerScoreEventUI 
	/// </summary>
	/// <param name="amount"></param>
	private void IncreaseScore(int amount)
	{
		totalPlayerScore += amount;

		Debug.Log("[FireModeGameManager.IncreaseScore]: " + "Increasing player's score " + totalPlayerScore + " by amount " + amount);

		FireModeEvents.IncreasePlayerScoreEventUI?.Invoke(totalPlayerScore);
	}

	/// <summary>
	///		Increase or decreases the amount of enemies remaining 
	/// </summary>
	/// <param name="amount"></param>
	private void IncreaseEnemies(int amount)
	{
		totalAITanksRemaining += amount;

		if (totalAITanksRemaining < 0)
		{
			Debug.LogWarning("[FireModeGameManager.IncreaseEnemies]: " + "Total AI Tanks Remaining is less than 0 so setting total ai tanks remaining to zero then invoking the ui to update!");
			totalAITanksRemaining = 0;

		}


		FireModeEvents.IncreaseEnemiesRemainingEventUI?.Invoke(totalAITanksRemaining);
	}

	/// <summary>
	///		Handles when the player wins the game 
	/// </summary>
	private void HandlePlayerWins()
	{
		Debug.Log("[FireModeGameManager.HandlePlayerWins]: " + "Player has won the game!");

		Debug.Break();
	}

	/// <summary>
	///		Handles when the AI wins the game 
	/// </summary>
	private void HandleAIWins()
	{
		Debug.Log("[FireModeGameManager.HandleAIWins]: " + "AI Wins the game!");

		// Find the player object and reset the game object 

		// Spawn the player back into the game 

		// Invoke Hard Reset Game Event 
		

		Debug.Break();
	}

	#endregion

}