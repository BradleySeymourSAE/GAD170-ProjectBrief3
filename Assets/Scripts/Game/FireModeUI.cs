using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///		Game UI Manager
/// </summary>
[System.Serializable]
public class FireModeUI : MonoBehaviour
{
	/// <summary>
	///		UI Instance 
	/// </summary>
	public static FireModeUI Instance;

	private const string styled = "---";

	[Header(styled + " UI Containers " + styled)]
	public PreWaveUI PreGameWaveUI;
	public InGameWaveUI InGameWaveUI;
	public PostWaveUI PostWaveUI;


	/// <summary>
	///		Once Enabled, Listen to events 
	/// </summary>
	private void OnEnable()
	{
		FireModeEvents.OnPreWaveEvent += DisplayPreWaveUI;
		FireModeEvents.OnUpdateWaveCountEvent += UpdateWaveCountUI;
		FireModeEvents.OnUpdatePlayerKillsEvent += UpdatePlayerKills;
		FireModeEvents.OnWaveOverEvent += DisplayPostWaveUI;
	}

	/// <summary>
	///		Remove event listeners
	/// </summary>
	private void OnDisable()
	{
		FireModeEvents.OnPreWaveEvent -= DisplayPreWaveUI;
		FireModeEvents.OnUpdateWaveCountEvent -= UpdateWaveCountUI;
		FireModeEvents.OnUpdatePlayerKillsEvent -= UpdatePlayerKills;
		FireModeEvents.OnWaveOverEvent -= DisplayPostWaveUI;
	}

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}


	/// <summary>
	///		Displays UI Pre Game 
	/// </summary>
	private void DisplayPreWaveUI()
	{

	}

	/// <summary>
	///		Displays the In-Game UI
	/// </summary>
	private void UpdateWaveCountUI(int Wave)
	{

	}

	/// <summary>
	///		Event called once the round has completed
	/// </summary>
	private void DisplayPostWaveUI()
	{

	}

	private void UpdatePlayerKills(int KillCount)
	{

	}
}

