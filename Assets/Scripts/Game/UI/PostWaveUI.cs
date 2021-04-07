using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[System.Serializable]
public class PostWaveUI
{

	public TMP_Text Title;
	public TMP_Text TotalKillsText;
	public Image BackgroundImage;

	private FireModeUI m_FireModeUI;

	public void Setup(FireModeUI FireModeUI)
	{
		m_FireModeUI = FireModeUI;
	}
}