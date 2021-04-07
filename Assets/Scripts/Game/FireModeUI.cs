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
	public static FireModeUI Instance; // the game mode ui instance 

	[Header("User Interfaces")]
	public PreWaveUI PreGameWaveUI; // pre game ui data class  
	public InGameWaveUI InGameWaveUI; // in game wave ui data class 
	public PostWaveUI PostWaveUI; // post wave game ui data class 

	[SerializeField] private GameObject PreGameUI; //ref 
	[SerializeField] private GameObject InGameUI; // ref 
	[SerializeField] private GameObject PostGameUI; // ref 


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

	/// <summary>
	///		Creates a persistant instance across scenes 
	/// </summary>
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

