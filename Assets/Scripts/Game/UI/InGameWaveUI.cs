using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


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


[System.Serializable]
public class PlayerKillsUI
{ 
	public TMP_Text Title;
	public TMP_Text KillCountText;
	public TMP_Text EnemiesRemainingText;
	public Image BackgroundImage;



}

[System.Serializable]
public class WaveCounterUI
{
	public TMP_Text Title;
	public TMP_Text CurrentWave;
	public TMP_Text TotalEnemies;
	public Image BackgroundImage;



}
