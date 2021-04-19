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
	public PostWaveUI postWaveUI; 

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


		//	Updating Wave, Player & HUD Events 
		FireModeEvents.SpawnPlayerEvent += DisplayOnScreenUI;
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
	}

	/// <summary>
	///		We should setup the UI screens on the games start - Called before the first frame update!
	/// </summary>
	private void Start()
	{
		Debug.Log("[FireModeUI.Start]: " + "Setting up UI using Fire Mode UI Reference");
		preGameWaveUI.Setup(this);
		inGameWaveUI.Setup(this);
		postWaveUI.Setup(this);
		onScreenUI.Setup(this);


		// As this is the first function to be called once the application begins, we should be fading the audio to a lower volume 

		// Grab the audio manager instance 
		if (AudioManager.Instance)
		{
			// Fade the volume down to a lower level as overall it's quite loud upon start 
			// Grab the audio sound by its string name "Background Audio", set the volume to 0.1 over the duration of 
			// one and a half seconds 
			AudioSource s = AudioManager.Instance.GetAudioSource(GameAudio.BackgroundThemeTrack);

			if (s.isPlaying == true)
			{
				StartCoroutine(StartFade(s, 4f, 0.15f));
				
			}
			else
			{
				Debug.LogWarning("Could not find a background theme track audio source!");
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

			preGameWaveUI.SetPreGameWaveUI(m_PreWaveCountdownTimer);
		}


		if (shouldStartNextWaveCountdown)
		{
			m_NextWaveCountdownTimer -= 1 * Time.deltaTime;

			postWaveUI.nextRoundStarting.GetComponentInChildren<TMP_Text>().text = m_NextWaveCountdownTimer.ToString("0");
		}

	}

	#endregion

	#region Public Methods

	public IEnumerator StartFade(AudioSource source, float durationTime, float target)
	{
		float s_CurrentTime = 0;
		float start = source.volume;

		while (s_CurrentTime < durationTime)
		{
			s_CurrentTime += Time.deltaTime;
			source.volume = Mathf.Lerp(start, target, s_CurrentTime / durationTime);

			yield return null;
		}

		yield break;
	}

	#endregion

	#region Private Methods 

	/// <summary>
	///		Displays the Pre Game Wave UI
	/// </summary>
	/// <param name="displaySeconds">The amount of seconds to display the ui for</param>
	/// <returns></returns>
	private void DisplayPreWaveUI()
	{
		float displaySeconds = FindObjectOfType<FireModeGameManager>().preWaveSetupTimer;


		Debug.Log("[FireModeUI.DisplayPreWaveUI]: " + "Displaying Pre Wave Game UI for " + displaySeconds);
		StartCoroutine(ShowPreWaveCountdownTimer(displaySeconds));
	}

	/// <summary>
	///		Starts IEnumerator for showing the pre wave ui coundown 
	/// </summary>
	/// <param name="Seconds"></param>
	/// <returns></returns>
	private IEnumerator ShowPreWaveCountdownTimer(float Seconds)
	{
		// Set the pre wave countdown seconds 
		m_PreWaveCountdownTimer = Seconds;
		//	 Start the pre wave countdown 	
		shouldStartPreWaveCountdown = true;

		Debug.Log("[FireModeUI.ShowPreWaveUI]: " + "Showing screen!");
		// Display the pre game wave ui 
		preGameWaveUI.ShowScreen(true);

		// Wait pre wave countdown amount of seconds 
		yield return new WaitForSeconds(Seconds);

		// Set the pre game wave ui screen off 
		preGameWaveUI.ShowScreen(false);

		// Stop counting down 
		shouldStartPreWaveCountdown = false;

		// reset the wave countdown seconds 
		m_PreWaveCountdownTimer = Seconds; // Reset the seconds to default. (or zero) 

		Debug.Log("[FireModeUI.ShowPreWaveUI]: " + "Stopped showing screen!");
		yield return null;
	}

	/// <summary>
	///		Event called once the wave has been completed - Displays the next wave! 
	/// </summary>
	private void DisplayNextWaveUI()
	{
		Debug.Log("[FireModeUI.DisplayPostWaveUI]: " + "Displaying Post Wave UI!");
		postWaveUI.ShowScreen(true);
	}

	/// <summary>
	///		Displays the In-Game UI with the wave! 
	/// </summary>
	private void DisplayInGameUI()
	{
		Debug.Log("[FireModeUI.DisplayInGameUI]: " + "Displaying in game UI!");
		inGameWaveUI.Show(true);
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

