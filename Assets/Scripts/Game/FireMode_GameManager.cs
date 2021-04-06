using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		Game Mode Manager 
/// </summary>
public class FireMode_GameManager : MonoBehaviour
{ 
  
	/// <summary>
	///		Instance of the fire mode game manager 
	/// </summary>
	public static FireMode_GameManager Instance;

	[Header("Game Mode Timers")]
	public float preGameSetupTimer = 3f; // seconds before game starts 
	public float nextWaveTimer = 15f; // seconds before the next wave 
	public float resetRoundTimer = 3f; // seconds before the round is restarted    

	[Header("Game Mode Settings")]
	public int startingEnemyTanks = 3;
	public int startingEnemyInfantry = 3;

	[SerializeField] private float totalEnemiesRemaining; // enemies remaining 
	[SerializeField] private int m_currentWaveCount; // the current wave the player is on 						 

	// Testing adding both tank lists to this one 
	[SerializeField] private List<GameObject> aliveEnemiesRemaining = new List<GameObject>();

	private Tank m_currentPlayerReference; // the player 
	[SerializeField] private int totalKillCount; // the total amount of player kills


	[SerializeField] private int m_spawnTankCount;
	[SerializeField] private int m_spawnInfantryCount;

	/// <summary>
	///		Handles spawning of the player, Enemy AI & Collectable Items 
	/// </summary>
	private void OnEnable()
	{
		FireModeEvents.OnEnemyWaveSpawnedEvent += SpawnedEntitys;
		FireModeEvents.OnObjectDestroyedEvent += DespawnEntity;
	}

	/// <summary>
	///		Handles Despawning / Destroying of the player, Enemy AI & Collectable Items 
	/// </summary>
	private void OnDisable()
	{
		FireModeEvents.OnEnemyWaveSpawnedEvent -= SpawnedEntitys;
		FireModeEvents.OnObjectDestroyedEvent -= DespawnEntity;
	}


	public int GetCurrentWave()
	{
		return m_currentWaveCount;
	}

	/// <summary>
	///		Handles spawning of the enemy tanks & enemy infantry AI characters  
	/// </summary>
	/// <param name="EnemyTanksSpawned"></param>
	/// <param name="EnemyCharactersSpawned"></param>
	private void SpawnedEntitys(List<GameObject> enemiesSpawned)
	{

		aliveEnemiesRemaining.Clear();

		for (int i = 0; i < enemiesSpawned.Count; i++)
		{

			if (!enemiesSpawned[i].GetComponent<TankAI>() || !enemiesSpawned[i].GetComponent<InfantryAI>())
			{
				continue;
			}
		
			GameObject _enemy = enemiesSpawned[i];

			aliveEnemiesRemaining.Add(_enemy);
		}

		totalEnemiesRemaining = enemiesSpawned.Count;

		FireModeEvents.OnUpdateWaveCountEvent?.Invoke(m_currentWaveCount);
	}

	/// <summary>
	///		Handles Despawning of an enemy AI entity 
	/// </summary>
	/// <param name="EnemyEntity"></param>
	private void DespawnEntity(Transform EnemyEntity)
	{
		Debug.Log("[FireMode_GameManager.DespawnEntity]: " + "Despawning Entity " + EnemyEntity.name);
		
		if (EnemyEntity.GetComponent<TankAI>() || EnemyEntity.GetComponent<Tank>() || EnemyEntity.GetComponent<InfantryAI>())
		{
			Debug.Log("[FireMode_GameManager.DespawnEntity]: " + "Entity not found " + EnemyEntity.name);
			return;
		}



		aliveEnemiesRemaining.Remove(EnemyEntity.gameObject);

		


		totalEnemiesRemaining = aliveEnemiesRemaining.Count;

		Debug.Log("[FireMode_GameManager.DespawnEntity]: " + "Enemies Remaining:  " + totalEnemiesRemaining);


		
	}

	/// <summary>
	///		Updates the players kill count
	/// </summary>
	private void UpdatePlayerKillCount()
	{
			FireModeEvents.OnUpdatePlayerKillsEvent?.Invoke(totalKillCount);
	}

	private void UpdateWaveCount()
	{
		FireModeEvents.OnUpdateWaveCountEvent?.Invoke(m_currentWaveCount);
	}


	/// <summary>
	///		Resets the game 
	/// </summary>
	private void RestartGameEvent()
	{
		FireModeEvents.OnRestartGameEvent?.Invoke();
		

		Invoke(nameof(StartFirstEnemyWave), preGameSetupTimer);
	}

	/// <summary>
	///		Starts the first wave of the attack ( On game start event ) 
	/// </summary>
	private void StartFirstEnemyWave()
	{
		m_currentWaveCount = 1;
		m_spawnTankCount = startingEnemyTanks;
		m_spawnInfantryCount = startingEnemyInfantry;

		FireModeEvents.SpawnEnemyWaveEvent(m_spawnInfantryCount, m_spawnTankCount);
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
		// Invoke Game Reset Event 

		FireModeEvents.OnRestartGameEvent?.Invoke(); // invoke restart game 

		// Spawn the player in 
		FireModeEvents.SpawnPlayerEvent?.Invoke();

		
		FireModeEvents.OnPreWaveEvent?.Invoke(); // call pre wave event which will display wave count down timer  

		// We use a pre game wait timer so we have time to setup the game events before
		yield return new WaitForSeconds(preGameSetupTimer);

		FireModeEvents.SpawnEnemyWaveEvent(startingEnemyInfantry, startingEnemyTanks); // spawns the enemies...

		// Starts the first round! 
		FireModeEvents.OnWaveStartedEvent?.Invoke(); // makes the enemies aggressive! 
		

		yield return null; // Tells coroutine when the next update is 
	}

}
