#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
///		Handles the Main Menu, Credits Menu & Settings Menu UI
/// </summary>
[System.Serializable]
public class FieldOfFire_UIManager : MonoBehaviour
{

	#region Public Variables 
	
	/// <summary>
	///		The Main Menu UI Reference 
	/// </summary>
	public MainMenu MainMenuUI;

	/// <summary>
	///		The Credits Menu UI Reference 
	/// </summary>
	public CreditsMenu CreditsMenuUI;
	// public SettingsMenu SettingsMenuUI; -- future reference 

	#endregion

	#region Unity References 
	private void Start()
	{
		//	 Checks for an audio manager instance 
		if (AudioManager.Instance)
		{
			// Begins playing the Background Theme Track 
			AudioManager.Instance.PlaySound(GameAudio.BackgroundThemeTrack);
		}

		// Sets up Main Menu & Credits Menu References 
		MainMenuUI.Setup(this);
		CreditsMenuUI.Setup(this);

		
		// Default to displaying the main menu and hiding the credits menu 
		DisplayMainMenu(true);
		// DisplaySettingsMenu(false); -- For future reference 
		DisplayCreditsMenu(false);
	}

	#endregion

	#region Public Methods 

	/// <summary>
	///		Opens the credits menu 
	/// </summary>
	public void DisplayMainMenu(bool show) => MainMenuUI.ShowDisplay(show);
	

	/// <summary>
	///		Displays the credits menu
	/// </summary>
	public void DisplayCreditsMenu(bool show) => CreditsMenuUI.ShowDisplay(show);

	/// <summary>
	///		Plays the menu select sound effect
	/// </summary>
	public void PlayMenuSelect() => AudioManager.Instance.PlaySound(GameAudio.GUI_Select);

	/// <summary>
	///		Plays the menu switch sound effect 
	/// </summary>
	public void PlayMenuSwitch() => AudioManager.Instance.PlaySound(GameAudio.GUI_MenuSwitch);

	#endregion

}
