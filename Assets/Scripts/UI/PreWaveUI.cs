using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class PreWaveUI
{

	private FireModeUI m_FireModeUI;
	public Image BackgroundImage;
	public Image ForgroundImage;
	public TMP_Text PreWaveLabel;
	public TMP_Text PreWaveCounter;


	public void Setup(FireModeUI FireModeUI)
	{
		m_FireModeUI = FireModeUI;

	}
}