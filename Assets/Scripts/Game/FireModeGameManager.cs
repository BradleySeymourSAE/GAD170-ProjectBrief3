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
public class FireModeGameManager : MonoBehaviour
{

	/// <summary>
	///		The amount of seconds to wait during the pre game setup  
	/// </summary>
	public float preWaveSetupTimer = 15f; // seconds before game starts

	/// <summary>
	///		The amount of seconds to wait before the next wave begins 
	/// </summary>
	public float nextWaveStartTimer = 3f;

	/// <summary>
	///		The amount of enemy tanks to spawn in to start with 
	/// </summary>
	[Min(2)] public int startingTanks = 2;
	
	/// <summary>
	///		The starting amount of health packs to spawn into the game 
	/// </summary>
	[Min(4)] public int startingHealthPacks = 3;
	
	/// <summary>
	///		The starting amount of ammunition packs to spawn into the game 
	/// </summary>
	[Min(10)] public int startingAmmunitionPacks = 5;

	/// <summary>
	///		The index at which more items will be spawned during a round 
	/// </summary>
	/// 
	[Min(5)] [HideInInspector] public int triggerItemRespawnAmount = 5;

	/// <summary>
	///		The starting amount of lives for a player 
	/// </summary>
	[Min(1)] public int startingLives = 3;


	
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
	[SerializeField] private List<GameObject> m_AliveEnemies = new List<GameObject>(); 
	
	/// <summary>
	///		List of currently spawned in collectable items 
	/// </summary>
	[SerializeField] private List<GameObject> spawnedGameItemsRemaining = new List<GameObject>();
	
	/// <summary>
	///		A reference to the current player in the scene 
	/// </summary>
	[SerializeField] private Transform m_currentPlayerReference;

	/// <summary>
	///		The total player kills 
	/// </summary>
	[SerializeField] private int totalPlayerScore;

	/// <summary>
	///		The total amount of player lives remaining 
	/// </summary>
	[SerializeField] private int currentLives;
	[SerializeField] private int healthPackSpawnAmount;
	[SerializeField] private int ammunitionPackSpawnAmount;
	[SerializeField] private int tankAISpawnAmount; 




	#region Unity References 

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


		FireModeEvents.IncreaseLivesEvent += IncreaseLives;
		FireModeEvents.IncreasePlayerScoreEvent += IncreaseScore;
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

		FireModeEvents.IncreaseLivesEvent -= IncreaseLives;
		FireModeEvents.IncreasePlayerScoreEvent -= IncreaseScore;
		FireModeEvents.IncreaseWaveEvent -= IncreaseWave;
		FireModeEvents.PlayerWinsEvent -= HandlePlayerWins;
		FireModeEvents.AIWinsEvent -= HandleAIWins;
		FireModeEvents.HandleAIDestroyedEvent -= DespawnEnemyAI;
		FireModeEvents.HandleOnGameItemDestroyed -= DespawnGameItem;
	}

	#endregion


	/// <summary>
	///		What do we want to do with the spawned in player entity? 
	/// </summary>
	/// <param name="PlayerEntity"></param>
	private void SpawnedPlayerEntity(Transform PlayerEntity)
	{
		if (PlayerEntity.GetComponent<MainPlayerTank>())
		{
			m_currentPlayerReference = PlayerEntity.GetComponent<MainPlayerTank>().transform;
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
		m_AliveEnemies.Clear();

		for (int i = 0; i < spawnedAITanks.Count; i++)
		{

			// Add the newly spawned enemy to the alive enemies list 
			m_AliveEnemies.Add(spawnedAITanks[i]);
		}

		totalAITanksRemaining = m_AliveEnemies.Count;

		Debug.Log("[FireModeGameManager.SpawnedAI]: " + "Total AI Tanks Remaining: " + totalAITanksRemaining);
	}


	/// <summary>
	///		Handles Despawning of an enemy AI entity 
	/// </summary>
	/// <param name="AIEntity"></param>
	private void DespawnEnemyAI(GameObject AIEntity)
	{
		Debug.Log("[FireModeGameManager.DespawnEnemyAI]: " + "Attempting to despawn Entity " + AIEntity.name);


		if (AIEntity.GetComponent<AI>() == null)
		{
			Debug.Log("[FireModeGameManager.DespawnEnemyAI]: " + "That's not an enemy Entity!");
			return;
		}

		// Remove the AI Entity from the list of AI Entities
		m_AliveEnemies.Remove(AIEntity);

		
		//	Increase the playesr score by 1 
		FireModeEvents.IncreasePlayerScoreEvent?.Invoke(1);

		
		totalAITanksRemaining = m_AliveEnemies.Count;



		if (totalAITanksRemaining <= 0)
		{
			// Invoke the next wave event 
			Debug.Log("[FireModeGameManager.DespawnEnemyAI]: " + "Starting Next Wave Event! - There are no enemies remaining - " + totalAITanksRemaining);
			
			// Then we want to run the next round 
			FireModeEvents.HandleNextWaveStarted?.Invoke();
		}
	}

	/// <summary>
	///		Handles adding the currently spawned collectable items to the game manager's private list 
	/// </summary>
	private void SpawnedGameItems(List<GameObject> spawnedGameItems)
	{

		spawnedGameItemsRemaining.Clear();

		for (int i = 0; i < spawnedGameItems.Count; i++)
		{

			GameObject item = spawnedGameItems[i];

			spawnedGameItemsRemaining.Add(item);
		}


		totalGameItemsRemaining = spawnedGameItemsRemaining.Count;
	}

	/// <summary>
	///		Handles despawning of the pickup items 
	/// </summary>
	/// <param name="Pickup"></param>
	private void DespawnGameItem(GameObject Pickup)
	{
		// If the pickup item does not have the basic item pickup class 
		if (Pickup.GetComponent<BasicItemPickup>() == null)
		{
			// Then we want to return. 
			return;
		}

		// Remove the item from the list 
		spawnedGameItemsRemaining.Remove(Pickup);


		totalGameItemsRemaining = spawnedGameItemsRemaining.Count; 



		// If the total remaining items count is less than or equal to the trigger item respawn count 
		if (totalGameItemsRemaining <= triggerItemRespawnAmount)
		{
			
			Debug.Log("[FireModeGameManager.DespawnGameItem]: " + "Triggering Respawn of Items!");

			// respawn more items 
			FireModeEvents.SpawnGameItemsEvent?.Invoke(healthPackSpawnAmount, ammunitionPackSpawnAmount);


			

			Debug.Log("[FireModeGameManager.DespawnGameItem]: " + "Total items remaining " + totalGameItemsRemaining + " less than or equal to trigger respawn " + triggerItemRespawnAmount + ". Spawning " + startingHealthPacks + " Health, " + startingAmmunitionPacks + " Ammunition!");
			
		}
	}

	#region Game Methods 

	/// <summary>
	///		Starts Field Of Fire Game Mode!
	/// </summary>
	private void Start()
	{
		// Once the scene loads, Field of fire game is started!
		StartCoroutine(RunGameModeLogic());
	}


	/// <summary>
	///		Resets the current game's round 
	/// </summary>
	private void ResetGame()
	{
		currentWaveIndex = 1;
		currentLives = startingLives;
		totalPlayerScore = 0;
		healthPackSpawnAmount = startingHealthPacks;
		ammunitionPackSpawnAmount = startingAmmunitionPacks;
		tankAISpawnAmount = startingTanks;

		Debug.Log("[FireModeGameManager.ResetGame]: " + "Resetting current wave index, current lives and total players score!");
	}

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

		Debug.Log("[FireModeGameManager.RunGameModeLogic]: " + "Spawn Pickups Event Called! Starting Health Packs: " + startingHealthPacks + " Starting Ammunition Packs: " + startingAmmunitionPacks);
		FireModeEvents.SpawnGameItemsEvent(healthPackSpawnAmount, ammunitionPackSpawnAmount);

		// Wait X amount of seconds before spawning enemies and weapon pickups 
		yield return new WaitForSeconds(preWaveSetupTimer);

		Debug.Log("[FireModeGameManager.RunGameModeLogic]: " + "Spawn Event Wave Event called! Starting AI Tanks: " + tankAISpawnAmount);
		FireModeEvents.SpawnAIEvent(tankAISpawnAmount); // spawns the enemies...

		// Starts the first round! 
		Debug.Log("[FireModeGameManager.RunGameModeLogic]: " + "On Game Started Event has been called!");
		

		Invoke(nameof(GameStarted), 3f);
		yield return null; // Tells coroutine when the next update is 
	}

	/// <summary>
	///		Runs the next wave in the sequence 
	/// </summary>
	private void NextWave()
	{
		Debug.Log("[FireModeGameManager.NextWave]: " + "Starting next wave!!");
		int s_previousWaveIndex = currentWaveIndex;

		Debug.Log("[FireModeGameManager.NextWave]: " + "Previous Wave Index: " + s_previousWaveIndex);

		//	 Invoke Increase Wave Event by 1 
		FireModeEvents.IncreaseWaveEvent?.Invoke(1);
			
		// Increase the next wave health packs count 

		// Increase the next wave ammunition packs count 

		// Increase the next wave AI spawn count 

		// Invoke Spawn Game Items Event 

		// Invoke Spawn AI Wave Event 


		Debug.Break();	


		Debug.Log("[FireModeGameManager.NextWave]: " + "Calling GameStarted and running the next wave!");
		// Starts the game again and enables the AI tanks and pickups  
		Invoke(nameof(GameStarted), nextWaveStartTimer);
	}


	/// <summary>
	///		Starts the first wave (Game Started Event) 
	/// </summary>
	private void GameStarted()
	{
		// Starts the game 
		FireModeEvents.GameStartedEvent?.Invoke();
	}

	private void IncreaseLives(int amount)
	{
		currentLives += amount;

		if (currentLives <= 0)
		{
			Debug.Log("[FireModeGameManager.IncreaseLives]: " + "Player lives are less than or equal to 0! Lives: " + currentLives);



			// Hard Reset the Game 
			FireModeEvents.ResetGameEvent?.Invoke();
			Debug.Break();
		}
		else
		{
			// Update the UI to reflect the lives remaining 
			Debug.Log("[FireModeGameManager.IncreaseLives]: " + "Increase Lives Event UI Invoked! Lives Remaining: " + currentLives);

			FireModeEvents.IncreaseLivesEventUI?.Invoke(currentLives);
		}
	}

	/// <summary>
	///		Increases the current wave index 
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
			FireModeEvents.IncreaseWaveEventUI?.Invoke(currentWaveIndex);
		}
	}

	/// <summary>
	///		Increases the current players score 
	/// </summary>
	/// <param name="amount"></param>
	private void IncreaseScore(int amount)
	{
		totalPlayerScore += amount;

		Debug.Log("[FireModeGameManager.IncreaseScore]: " + "Increasing player's score " + totalPlayerScore + " by amount " + amount);

		FireModeEvents.IncreasePlayerScoreEventUI?.Invoke(totalPlayerScore);
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