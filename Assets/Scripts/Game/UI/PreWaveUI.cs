using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class PreWaveUI
{

	private FireModeUI m_FireModeUI;
	public TMP_Text Title;
	public TMP_Text RoundStartTimer;



	public void Setup(FireModeUI FireModeUI)
	{
		m_FireModeUI = FireModeUI;

	}
}