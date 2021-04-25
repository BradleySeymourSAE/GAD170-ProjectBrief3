#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#endregion


[System.Serializable]
public class NextWaveUI
{
	#region Public Variables 
	public GameObject postWaveUIContainer;
	public Image backgroundImage;
	public Image foregroundImage;
	public TMP_Text nextRoundStartingLabel;
	public TMP_Text nextRoundStarting;

	public TMP_Text waveLabel;
	public TMP_Text waveText;
	public TMP_Text nextWaveLabel;
	public TMP_Text nextWaveText;
	#endregion

	#region Private Variables
	
	/// <summary>
	///  Reference to the fire mode instance 
	/// </summary>
	private FireModeUI m_FireModeUI;

	#endregion

	#region Public Methods 
	
	/// <summary>
	///		Instantiates the UI fields with some default values 
	/// </summary>
	/// <param name="FireModeUI"></param>
	public void Setup(FireModeUI FireModeUI)
	{
		m_FireModeUI = FireModeUI;
		
		nextRoundStartingLabel.GetComponentInChildren<TMP_Text>().text = GameTextUI.PostWaveUI_NextRoundStartingLabel;
		nextRoundStarting.GetComponentInChildren<TMP_Text>().text = GameTextUI.PostWaveUI_NextRoundStarting;
		
		waveLabel.GetComponentInChildren<TMP_Text>().text = GameTextUI.PostWaveUI_WaveLabel;
		waveText.GetComponentInChildren<TMP_Text>().text = GameTextUI.PostWaveUI_WaveText;
	
		nextWaveLabel.GetComponentInChildren<TMP_Text>().text = GameTextUI.PostWaveUI_NextWaveLabel;
		nextWaveText.GetComponentInChildren<TMP_Text>().text=  GameTextUI.PostWaveUI_NextWaveText;
	}


	/// <summary>
	///		Displays whether to show the Post Wave UI 
	/// </summary>
	/// <param name="ShouldDisplay"></param>
	public void ShowScreen(bool ShouldDisplay)
	{
		postWaveUIContainer.SetActive(ShouldDisplay);
	}


	/// <summary>
	///		Sets the next round starting timer text field and formats it to one decimal place 
	/// </summary>
	/// <param name="timeInSeconds"></param>
	public void SetNextWaveTimer(float timeInSeconds)
	{ 
		nextRoundStarting.GetComponentInChildren<TMP_Text>().text = timeInSeconds.ToString("0");
	}

	public void SetNextWave(int NextWaveIndex) => nextWaveText.GetComponentInChildren<TMP_Text>().text = NextWaveIndex.ToString("0");

	public void SetCurrentWave(int CurrentWaveIndex) => waveText.GetComponentInChildren<TMP_Text>().text = (CurrentWaveIndex - 1).ToString("0");
	
	#endregion

}