using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///		Displays In-Game UI
/// </summary>
[System.Serializable]
public class InGameWaveUI
{

	public PlayerKillsUI m_PlayerKillsUI;
	public WaveCounterUI m_WaveCounterUI;
	private FireModeUI m_FireModeUI;


	public void Setup(FireModeUI FireModeUI)
	{
		m_FireModeUI = FireModeUI;

	}
}



/// <summary>
///		Player Kills Data Class
/// </summary>
[System.Serializable]
public class PlayerKillsUI
{
	public Image BackgroundImage;
	public TMP_Text TotalKillsLabel;
	public TMP_Text TotalAllTimeKills;
	public TMP_Text WaveKillsLabel;
	public TMP_Text WaveKills;
}


/// <summary>
///	 Wave Counter Data Class 
/// </summary>
[System.Serializable]
public class WaveCounterUI
{
	public Image BackgroundImage;
	public TMP_Text CurrentWaveLabel;
	public TMP_Text EnemiesAliveLabel;
	public TMP_Text CurrentWave;
	public TMP_Text EnemiesAlive;
}
