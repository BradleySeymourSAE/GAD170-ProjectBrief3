#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#endregion


/// <summary>
///		Displays In-Game UI
/// </summary>
[System.Serializable]
public class InGameWaveUI
{
	#region Public Variables 
	public GameObject inGameWaveUI;

	/// <summary>
	///		Player Kills UI 
	/// </summary>
	public PlayerKillsUI playerKillsUI;
	
	/// <summary>
	///		The Wave Counter UI 
	/// </summary>
	public WaveCounterUI waveCounterUI;

	#endregion

	#region Private Variables 
	/// <summary>
	///		A reference to the Fire mode UI instance 
	/// </summary>
	private FireModeUI m_FireModeUI;
	#endregion

	#region Public Methods 
	/// <summary>
	///		Runs the setup for in game UI 
	/// </summary>
	/// <param name="FireModeUI"></param>
	public void Setup(FireModeUI FireModeUI)
	{
		m_FireModeUI = FireModeUI;

		
		playerKillsUI.Setup(m_FireModeUI);
		waveCounterUI.Setup(m_FireModeUI);

		playerKillsUI.Show(true);
		waveCounterUI.Show(true);
	}

	/// <summary>
	///		Toggles whether the In Game Wave UI should be display 
	/// </summary>
	/// <param name="ShouldDisplay"></param>
	public void ShowScreen(bool ShouldDisplay)
	{
		inGameWaveUI.SetActive(ShouldDisplay);
	}

	/// <summary>
	///		Sets the players total kills 
	/// </summary>
	/// <param name="kills"></param>
	public void SetTotalPlayerKills(int kills)
	{
		playerKillsUI.TotalAllTimeKills.GetComponentInChildren<TMP_Text>().text = kills.ToString();
	}

	/// <summary>
	///		Sets In Game Wave UI fields 
	/// </summary>
	/// <param name="nextWave"></param>
	/// <param name="EnemiesRemaining"></param>
	/// <param name="WaveEnemiesKilled"></param>
	public void SetWave(int nextWave, int EnemiesRemaining, int WaveEnemiesKilled)
	{
		waveCounterUI.currentWave.GetComponentInChildren<TMP_Text>().text = nextWave.ToString();
		waveCounterUI.enemiesRemaining.GetComponentInChildren<TMP_Text>().text = EnemiesRemaining.ToString();
		playerKillsUI.WaveKills.GetComponentInChildren<TMP_Text>().text = WaveEnemiesKilled.ToString();
	}

	#endregion
}



/// <summary>
///		Displays UI relating to the players current kills 
/// </summary>
[System.Serializable]
public class PlayerKillsUI
{

	#region Public Variables 
	/// <summary>
	///		The player kills ui game object container 
	/// </summary>
	public GameObject playerKillsUI;

	/// <summary>
	///		The player kills background image (color) 
	/// </summary>
	public Image BackgroundImage;

	/// <summary>
	///		The total amount of kills text label 
	/// </summary>
	public TMP_Text TotalKillsLabel;

	/// <summary>
	/// The total amount of kills for the player 
	/// </summary>
	public TMP_Text TotalAllTimeKills;

	/// <summary>
	///		Wave kills label text 
	/// </summary>
	public TMP_Text WaveKillsLabel;

	/// <summary>
	///		The current amount of kills this wave text
	/// </summary>
	public TMP_Text WaveKills;

	#endregion

	#region Private Variables 
	/// <summary>
	///		Fire Mode UI Instance Reference 
	/// </summary>
	private FireModeUI m_FireModeUI;

	/// <summary>
	///		The current background image color 
	/// </summary>
	[SerializeField] private Color m_CurrentBackgroundColor;
	#endregion

	#region Public Methods 
	/// <summary>
	///		Sets up the player kills ui 
	/// </summary>
	/// <param name="FireModeUI"></param>
	public void Setup(FireModeUI FireModeUI)
	{
		m_FireModeUI = FireModeUI;

		TotalKillsLabel.GetComponentInChildren<TMP_Text>().text = GameTextUI.PlayerKillsUI_TotallKillsLabel;
		TotalAllTimeKills.GetComponentInChildren<TMP_Text>().text = GameTextUI.PlayerKillsUI_TotalAllTimeKills;
		WaveKills.GetComponentInChildren<TMP_Text>().text = GameTextUI.PlayerKillsUI_WaveKills;
		WaveKillsLabel.GetComponentInChildren<TMP_Text>().text = GameTextUI.PlayerKillsUI_WaveKillsLabel;

		m_CurrentBackgroundColor = BackgroundImage.color;
	}

	/// <summary>
	///		Sets whether the player kills ui should be seen 
	/// </summary>
	/// <param name="ShouldDisplay"></param>
	public void Show(bool ShouldDisplay)
	{
		playerKillsUI.SetActive(ShouldDisplay);
	}

	#endregion
}


/// <summary>
///	 Wave Counter Data Class 
/// </summary>
[System.Serializable]
public class WaveCounterUI
{

	#region Public Variables 
	/// <summary>
	///		The wave counter game object container
	/// </summary>
	public GameObject waveCounterUI;

	/// <summary>
	///		The background image of the wave counter 
	/// </summary>
	public Image BackgroundImage;

	/// <summary>
	///		The current wave ui text label
	/// </summary>
	public TMP_Text currentWaveLabel;

	/// <summary>
	///		The enemies remaining text label 
	/// </summary>
	public TMP_Text enemiesRemainingLabel;

	/// <summary>
	///		The current wave text
	/// </summary>
	public TMP_Text currentWave;

	/// <summary>
	///		The amount of remaining enemies text 
	/// </summary>
	public TMP_Text enemiesRemaining;

	#endregion

	#region Private Variables 
	/// <summary>
	///		The Fire Mode UI Instance Reference 
	/// </summary>
	private FireModeUI m_FireModeUI;
	/// <summary>
	///		The current wave counter ui background color 
	/// </summary>
	[SerializeField] private Color m_CurrentBackgroundColor;

	#endregion

	#region Public Methods 
	public void Setup(FireModeUI FireModeUI)
	{
		m_FireModeUI = FireModeUI;

		m_CurrentBackgroundColor = BackgroundImage.color;

		currentWaveLabel.GetComponentInChildren<TMP_Text>().text = GameTextUI.WaveCounterUI_CurrentWaveLabel;
		enemiesRemainingLabel.GetComponentInChildren<TMP_Text>().text = GameTextUI.WaveCounterUI_EnemiesRemainingLabel;

		currentWave.GetComponentInChildren<TMP_Text>().text = GameTextUI.WaveCounterUI_CurrentWave;
		enemiesRemaining.GetComponentInChildren<TMP_Text>().text = GameTextUI.WaveCounterUI_EnemiesRemaining;
	}


	/// <summary>
	///		Sets whether the WaveCounter should be active
	/// </summary>
	/// <param name="ShouldDisplay"></param>
	public void Show(bool ShouldDisplay)
	{
		waveCounterUI.SetActive(ShouldDisplay);
	}

	#endregion
}
