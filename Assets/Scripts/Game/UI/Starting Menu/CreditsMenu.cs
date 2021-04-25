#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
#endregion

/// <summary>
///		Credits Menu Data Class 
/// </summary>
[System.Serializable]
public class CreditsMenu
{

	#region Public Variables 

	public GameObject CreditsMenuScreen; // credits menu reference
	public TMP_Text CreditsTitle; // credits title text 
	public TMP_Text DevelopmentTeamTitle; // development team text 
	public TMP_Text Developer;  // developer name 

	public TMP_Text InspirationHeader;
	public TMP_Text InspirationMentionDice;
	public TMP_Text InspirationMentionSebastian;
	public Button Exit; // exit button ref

	#endregion

	#region Private Variables 

	private FieldOfFire_UIManager m_UIManager; // ui manager reference class 

	#endregion

	#region Public Methods 

	/// <summary>
	///		Sets up references to the UI Manager 
	/// </summary>
	/// <param name="UIManager"></param>
	public void Setup(FieldOfFire_UIManager UIManager)
	{
		m_UIManager = UIManager;

		// Credits Header & Development Team 
		CreditsTitle.text = GameTextUI.CreditsMenu_GameTitle;
		DevelopmentTeamTitle.text = GameTextUI.CreditsMenu_DevelopmentTeamTitle;
		Developer.text = GameTextUI.CreditsMenu_Developer;

		// Design Inspiration Header & Inspirations
		InspirationHeader.text = GameTextUI.CreditsMenu_InspirationHeader;
		InspirationMentionDice.text = GameTextUI.CreditsMenu_DiceMention;
		InspirationMentionSebastian.text = GameTextUI.CreditsMenu_SebastianLagueMention;



		Exit.GetComponentInChildren<Text>().text = GameTextUI.CreditsMenu_Exit;
		Exit.onClick.RemoveAllListeners();
		Exit.onClick.AddListener(OpenMainMenu);
	}

	/// <summary>
	///		 Toggles displaying the credits menu 
	/// </summary>
	/// <param name="Show"></param>
	public void ShowDisplay(bool Show) => CreditsMenuScreen.SetActive(Show);

	/// <summary>
	///		Opens the main menu and hides the credits menu 
	/// </summary>
	public void OpenMainMenu()
	{
		ShowDisplay(false);
		m_UIManager.DisplayMainMenu(true);
	}

	#endregion

}