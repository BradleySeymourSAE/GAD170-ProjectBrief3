using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
///		Main Menu UI Data Class 
/// </summary>
[System.Serializable]
public class MainMenu
{
	
	public GameObject MainMenuScreen; // reference to our main menu container
	public TMP_Text GameTitle; // game title text
	public TMP_Text GameTagline; // game tagline / description text
	public TMP_Text GameVersion; // game version text

	public Button Start; // reference to start button 
	public Button Credits; // reference to credits button
	public Button Leave; // reference to leave button

	private FireMode_UIManager m_UIManager; // reference to our UI manager 

	public void Setup(FireMode_UIManager UIManager)
	{
		m_UIManager = UIManager;

		// Set the text for the main menu ui 
		GameTitle.text = GameTextUI.MainMenu_GameTitle;
		GameTagline.text = GameTextUI.MainMenu_GameSubtitle;
		GameVersion.text = GameTextUI.MainMenu_GameVersion;


		// Start Button - Reset Text & Listeners 
		Start.GetComponentInChildren<Text>().text = GameTextUI.MainMenu_Start;
		Start.onClick.RemoveAllListeners(); // Remove default event listener
		Start.onClick.AddListener(StartGame); // add listener to our start button reference 

		// Credits Button - Reset Text & Listeners 
		Credits.GetComponentInChildren<Text>().text = GameTextUI.MainMenu_Credits;
		Credits.onClick.RemoveAllListeners();
		Credits.onClick.AddListener(OpenCreditsMenu);
		
		Leave.GetComponentInChildren<Text>().text = GameTextUI.MainMenu_Quit;
		Leave.onClick.RemoveAllListeners();
		Leave.onClick.AddListener(QuitApplication);

	}

	public void ShowDisplay(bool Display)
	{
		MainMenuScreen.SetActive(Display);
	}



	/// <summary>
	///		Starts our application
	/// </summary>
	private void StartGame()
	{
		Debug.Log("[MainMenu.StartGame]: " + "Loading Game Scene: " + GameScenes.GameLevel_01);
		SceneManager.LoadScene(GameScenes.GameLevel_01);
	}

	/// <summary>
	///	Displays the credits menu 
	/// </summary>
	/// <param name="Show"></param>
	private void OpenCreditsMenu()
	{
		ShowDisplay(false);
		m_UIManager.DisplayCredits(true);
	}

	/// <summary>
	///		Quits the application 
	/// </summary>
	private void QuitApplication()
	{
		#if UNITY_STANDALONE
			Debug.Log("[MainMenu.QuitApplication]: " + "Quitting Application!");
			Application.Quit();
#endif

		// Running in the editor 

#if UNITY_EDITOR
		// Stop playing the scene 
		Debug.Log("[MainMenu.QuitApplication]: " + "Running in the Editor - Editor application has stopped playing!");
		UnityEditor.EditorApplication.isPlaying = false;
		#endif
	}
}