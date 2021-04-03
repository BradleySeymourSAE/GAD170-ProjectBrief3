using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


/// <summary>
///		Credits Menu Data Class 
/// </summary>
[System.Serializable]
public class CreditsMenu
{

	public GameObject CreditsMenuScreen; // credits menu reference
	public TMP_Text CreditsTitle; // credits title text 
	public TMP_Text DevelopmentTeamTitle; // development team text 
	public TMP_Text DevelopmentTeamRole; // development team role 
	public TMP_Text Developer;  // developer name 
	public Button Exit; // exit button ref


	private FireMode_UIManager m_UIManager; // ui manager reference class 

	public void Setup(FireMode_UIManager UIManager)
	{
		m_UIManager = UIManager;

		CreditsTitle.text = GameTextUI.CreditsMenu_GameTitle;
		DevelopmentTeamTitle.text = GameTextUI.CreditsMenu_DevelopmentTeamTitle;
		DevelopmentTeamRole.text = GameTextUI.CreditsMenu_DevelopmentTeamRole;
		Developer.text = GameTextUI.CreditsMenu_Developer;


		Exit.GetComponentInChildren<Text>().text = GameTextUI.CreditsMenu_Exit;
		Exit.onClick.RemoveAllListeners();
		Exit.onClick.AddListener(OpenMainMenu);
	}

	public void ShowDisplay(bool Show)
	{
		CreditsMenuScreen.SetActive(Show);
	}

	public void OpenMainMenu()
	{
		ShowDisplay(false);
		m_UIManager.DisplayMainMenu(true);
	}
}