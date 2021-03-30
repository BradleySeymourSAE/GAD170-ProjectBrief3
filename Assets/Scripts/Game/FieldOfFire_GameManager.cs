using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfFire_GameManager : MonoBehaviour
{
  
	[Header("Game Timers")]
	public float preGameSetupTimer = 3f; // seconds before game starts 
	public float nextWaveTimer = 15f; // seconds before the next wave 
	public float resetRoundTimer = 3f; // seconds before the round is restarted    

	private List<Tank> enemyTanksRemaining = new List<Tank>(); // list of all the enemy AI tanks remaining 
	// private List<EnemyAI_Infantry> = new List<EnemyAI_Infantry>(); // list of enemy ai infantry remaining 

	
	private int totalRoundsWon; // the total amount of kills the player has got
							 
	private Tank mainPlayer;


	/// <summary>
	///		Handles spawning of the player, Enemy AI & Collectable Items 
	/// </summary>
	private void OnEnable()
	{

	}

	/// <summary>
	///		Handles Despawning / Destroying of the player, Enemy AI & Collectable Items 
	/// </summary>
	private void OnDisable()
	{
		
	}

	/// <summary>
	///		Handles spawning of the enemy tanks & enemy infantry AI characters  
	/// </summary>
	/// <param name="EnemyTanksSpawned"></param>
	/// <param name="EnemyCharactersSpawned"></param>
	private void SpawnEnemyAI(List<GameObject> EnemyTanksSpawned, List<GameObject> EnemyCharactersSpawned)
	{

		enemyTanksRemaining.Clear(); // clear the list of enemy ai tanks remaining 
		// clear enemy infantry remaining list 

		// Loop through
			// If NOT index of tanksSpawnedIn get component tank then continue. 

		// Tank temp = EnemyTanksSpawned[i].GetComponent<Tank>(); // store a reference to the tank 
		// add the tank to the enemyTanks remaining list 

		// Update the scores 

		// Do the same for the enemy characters spawned list 
	}

	/// <summary>
	///		Handles Despawning of an enemy AI entity 
	/// </summary>
	/// <param name="EnemyEntity"></param>
	private void DespawnEnemyAI(Transform EnemyEntity)
	{
		// bool isTankEntity = true; 
		// if enemyEntity.GetComponent<Tank>() == null || enemyEntity.GetComponent<EnemyInfantryAI>() == null 

		// go through the enemy tanks spawned in list and remove the enemy entity index(tank ref) 

		// If remaining tanks count less than or equal to 0 && enemy ai characters count is less than or equal to 0 

		// If player is alive

		// Then the player has won  
		// EDIT: The player could have a currentKills int, or we could set that in the game manager. Have a think about it 
		// future bradley 

		// Update the scores 

		// Handle what happens on the round ended event 

		// If the player won -> 
			// After float nextRoundTimer = 5f (seconds) 
			// Invoke("NextRound", nextRoundTimer);


		// Otherwise, Reset the round 

		// Invoke("ResetRound", resetRoundTimer); (3 seconds or whatever) 

	}

	/// <summary>
	///		Updates the games scoreboard 
	/// </summary>
	private void UpdateScoreboard()
	{
		if (mainPlayer.playerNumber == Player.One)
		{
			// On Score updated event. 
			// We only have a single player here as we are using AI so Ill just do it like this i guess 
		}	
	}


	/// <summary>
	///		Invokes events that happen on the rounds reset 
	/// </summary>
	private void ResetRound()
	{


		// Invokes StartEnemyWaves() 
	}

	/// <summary>
	///		Starts the first wave of the attack ( On game start event ) 
	/// </summary>
	private void StartFirstEnemyWave()
	{

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

		// Invoke pre game event 

		// Invoke the tank setup events 

		// We use a pre game wait timer so we have time to setup the game events before
		// starting
		yield return new WaitForSeconds(preGameSetupTimer);

		// Run the game started event 


		// Then spawn in the enemy ai 



		yield return null; // Tells coroutine when the next update is 
	}

}
