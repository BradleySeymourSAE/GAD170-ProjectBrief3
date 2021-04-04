using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FieldOfFire_UIManager : MonoBehaviour
{

	public MainMenu mainMenu;
	public CreditsMenu creditsMenu;



	private void Start()
	{
		mainMenu.Setup(this);
		creditsMenu.Setup(this);

		

		DisplayMainMenu(true);
		DisplayCredits(false);
	}
	/// <summary>
	///		Opens the credits menu 
	/// </summary>
	public void DisplayCredits(bool show)
	{
		creditsMenu.ShowDisplay(show);
	}

	/// <summary>
	///		Shows the main menu 
	/// </summary>
	public void DisplayMainMenu(bool show)
	{
		mainMenu.ShowDisplay(show);
	}

}
