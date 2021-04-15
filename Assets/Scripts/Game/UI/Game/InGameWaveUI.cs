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
	public void Show(bool ShouldDisplay)
	{
		inGameWaveUI.SetActive(ShouldDisplay);
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
