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
	public int startingTanks = 2;
	
	/// <summary>
	///		The starting amount of health packs to spawn into the game 
	/// </summary>
	public int startingHealthPacks = 3;
	
	/// <summary>
	///		The starting amount of ammunition packs to spawn into the game 
	/// </summary>
	public int startingAmmunitionPacks = 5;

	/// <summary>
	///		The index at which more items will be spawned during a round 
	/// </summary>
	/// 
	public int triggerItemRespawnAmount = 2;

	/// <summary>
	///		The starting amount of lives for a player 
	/// </summary>
	public int startingLives = 3;


	
	/// <summary>
	///		The total amount of collectable items remaining 
	/// </summary>
	[Header("Debugging")]
	[SerializeField] private int totalCollectableItemsRemaining;

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

	/// <summary>
	///		The total player kills 
	/// </summary>
	[SerializeField] private int totalPlayerScore;

	/// <summary>
	///		The total amount of player lives remaining 
	/// </summary>
	[SerializeField] private int currentLives;





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
	private void SpawnedAI(List<GameObject> enemiesSpawned)
	{

		m_AliveEnemies.Clear();

		for (int i = 0; i < enemiesSpawned.Count; i++)
		{

			// Add the newly spawned enemy to the alive enemies list 
			m_AliveEnemies.Add(enemiesSpawned[i]);
		}

		totalEnemiesRemaining = m_AliveEnemies.Count;
	}


	/// <summary>
	///		Handles Despawning of an enemy AI entity 
	/// </summary>
	/// <param name="EnemyEntity"></param>
	private void DespawnEnemyAI(Transform EnemyEntity)
	{
		Debug.Log("[FireMode_GameManager.DespawnEntity]: " + "Attempting to despawn Entity " + EnemyEntity.name);








		if (totalEnemiesRemaining <= 0)
		{
			// Invoke the next wave event 
			Debug.Log("[FireMode_GameManager.DespawnEntity]: " + "Starting Next Wave Event! - There are no enemies remaining - " + totalEnemiesRemaining);
			
			// Then we want to run the next round 
			FireModeEvents.HandleOnNextWaveEvent?.Invoke();
		}
	}

	/// <summary>
	///		Handles adding the currently spawned collectable items to the game manager's private list 
	/// </summary>
	private void SpawnedGameItems(List<GameObject> spawnedCollectableItems)
	{

		spawnedCollectableItemsRemaining.Clear();

		for (int i = 0; i < spawnedCollectableItems.Count; i++)
		{

			GameObject item = spawnedCollectableItems[i];

			spawnedCollectableItemsRemaining.Add(item);
		}


		totalCollectableItemsRemaining = spawnedCollectableItemsRemaining.Count;
	}

	/// <summary>
	///		Handles despawning of the pickup items 
	/// </summary>
	/// <param name="Pickup"></param>
	private void CollectedItem(Transform Pickup)
	{
		// If the pickup item does not have the basic item pickup class 
		if (Pickup.GetComponent<BasicItemPickup>() == null)
		{
			// Then we want to return. 
			return;
		}

		// Remove the item from the list 
		spawnedCollectableItemsRemaining.Remove(Pickup.gameObject);


		totalCollectableItemsRemaining = spawnedCollectableItemsRemaining.Count; 



		// If the total remaining items count is less than or equal to the trigger item respawn count 
		if (totalCollectableItemsRemaining <= triggerItemRespawnAmount)
		{
	
			// respawn more items 

			

			Debug.Log("[FireMode_GameManager.CollectedItem]: " + "Total items remaining " + totalCollectableItemsRemaining + " less than or equal to trigger respawn " + triggerItemRespawnAmount + ". Spawning " + startingHealthPacks + " Health, " + startingAmmunitionPacks + " Ammunition!");
			
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


		Debug.Log("[FireModeGameManager.ResetGame]: " + "Resetting current wave index, current lives and total players score!");
	}

	/// <summary>
	///		Starts Field of Fire in a custom updated function. Allows you to control when / where to update the game events and what game events to run? 
	/// </summary>
	/// <returns></returns>
	private IEnumerator RunGameModeLogic()
	{
		Debug.Log("[FireMode_GameManager.RunGameModeLogic]: " + "Running game mode logic!");
		FireModeEvents.ResetGameEvent?.Invoke(); 

		FireModeEvents.SpawnPlayerEvent?.Invoke(); // spawn the player in

		FireModeEvents.PreGameStartedEvent?.Invoke(); 

		// Wait X amount of seconds before spawning enemies and weapon pickups 
		yield return new WaitForSeconds(preWaveSetupTimer);

		Debug.Log("[FireMode_GameManager.StartFieldOfFire]: " + "Spawn Pickups Event Called! Starting Health Packs: " + startingHealthPacks + " Starting Ammunition Packs: " + startingAmmunitionPacks);
		FireModeEvents.SpawnGameItemsEvent(startingHealthPacks, startingAmmunitionPacks);

		Debug.Log("[FireMode_GameManager.StartFieldOfFire]: " + "Spawn Event Wave Event called! Starting AI Tanks: " + startingTanks);
		FireModeEvents.SpawnAIEvent(startingTanks); // spawns the enemies...

		// Starts the first round! 
		Debug.Log("[FireMode_GameManager.StartFieldOfFire]: " + "On Game Started Event has been called!");
		

		Invoke("GameStarted", 3f);
		yield return null; // Tells coroutine when the next update is 
	}

	/// <summary>
	///		Runs the next wave in the sequence 
	/// </summary>
	private void NextWave()
	{
		Debug.Log("[FireMode_GameManager.NextWave]: " + "Starting next wave!!");
	
			
		Debug.Break();	
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
			Debug.Break();
		}

		// Hard Reset the Game 
		FireModeEvents.ResetGameEvent?.Invoke();
	}


	/// <summary>
	///		Increases the current wave index 
	/// </summary>
	/// <param name="amount"></param>
	private void IncreaseWave(int amount)
	{
		currentWaveIndex += amount;

		Debug.Log("[FireModeGameManager.IncreaseWave]: " + "Increased wave " + (currentWaveIndex - 1) + " by amount " + amount);
		
		if (currentWaveIndex > 3)
		{
			Debug.Log("[FireModeGameManager.IncreaseWave]: " + "Player has won the game!");
			Debug.Break();
		}

		FireModeEvents.IncreaseWaveEventUI?.Invoke(currentWaveIndex);
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
		

		Debug.Break();
	}
	#endregion
}