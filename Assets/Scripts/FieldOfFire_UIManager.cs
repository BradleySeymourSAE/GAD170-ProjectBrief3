using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FieldOfFire_UIManager : MonoBehaviour
{
	public MainMenu MainMenuUI;
	public CreditsMenu CreditsMenuUI;



	private void Awake()
	{
		if (AudioManager.Instance != null)
		{
			AudioManager.Instance.PlaySound(GameAudio.BackgroundThemeTrack);
		}
	}

	private void Start()
	{
		MainMenuUI.Setup(this);
		CreditsMenuUI.Setup(this);

		

		DisplayMainMenu(true);
		DisplayCreditsMenu(false);
	}
	/// <summary>
	///		Opens the credits menu 
	/// </summary>
	public void DisplayMainMenu(bool show)
	{
		MainMenuUI.ShowDisplay(show);
	}

	/// <summary>
	///		Shows the main menu 
	/// </summary>
	public void DisplayCreditsMenu(bool show)
	{
		CreditsMenuUI.ShowDisplay(show);
	}

}
