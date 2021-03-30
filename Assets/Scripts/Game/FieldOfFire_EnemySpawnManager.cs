using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FieldOfFire_EnemySpawnManager : MonoBehaviour
{

	/// <summary>
	///		Spawn Points & Starting Spawn Positions 
	/// </summary>
	public List<Transform> enemySpawnPoints = new List<Transform>(); // list of enemy spawn points 
	private List<Transform> startingSpawns = new List<Transform>(); // store starting spawn points 


	/// <summary>
	///		Enemy Character Prefabs (Soldiers) 
	/// </summary>
	public List<GameObject> enemyCharacterPrefabs = new List<GameObject>(); // list of possible enemy character prefabs 
	private List<GameObject> aliveEnemyCharactersSpawned = new List<GameObject>(); // The currently alive enemy characters spawned in 

	public List<GameObject> enemyTankPrefabs = new List<GameObject>(); // List of enemy tank prefabs to spawn in 
	private List<GameObject> aliveEnemyTanksSpawned = new List<GameObject>(); // currently alive enemy tanks spawned in 



	/// <summary>
	///		Enemy Spawn Manager Event Listeners 
	/// </summary>
	private void OnEnable()
	{
		
	}

	/// <summary>
	///		Enemy Spawn Manager Event Listener Cleanups 
	/// </summary>
	private void OnDisable()
	{
		
	}

	/// <summary>
	///		Debugging Helper
	/// </summary>
	private void OnDrawGizmos()
	{
		
	}

	private void ResetGame()
	{
		// Destroy game objects 

		// Clear the tank lists 

		// add possible spawn points 
	}

	private void SpawnEnemyWave(int AmountOfCharacters, int AmountOfTanks)
	{
		// Check if tank prefabs count is less than or equal to amountoftanks to spawn and that all spawnpoints.count is greater than or equal to amount of tanks to spawn 

		// if it is, i = 0, loop through the amount of tanks to spawn increase i. 

		// Transform temp is random range between starting spawn points and the count 
		// game object clone instantiate here 
		
		// remove temp spawn point from list 
		// spawn in tanks 
		

		// Do the same for characters 


		// Otherwise, log an error 



		// Call GameEvents.OnSpawnedEvent?.Invoke(listOfEnemyCharacters, listOfEnemyTanks)
	}
}
