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
	///		Pre Gave Wave User Interface Data Class 
	/// </summary>
	public PreWaveUI preGameWaveUI; 

	/// <summary>
	///		In Game Wave User Interface Data Class 
	/// </summary>
	public InGameWaveUI inGameWaveUI;

	/// <summary>
	///		Post Wave User Interface Data Class 
	/// </summary>
	public NextWaveUI nextWaveUI; 

	/// <summary>
	///		On Screen Heads Up Display UI
	/// </summary>
	public OnScreenUI onScreenUI;

	#endregion

	#region Private Variables 
	
	/// <summary>
	///		Should we start the pre wave countdown timer? 
	/// </summary>
	private bool shouldStartPreWaveCountdown = false;
	
	/// <summary>
	///		Float for storing the pre wave countdown 
	/// </summary>
	private float m_PreWaveCountdownTimer;

	/// <summary>
	///		Should we star the next wave countdown timer? 
	/// </summary>
	private bool shouldStartNextWaveCountdown = false;
	
	/// <summary>
	///		Float for storing the next wave countdown timer 
	/// </summary>
	private float m_NextWaveCountdownTimer;

	/// <summary>
	///		 Reference to the Game Manager Script 
	/// </summary>
	private FireModeGameManager m_GameManager;
	
	#endregion

	#region Unity References   

	/// <summary>
	///		Once Enabled, Listen to events 
	/// </summary>
	private void OnEnable()
	{
		// Wave Events 
		FireModeEvents.PreGameStartedEvent += DisplayPreWaveUI;
		FireModeEvents.GameStartedEvent += DisplayInGameUI;
		FireModeEvents.HandleNextWaveStarted += DisplayNextWaveUI;
		FireModeEvents.SpawnPlayerEvent += DisplayOnScreenUI;


		FireModeEvents.IncreasePlayerHealthEventUI += SetPlayerHealthUI;
		FireModeEvents.IncreaseEnemiesRemainingEventUI += SetEnemiesRemainingUI;
		FireModeEvents.IncreaseAmmunitionEventUI += SetPlayerAmmunitionUI; 
		FireModeEvents.IncreaseWaveEventUI += SetNextWaveUI; 
	}

	/// <summary>
	///		Remove event listeners
	/// </summary>
	private void OnDisable()
	{
		// Wave Events 
		FireModeEvents.PreGameStartedEvent -= DisplayPreWaveUI;
		FireModeEvents.GameStartedEvent -= DisplayInGameUI;
		FireModeEvents.HandleNextWaveStarted -= DisplayNextWaveUI;
		FireModeEvents.SpawnPlayerEvent -= DisplayOnScreenUI;

		FireModeEvents.IncreasePlayerHealthEventUI -= SetPlayerHealthUI;
		FireModeEvents.IncreaseEnemiesRemainingEventUI -= SetEnemiesRemainingUI;
		FireModeEvents.IncreaseAmmunitionEventUI -= SetPlayerAmmunitionUI; // Working 
		FireModeEvents.IncreaseWaveEventUI -= SetNextWaveUI; // On and off 
	}

	/// <summary>
	///		We should setup the UI screens on the games start - Called before the first frame update!
	/// </summary>
	private void Start()
	{
		Debug.Log("[FireModeUI.Start]: " + "Setting up UI using Fire Mode UI Reference");
		preGameWaveUI.Setup(this);
		inGameWaveUI.Setup(this);
		nextWaveUI.Setup(this);
		onScreenUI.Setup(this);

		if (FindObjectOfType<FireModeGameManager>())
		{
			m_GameManager = FindObjectOfType<FireModeGameManager>();
		}


		// As this is the first function to be called once the application begins, we should be fading the audio to a lower volume 

		// Grab the audio manager instance 
		if (AudioManager.Instance)
		{
			// Fade the volume down to a lower level as overall it's quite loud upon start 
			// Grab the audio sound by its string name "Background Audio", set the volume to 0.1 over the duration of 
			// one and a half seconds 
			AudioSource s_Source = AudioManager.Instance.GetAudioSource(GameAudio.BackgroundThemeTrack);

			if (s_Source.isPlaying == true)
			{
				StartCoroutine(StartFade(s_Source, 4f, 0.15f));
				
			}
			else
			{
				Debug.LogWarning("[FireModeUI.Start]: " + "Could not find a background theme track audio source that's currently playing!");
			}			
		}
		else
		{
			Debug.LogWarning("[FireModeUI.Start]: " + "Could not find audio manager instance to fade backing track out!");
		}


	}

	private void Update()
	{
	

		if (shouldStartPreWaveCountdown == true)
		{
			m_PreWaveCountdownTimer -= 1 * Time.deltaTime;

			preGameWaveUI.SetPreGameTimer(m_PreWaveCountdownTimer);
		}


		if (shouldStartNextWaveCountdown)
		{
			m_NextWaveCountdownTimer -= 1 * Time.deltaTime;

			nextWaveUI.SetNextWaveTimer(m_NextWaveCountdownTimer);
		}
	}

	#endregion

	#region Public Methods

	public IEnumerator StartFade(AudioSource source, float duration, float targetVolume)
	{
		float s_CurrentTime = 0;
		float start = source.volume;

		while (s_CurrentTime < duration)
		{
			s_CurrentTime += Time.deltaTime;
			source.volume = Mathf.Lerp(start, targetVolume, s_CurrentTime / duration);

			yield return null;
		}

		yield break;
	}

	/// <summary>
	///		Sets the next wave index 
	/// </summary>
	/// <param name="NextWaveIndex"></param>
	private void SetNextWaveUI(int NextWaveIndex) => nextWaveUI.SetNextWave(NextWaveIndex);

	/// <summary>
	///		Sets the players ammunition UI! 
	/// </summary>
	/// <param name="Rounds"></param>
	private void SetPlayerAmmunitionUI(int Rounds) => onScreenUI.Ammunition.SetAmmunition(Rounds);

	/// <summary>
	///		Sets the current enemies remaining in the current wave
	/// </summary>
	/// <param name="EnemiesRemaining"></param>
	private void SetEnemiesRemainingUI(int EnemiesRemaining) => inGameWaveUI.waveCounterUI.SetEnemiesRemaining(EnemiesRemaining);

	private void SetPlayerHealthUI(float Health) => onScreenUI.Health.SetHealthBarSlider(Health, 100);
	
	#endregion

	#region Private Methods 

	/// <summary>
	///		Displays the Pre Game Wave UI
	/// </summary>
	/// <param name="displaySeconds">The amount of seconds to display the ui for</param>
	/// <returns></returns>
	private void DisplayPreWaveUI()
	{
		Debug.Log("[FireModeUI.DisplayPreWaveUI]: " + "Display Pre Wave UI - Starting Pre Wave Countdown timer!");
		StartCoroutine(ShowPreWaveCountdownTimer());
	}

	/// <summary>
	///		Starts IEnumerator for showing the pre wave ui coundown 
	/// </summary>
	/// <param name="Seconds"></param>
	/// <returns></returns>
	private IEnumerator ShowPreWaveCountdownTimer()
	{
		// Set the pre wave countdown seconds 
		m_PreWaveCountdownTimer = m_GameManager.preWaveSetupTimer;
		//	 Start the pre wave countdown 	
		shouldStartPreWaveCountdown = true;

		Debug.Log("[FireModeUI.ShowPreWaveCountdownTimer]: " + "DISPLAY PRE WAVE UI FOR " + m_GameManager.preWaveSetupTimer + " seconds");
		// Display the pre game wave ui 
		preGameWaveUI.ShowScreen(true);

		// Wait pre wave countdown amount of seconds 
		yield return new WaitForSeconds(m_GameManager.preWaveSetupTimer);

		// Set the pre game wave ui screen off 
		preGameWaveUI.ShowScreen(false);

		// Stop counting down 
		shouldStartPreWaveCountdown = false;

		// reset the wave countdown seconds 
		m_PreWaveCountdownTimer = m_GameManager.preWaveSetupTimer; // Reset the seconds to default. (or zero) 

		Debug.Log("[FireModeUI.ShowPreWaveUI]: " + "STOPPED DISPLAYING PRE WAVE UI!");
		yield return null;
	}

	/// <summary>
	///		Event called once the wave has been completed - Displays the next wave! 
	/// </summary>
	private void DisplayNextWaveUI()
	{
		Debug.Log("[FireModeUI.DisplayPostWaveUI]: " + "Displaying Post Wave UI!");
		StartCoroutine(ShowNextWaveCountdownTimer());
	}

	private IEnumerator ShowNextWaveCountdownTimer()
	{
		// Set the Next Wave Countdown timer seconds 
		m_NextWaveCountdownTimer = m_GameManager.nextWaveStartTimer;

		// Start the next wave countdown 

		shouldStartNextWaveCountdown = true;

		Debug.Log("[FireModeUI.ShowNextWaveUI]: " + "DISPLAYING NEXT WAVE UI FOR " + m_GameManager.nextWaveStartTimer + " seconds");

		//	 Show the next wave UI 
		nextWaveUI.ShowScreen(true);

		// Wait X next wave countdown seconds 
		yield return new WaitForSeconds(m_GameManager.nextWaveStartTimer);

		// Stop displaying the next wave UI 
		nextWaveUI.ShowScreen(false);

		// Stop counting down 
		shouldStartNextWaveCountdown = false;

		// Reset the next wave countdown timer 
		m_NextWaveCountdownTimer = m_GameManager.nextWaveStartTimer;

		yield return null;
	}	

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

}

