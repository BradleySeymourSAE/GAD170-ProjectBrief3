#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#endregion

/// <summary>
///		Pre Game Wave Data Class 
/// </summary>
[System.Serializable]
public class PreWaveUI
{
	#region Public Variables 
		/// <summary>
		///		The pre wave ui game object container 
		/// </summary>
		public GameObject preWaveUIContainer;

		/// <summary>
		///		The Background Image 
		/// </summary>
		public Image backgroundImage;

		/// <summary>
		///		The Foreground Image 
		/// </summary>
		public Image foregroundImage;

		/// <summary>
		///		Pre Wave Label Text
		/// </summary>
		public TMP_Text preWaveLabel;

		/// <summary>
		///		Pre Wave Counter Text
		/// </summary>
		public TMP_Text preWaveCounterText;

	#endregion

	#region Private Variables 
	/// <summary>
	///		Fire Mode UI Instance - Helpful for displaying other ui menu's from local methods 
	/// </summary>
	private FireModeUI m_FireModeUI;

	/// <summary>
	///		The background image color 
	/// </summary>
	private Color m_BackgroundColor;

	/// <summary>
	///		The foreground image color 
	/// </summary>
	private Color m_ForegroundColor;

	#endregion

	#region Public Methods 
	public void Setup(FireModeUI FireModeUI)
	{
		m_FireModeUI = FireModeUI;

		m_BackgroundColor = backgroundImage.color;
		m_ForegroundColor = foregroundImage.color;

		// Set the text for the text
		preWaveLabel.GetComponentInChildren<TMP_Text>().text = GameTextUI.PreWaveUI_PreWaveLabel;
		preWaveCounterText.GetComponentInChildren<TMP_Text>().text = GameTextUI.PreWaveUI_PreWaveCounterText;
	}

	/// <summary>
	///		Displays the pre game ui 
	/// </summary>
	/// <param name="ShouldDisplayScreen"></param>
	public void ShowScreen(bool ShouldDisplayScreen)
	{
		preWaveUIContainer.SetActive(ShouldDisplayScreen);
	}

	/// <summary>
	///		Set pre game countdown timer 
	/// </summary>
	/// <param name="Seconds"></param>
	public void SetPreGameTimer(float Seconds)
	{
		preWaveCounterText.GetComponentInChildren<TMP_Text>().text = Seconds.ToString("0");
	}
	#endregion
}