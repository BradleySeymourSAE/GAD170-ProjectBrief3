using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///		Game UI Manager
/// </summary>
[System.Serializable]
public class FireMode_UIManager : MonoBehaviour
{
	private const string styled = "---";

	public GameObject InGameWaveUI;
	public GameObject PostGameResultsUI;


	/// <summary>
	///		Once Enabled, Listen to events 
	/// </summary>
	private void OnEnable()
	{
		
	}

	/// <summary>
	///		Remove event listeners
	/// </summary>
	private void OnDisable()
	{
		
	}


	

	/// <summary>
	///		Displays UI Pre Game 
	/// </summary>
	private void PreGame()
	{

	}

	/// <summary>
	///		Displays the In-Game UI
	/// </summary>
	private void InGame()
	{

	}

	/// <summary>
	///		Displays the scoreboard during the game
	///		1) If a wave is currently going
	///		2) If there are enemies 
	/// </summary>
	private void DisplayScoreboard()
	{
		
	}

	/// <summary>
	///		Updates the scoreboard during the game 
	/// </summary>
	/// <param name="p_currentPlayer"></param>
	/// <param name="Score"></param>
	private void UpdateScoreboardText(Player p_currentPlayer, int Score)
	{

	}

	/// <summary>
	///		Event called once the round has completed
	/// </summary>
	private void PostRound()
	{

	}
}

