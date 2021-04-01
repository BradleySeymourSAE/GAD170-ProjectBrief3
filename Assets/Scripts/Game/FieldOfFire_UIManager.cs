using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FieldOfFire_UIManager : MonoBehaviour
{
	private const string styled = "---";
	public GameObject MainMenuUI;
	public GameObject InGameUI;
	public GameObject PostGameUI;

	public PlayerUserInterface userInterface;


	[Header(styled + " Audio Settings " + styled)]
	public Slider[] volumeSliders;

	[Header(styled + " Video Settings " + styled)]
	public TMP_Dropdown[] videoSettingDropdowns;

	[Header(styled + " Display Settings " + styled)]
	public Toggle toggleFullscreen;
	public Toggle toggleEnableVSync;


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

[System.Serializable]
public class PlayerUserInterface
{
	public Player playerRef;
	public TMP_Text kdrText;


	/// <summary>
	///  Disables the current players ui text 
	/// </summary>
	public void DisableUIText()
	{
		kdrText.gameObject.SetActive(false);
	}

	/// <summary>
	///  Enables player ui text 
	/// </summary>
	public void EnableUIText(Player currentPlayer)
	{
		// If the Player reference is equal to the current player input 
		if (playerRef == currentPlayer)
		{
			// Then we want to set the player text game object to true (Turn it on) 
			kdrText.gameObject.SetActive(true);
		}
	}

	/// <summary>
	///		Sets the player UI to the players current total kills  
	/// </summary>
	/// <param name="currentPlayerRef"></param>
	/// <param name="KillCount"></param>
	public void SetPlayerKills(Player currentPlayerRef, int KillCount)
	{
		if (currentPlayerRef == playerRef)
		{
			kdrText.text = "Player " + playerRef.ToString() + ": " + KillCount; 
		}
	}
}
