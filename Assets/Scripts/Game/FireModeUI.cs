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
	public PreWaveUI preGameWaveUI; 

	/// <summary>
	///		In Game Wave User Interface Data Class 
	/// </summary>
	public InGameWaveUI inGameWaveUI;

	/// <summary>
	///		Post Wave User Interface Data Class 
	/// </summary>
	public PostWaveUI postWaveUI; 

	/// <summary>
	///		On Screen Heads Up Display UI
	/// </summary>
	public OnScreenUI onScreenUI;

	#endregion

	#region Private Variables 
	private bool shouldStartPreWaveCountdown = false;
	private float m_PreWaveCountdownSeconds;
	#endregion

	#region Unity References   

	#region Event Listeners 

	/// <summary>
	///		Once Enabled, Listen to events 
	/// </summary>
	private void OnEnable()
	{
		// Wave Events 
		FireModeEvents.OnPreWaveEvent += DisplayPreWaveUI;
		FireModeEvents.OnWaveStartedEvent += DisplayInGameUI;
		FireModeEvents.OnNextWaveEvent += DisplayNextWaveUI;


		//	Updating Wave, Player & HUD Events 
		FireModeEvents.SpawnPlayerEvent += DisplayOnScreenUI;
		FireModeEvents.UpdateWaveUIEvent += UpdateWaveUI;
		FireModeEvents.UpdatePlayerKillsEvent += UpdatePlayerKillsUI;
		FireModeEvents.UpdatePlayerAmmunitionEvent += UpdateAmmunitionCounterUI;
		FireModeEvents.UpdatePlayerHealthEvent += UpdateHealthUI;
	}

	/// <summary>
	///		Remove event listeners
	/// </summary>
	private void OnDisable()
	{
		// Wave Events 
		FireModeEvents.OnPreWaveEvent -= DisplayPreWaveUI;
		FireModeEvents.OnWaveStartedEvent -= DisplayInGameUI;
		FireModeEvents.OnNextWaveEvent -= DisplayNextWaveUI;

		FireModeEvents.SpawnPlayerEvent -= DisplayOnScreenUI;
		FireModeEvents.UpdateWaveUIEvent -= UpdateWaveUI;
		FireModeEvents.UpdatePlayerKillsEvent -= UpdatePlayerKillsUI;
		FireModeEvents.UpdatePlayerAmmunitionEvent -= UpdateAmmunitionCounterUI;
		FireModeEvents.UpdatePlayerHealthEvent -= UpdateHealthUI;
	}

	#endregion


	/// <summary>
	///		Creates the FireModeUI Instance 
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
	///		We should setup the UI screens on the games start - Called before the first frame update!
	/// </summary>
	private void Start()
	{
		Debug.Log("[FireModeUI.Start]: " + "Setting up UI screens...");
		preGameWaveUI.Setup(Instance);
		inGameWaveUI.Setup(Instance);
		postWaveUI.Setup(Instance);
		onScreenUI.Setup(Instance);
	}

	private void Update()
	{
	

		if (shouldStartPreWaveCountdown == true)
		{
			m_PreWaveCountdownSeconds -= 1 * Time.deltaTime;

			preGameWaveUI.SetPreGameWaveUI(m_PreWaveCountdownSeconds);
		}

	}

	#endregion

	#region Showing / Hiding UI 

	#region Pre Wave UI 
	/// <summary>
	///		Displays the Pre Game Wave UI
	/// </summary>
	/// <param name="displaySeconds">The amount of seconds to display the ui for</param>
	/// <returns></returns>
	private void DisplayPreWaveUI(float displaySeconds)
	{
		Debug.Log("[FireModeUI.DisplayPreWaveUI]: " + "Displaying Pre Wave Game UI for " + displaySeconds);
		StartCoroutine(ShowPreWaveUI(displaySeconds));
	}

	/// <summary>
	///		Toggles the pre wave ui 
	/// </summary>
	/// <param name="SecondsToDisplayFor">The amount of seconds to display the ui for</param>
	/// <returns></returns>
	private IEnumerator ShowPreWaveUI(float Seconds)
	{
		m_PreWaveCountdownSeconds = Seconds;
		//	 Start the pre wave countdown 	
		shouldStartPreWaveCountdown = true;

		Debug.Log("[FireModeUI.ShowPreWaveUI]: " + "Showing screen!");
		// Display the pre game wave ui 
		preGameWaveUI.ShowScreen(true);

		

		// Wait x amount of seconds 
		yield return new WaitForSeconds(Seconds);

		// Set the screen to off 
		preGameWaveUI.ShowScreen(false);

		// Stop counting down 
		shouldStartPreWaveCountdown = false;

		m_PreWaveCountdownSeconds = Seconds; // Reset the seconds to default. (or zero) 

		Debug.Log("[FireModeUI.ShowPreWaveUI]: " + "Stopped showing screen!");
		yield return null;
	}

	#endregion

	#region Next Wave UI 
	/// <summary>
	///		Event called once the wave has been completed - Displays the next wave! 
	/// </summary>
	private void DisplayNextWaveUI()
	{
		Debug.Log("[FireModeUI.DisplayPostWaveUI]: " + "Displaying Post Wave UI!");
		postWaveUI.ShowScreen(true);
	}
	#endregion

	#region In Game UI 
	
	/// <summary>
	///		Displays the In-Game UI with the wave! 
	/// </summary>
	private void DisplayInGameUI()
	{
		Debug.Log("[FireModeUI.DisplayInGameUI]: " + "Displaying in game UI!");
		inGameWaveUI.ShowScreen(true);
	}

	/// <summary>
	///		Displays the on screen ui 
	/// </summary>
	private void DisplayOnScreenUI()
	{
		Debug.Log("[FireModeUI.DisplayOnScreenUI]: " + "Displaying On Screen UI!");
		onScreenUI.ShowScreen(true);
	}

	#endregion

	#endregion

	#region Update UI Private Methods  

	/// <summary>
	///		Updates the current player kills ui 
	/// </summary>
	/// <param name="KillCount"></param>
	private void UpdatePlayerKillsUI(int Kills)
	{
		Debug.Log("[FireModeUI.UpdatePlayerKills]: " + "Updating the current players kills " + Kills);
	
		inGameWaveUI.SetTotalPlayerKills(Kills);
	}

	/// <summary>
	///		Updates the player's current ammunition count! 
	/// </summary>
	/// <param name="AmmunitionLoaded"></param>
	/// <param name="AmmunitionRemaining"></param>
	private void UpdateAmmunitionCounterUI(int AmmunitionLoaded, int AmmunitionTotal)
	{
		Debug.Log("[FireModeUI.UpdatePlayerAmmunition]: " + "Updating current players ammunition Count!");
		onScreenUI.Ammunition.SetAmmunition(AmmunitionLoaded, AmmunitionTotal);
	}

	/// <summary>
	///		Updates the in game wave UI 
	/// </summary>
	private void UpdateWaveUI(int NextWave, int WaveEnemiesRemaining, int WaveEnemiesKilled)
	{
		Debug.Log("[FireModeUI.UpdateWaveUI]: " + "Updating Wave UI... Next Wave: " + NextWave + " Enemies Remaining: " + WaveEnemiesRemaining + "Wave Enemies Killed: " + WaveEnemiesKilled);
		inGameWaveUI.SetWave(NextWave, WaveEnemiesRemaining, WaveEnemiesKilled);
	}

	/// <summary>
	///		Updates the players Health UI 
	/// </summary>
	private void UpdateHealthUI(float CurrentHealth)
	{
		onScreenUI.Health.UpdateHealth(CurrentHealth);
	}

	#endregion

}

