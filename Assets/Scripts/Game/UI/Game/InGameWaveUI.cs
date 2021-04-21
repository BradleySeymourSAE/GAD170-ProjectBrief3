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
	/// <summary>
	///		The In Game Wave UI Parent Game Object 
	/// </summary>
	public GameObject inGameWaveUI;

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

		waveCounterUI.Setup(m_FireModeUI);
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

	#endregion

	#region Public Methods 
	public void Setup(FireModeUI FireModeUI)
	{
		m_FireModeUI = FireModeUI;

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



	/// <summary>
	///		Sets the current enemies remaining 
	/// </summary>
	/// <param name="Remaining"></param>
	public void SetEnemiesRemaining(int Remaining) => enemiesRemaining.GetComponentInChildren<TMP_Text>().text = Remaining.ToString("0");

	/// <summary>
	///		Sets the current wave index in game 
	/// </summary>
	/// <param name="CurrentWaveIndex"></param>
	public void SetCurrentWaveInGame(int CurrentWaveIndex) => currentWave.GetComponentInChildren<TMP_Text>().text = CurrentWaveIndex.ToString("0");
	#endregion
}
