using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		Game Mode Manager 
/// </summary>
public class FireMode_GameManager : MonoBehaviour
{ 
  
	[Header("Game Mode Timers")]
	public float preGameSetupTimer = 3f; // seconds before game starts 
	public float nextWaveTimer = 15f; // seconds before the next wave 
	public float resetRoundTimer = 3f; // seconds before the round is restarted    

	[Header("Game Mode Settings")]
	public float startingEnemyTanks = 3;
	public float startingEnemyInfantry = 3;


	[SerializeField] private List<TankAI> enemyTanksSpawnedIn = new List<TankAI>(); // list of all the enemy AI tanks remaining 
	[SerializeField] private List<InfantryAI> enemyInfantrySpawnedIn = new List<InfantryAI>(); // list of enemy ai infantry remaining 
	[SerializeField] private float totalEnemiesRemaining; // enemies remaining 
	[SerializeField] private int currentWaveIndex; // the current wave the player is on 						 

	// Testing adding both tank lists to this one 
	[SerializeField] private List<GameObject> aliveEnemiesRemaining = new List<GameObject>();

	private Tank currentPlayer; // the player 
	[SerializeField] private int totalKillCount; // the total amount of player kills

	/// <summary>
	///		Handles spawning of the player, Enemy AI & Collectable Items 
	/// </summary>
	private void OnEnable()
	{
		FireModeEvents.OnEnemyWaveSpawnedEvent += SpawnedEnemyAI;
		FireModeEvents.OnObjectDestroyedEvent += DespawnEnemy;
	}

	/// <summary>
	///		Handles Despawning / Destroying of the player, Enemy AI & Collectable Items 
	/// </summary>
	private void OnDisable()
	{
		FireModeEvents.OnEnemyWaveSpawnedEvent -= SpawnedEnemyAI;
		FireModeEvents.OnObjectDestroyedEvent -= DespawnEnemy;
	}

	/// <summary>
	///		Handles spawning of the enemy tanks & enemy infantry AI characters  
	/// </summary>
	/// <param name="EnemyTanksSpawned"></param>
	/// <param name="EnemyCharactersSpawned"></param>
	private void SpawnedEnemyAI(List<GameObject> spawnedAICharacters, List<GameObject> spawnedAITanks)
	{

		enemyTanksSpawnedIn.Clear(); // clear the list of enemy ai tanks remaining 
		enemyInfantrySpawnedIn.Clear();
		aliveEnemiesRemaining.Clear();

		int spawnedInfantryIndex = 0;
		int spawnedTankIndex = 0;

		for (int i = 0; i < spawnedAICharacters.Count; i++)
		{
			if (!enemyInfantrySpawnedIn[i].GetComponent<InfantryAI>())
			{
				// Check for the infantry AI script 
				continue;
			}
			InfantryAI infantry = spawnedAICharacters[i].GetComponent<InfantryAI>();

			enemyInfantrySpawnedIn.Add(infantry);


			aliveEnemiesRemaining.Add(infantry.gameObject);

			spawnedInfantryIndex++;
		}

		for (int i = 0; i < spawnedAITanks.Count; i++)
		{
			if (!enemyTanksSpawnedIn[i].GetComponent<TankAI>())
			{
				continue;
			}
			TankAI tankAI = spawnedAITanks[i].GetComponent<TankAI>();

			enemyTanksSpawnedIn.Add(tankAI);

			aliveEnemiesRemaining.Add(tankAI.gameObject);

			spawnedTankIndex++;
		}


		totalEnemiesRemaining = spawnedInfantryIndex + spawnedTankIndex;
	}

	/// <summary>
	///		Handles Despawning of an enemy AI entity 
	/// </summary>
	/// <param name="EnemyEntity"></param>
	private void DespawnEnemy(Transform EnemyEntity)
	{
		if (EnemyEntity.GetComponent<TankAI>() == null && EnemyEntity.GetComponent<InfantryAI>() == null)
		{
			return;
		}
		

		bool isTankAI = EnemyEntity.GetComponent<TankAI>() == true;
		bool isInfantryAI = EnemyEntity.GetComponent<InfantryAI>() == true;


		

		if (isTankAI)
		{
			enemyTanksSpawnedIn.Remove(EnemyEntity.GetComponent<TankAI>());
		}
		else if (isInfantryAI)
		{
			enemyInfantrySpawnedIn.Remove(EnemyEntity.GetComponent<InfantryAI>());
		}


		if (aliveEnemiesRemaining.Count <= 1)
		{
			Debug.Log("Alive Enemies Remaining: " + aliveEnemiesRemaining.Count + " Total Enemies Remaining: " + totalEnemiesRemaining);



			FireModeEvents.OnPostWaveEvent?.Invoke();

			// Invoke("ResetRound", resetRoundTimer); (3 seconds or whatever) 
			Invoke("ResetEnemyWave", resetRoundTimer);
		}
	}

	/// <summary>
	///		Updates the games scoreboard 
	/// </summary>
	private void UpdatePlayerKillCount()
	{
		if (currentPlayer != null)
		{
			FireModeEvents.UpdatePlayerKillsEvent?.Invoke(totalKillCount);
		}
	}


	/// <summary>
	///		Invokes events that happen on the rounds reset 
	/// </summary>
	private void ResetEnemyWave()
	{
		FireModeEvents.OnResetWaveEvent?.Invoke();
		

		// Invokes StartEnemyWaves() 
	}

	/// <summary>
	///		Starts the first wave of the attack ( On game start event ) 
	/// </summary>
	private void StartFirstEnemyWave()
	{
		currentWaveIndex = 1;

	}

	/// <summary>
	/// 	Initializes the start of Field of Fire 
	/// </summary>
	private void Start()
	{
		StartCoroutine(StartFieldOfFire());
	}

	/// <summary>
	///		Starts Field of Fire in a custom updated function. Allows you to control when / where to update the game events and what game events to run? 
	/// </summary>
	/// <returns></returns>
	private IEnumerator StartFieldOfFire()
	{
		if (AudioManager.Instance != null)
		{
			AudioManager.Instance.PlaySound(GameAudio.BackgroundThemeTrack);
		}
		// Invoke Game Reset Event 

		FireModeEvents.OnRestartGameEvent?.Invoke(); // invoke restart game 
													
		FireModeEvents.OnPreWaveEvent?.Invoke(); // call pre game event 

		

		// We use a pre game wait timer so we have time to setup the game events before
		// starting
		yield return new WaitForSeconds(preGameSetupTimer);

		// Run the game started event 


		// Then spawn in the enemy ai 



		yield return null; // Tells coroutine when the next update is 
	}

}
