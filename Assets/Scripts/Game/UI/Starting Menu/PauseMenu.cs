#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
#endregion

/// <summary>
///		Menu to display when a player wants to pause the game 
/// </summary>
[System.Serializable]
public class PauseMenu
{

	#region Public Variables 
	/// <summary>
	///		The pause menu screen game object reference 
	/// </summary>
	public GameObject pauseMenuScreen;

	/// <summary>
	///		The Pause Menu Title 
	/// </summary>
	public TMP_Text pauseMenuTitle;
	
	/// <summary>
	///		Resumes the game 
	/// </summary>
	public Button Resume;
	/// <summary>
	///		Exits back into the game 
	/// </summary>
	public Button ExitToMain;

	#endregion

	#region Private Variables 

	/// <summary>
	///		Reference to the Fire Mode UI Class 
	/// </summary>
	private FireModeUI m_FireModeUI;

	#endregion

	#region Public Methods 

	/// <summary>
	///		Sets up the UI references 
	/// </summary>
	/// <param name="FireModeUIReference"></param>
	public void Setup(FireModeUI FireModeUIReference)
	{
		m_FireModeUI = FireModeUIReference;

		pauseMenuTitle.GetComponentInChildren<TMP_Text>().text = GameTextUI.PauseMenu_Title;
		
		// Handles resuming the game! 
		Resume.GetComponentInChildren<TMP_Text>().text = GameTextUI.PauseMenu_Resume;
		Resume.onClick.RemoveAllListeners();
		Resume.onClick.AddListener(ResumeGame);
		
		// Handles Exiting the game back to the main menu 
		ExitToMain.GetComponentInChildren<TMP_Text>().text = GameTextUI.PauseMenu_Exit;
		ExitToMain.onClick.RemoveAllListeners();
		ExitToMain.onClick.AddListener(QuitToMainMenu);
	}

	/// <summary>
	///		Whether the pause menu should be displayed or not 
	/// </summary>
	/// <param name="ShouldDisplay"></param>
	public void ShowDisplay(bool ShouldDisplay) => pauseMenuScreen.SetActive(ShouldDisplay);

	/// <summary>
	///		Pauses the game 
	/// </summary>
	public void PauseGame()
	{
		Debug.Log("[PauseMenu.PauseGame]: " + "The game has been paused and the time scale has been set to 0!");
		Time.timeScale = 0;
		ShowDisplay(true);
	}


	#endregion

	#region Private Methods 
	
	/// <summary>
	///		Resumes the game from where the player left off
	/// </summary>
	private void ResumeGame()
	{
		Debug.Log("[PauseMenu.ResumeGame]: " + "The game has been resumed and the time scale has been set back to 1!");
		Time.timeScale = 1;
		ShowDisplay(false);
	}

	/// <summary>
	///		Calls UnityEngine.SceneManagement and returns back to the starting menu 
	/// </summary>
	private void QuitToMainMenu()
	{
		ShowDisplay(false);

		// Then we want to return to the main menu! 
		Debug.Log("[PauseMenu.QuitToMainMenu]: " + "Exiting back to the starting menu! Loading Scene: " + GameScenes.StartingMenu);
		SceneManager.LoadScene(GameScenes.StartingMenu);
	}

	#endregion

}