#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#endregion

/// <summary>
///		Fire Mode UI Manager
///		Top level UI Manager for the Game Mode Scene 
/// </summary>
[System.Serializable]
public class FireModeUI : MonoBehaviour
{

	#region Public Variables 
	/// <summary>
	///		UI Instance 
	/// </summary>
	public static FireModeUI Instance;

	/// <summary>
	///		Pre Gave Wave User Interface Data Class 
	/// </summary>
	[Header("User Interfaces")]
	public PreWaveUI PreGameWaveUI; 

	/// <summary>
	///		In Game Wave User Interface Data Class 
	/// </summary>
	public InGameWaveUI InGameWaveUI;

	/// <summary>
	///		Post Wave User Interface Data Class 
	/// </summary>
	public PostWaveUI PostWaveUI; // post wave game ui data class 

	#endregion

	#region Private Variables  

	[SerializeField] private GameObject PreGameUI; //ref 
	[SerializeField] private GameObject InGameUI; // ref 
	[SerializeField] private GameObject PostGameUI; // ref 
	
	#endregion

	#region Unity References   

	#region Event Listeners 

	/// <summary>
	///		Once Enabled, Listen to events 
	/// </summary>
	private void OnEnable()
	{
		FireModeEvents.OnPreWaveEvent += DisplayPreWaveUI;
		FireModeEvents.OnUpdateWaveCountEvent += UpdateWaveCountUI;
		FireModeEvents.OnUpdatePlayerKillsEvent += UpdatePlayerKills;
		FireModeEvents.OnUpdatePlayerAmmunitionEvent += UpdateAmmunitionCount;
		FireModeEvents.OnNextWaveEvent += DisplayPostWaveUI;
	}

	/// <summary>
	///		Remove event listeners
	/// </summary>
	private void OnDisable()
	{
		FireModeEvents.OnPreWaveEvent -= DisplayPreWaveUI;
		FireModeEvents.OnUpdateWaveCountEvent -= UpdateWaveCountUI;
		FireModeEvents.OnUpdatePlayerKillsEvent -= UpdatePlayerKills;
		FireModeEvents.OnUpdatePlayerAmmunitionEvent -= UpdateAmmunitionCount;
		FireModeEvents.OnNextWaveEvent -= DisplayPostWaveUI;
	}

	#endregion


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

	private void Start()
	{
		// Nothing
	}

	#endregion

	#region Showing / Hiding UI 

	/// <summary>
	///		Displays UI Pre Game 
	/// </summary>
	public void DisplayPreWaveUI()
	{
		Debug.Log("[FireModeUI.DisplayPreWaveUI]: " + "Displaying Pre Wave UI!");
	}

	/// <summary>
	///		Event called once the round has completed
	/// </summary>
	public void DisplayPostWaveUI()
	{
		Debug.Log("[FireModeUI.DisplayPostWaveUI]: " + "Displaying Post Wave UI!");
	}

	#endregion

	#region Update UI Private Methods  

	/// <summary>
	///		Displays the In-Game UI with the wave! 
	/// </summary>
	private void UpdateWaveCountUI(int Wave)
	{
		Debug.Log("[FireModeUI.UpdateWaveCountUI]: " + "Updating Wave Count UI!");
	}

	/// <summary>
	///		Updates the current player kills ui 
	/// </summary>
	/// <param name="KillCount"></param>
	private void UpdatePlayerKills(int KillCount)
	{
		Debug.Log("[FireModeUI.UpdatePlayerKills]: " + "Updating the current players kills!");
	}

	/// <summary>
	///		Updates the player's current ammunition count! 
	/// </summary>
	/// <param name="AmmunitionLoaded"></param>
	/// <param name="AmmunitionRemaining"></param>
	private void UpdateAmmunitionCount(int AmmunitionLoaded, int AmmunitionRemaining)
	{
		Debug.Log("[FireModeUI.UpdatePlayerAmmunition]: " + "Updating current players ammunition Count!");
	}

	#endregion
}

