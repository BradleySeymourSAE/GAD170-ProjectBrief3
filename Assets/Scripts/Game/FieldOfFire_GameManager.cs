using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfFire_GameManager : MonoBehaviour
{
  
	[Header("Game Timers")]
	public float preGameSetupTimer = 3f; // seconds before game starts 
	
	private int totalPlayerKills; // the total amount of kills the player has got
							 
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
	///		Starts the first wave of the attack ( On game start event ) 
	/// </summary>
	private void StartWave()
	{

	}

	/// <summary>
	///		Invokes events that happen on the rounds reset 
	/// </summary>
	private void ResetRound()
	{


		// Invokes StartWave() 
	}




	// Initializes the start of Field of Fire 
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
