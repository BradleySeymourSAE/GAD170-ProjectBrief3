using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[System.Serializable]
public class PostWaveUI
{
	public Image BackgroundImage;
	public Image ForegroundImage;
	public TMP_Text NextRoundStartingLabel;
	public TMP_Text NextRoundStarting;

	public TMP_Text WaveLabel;
	public TMP_Text Wave;
	public TMP_Text NextWaveLabel;
	public TMP_Text NextWave;

	private FireModeUI m_FireModeUI;

	public void Setup(FireModeUI FireModeUI)
	{
		m_FireModeUI = FireModeUI;
	}
}