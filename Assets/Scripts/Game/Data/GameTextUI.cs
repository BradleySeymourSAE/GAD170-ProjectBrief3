using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		Game Text UI Instance 
/// </summary>
public static class GameTextUI 
{

	#region Field of Fire - Starting Scene
	
	#region Main Menu UI Text Elements 

	/// <summary>
	///		Main Menu UI Elements
	/// </summary>
	public static string MainMenu_GameTitle = "Field of Fire";
	public static string MainMenu_GameSubtitle = "Ready to do the pew pew?";
	public static string MainMenu_GameVersion = "Development - v1.0.0";
	public static string MainMenu_Start = "Start Game";
	public static string MainMenu_Credits = "Credits";
	public static string MainMenu_Quit = "Leave";
	#endregion

	#region Credits Menu UI Text Elements

	/// <summary>
	///		Credits Menu UI Elements 
	/// </summary>
	public static string CreditsMenu_GameTitle = "Field of Fire - Credits";
	public static string CreditsMenu_DevelopmentTeamTitle = "Development Team";
	public static string CreditsMenu_DevelopmentTeamRole = "Project Lead";

	public static string CreditsMenu_SpecialThanksTitle = "Special Thanks";
	public static string CreditsMenu_SpecialThanksMention_DICE = "Battlefield 4 Models - Dice / EA";
	public static string CreditsMenu_Developer = "Bradley Seymour";
	public static string CreditsMenu_Exit = "Back to main menu";
	#endregion

	#endregion

	#region Field of Fire - Game Scene Level 01 

	#region Pre Wave UI 
	/// <summary>
	///		Pre Game Wave Menu UI  
	/// </summary>
	public const string PreWaveUI_PreWaveLabel = "Wave starting in...";
	public const string PreWaveUI_PreWaveCounterText = "15";

	#endregion

	#region In Game Wave UI 


	#region In Game Wave UI - Wave Counter Menu UI 
	/// <summary>
	///	 Wave Counter Menu UI Text 
	/// </summary>
	public const string WaveCounterUI_CurrentWaveLabel = "Current Wave:";
	public const string WaveCounterUI_CurrentWave = "1";
	public const string WaveCounterUI_EnemiesRemainingLabel = "Enemies Remaining:";
	public const string WaveCounterUI_EnemiesRemaining = "0";
	#endregion

	#endregion

	#region Post Wave UI 
	
	/// <summary>
	///		Post Wave UI Menu - Text
	/// </summary>
	public const string PostWaveUI_NextRoundStartingLabel = "Next Wave Starting in ...";
	public const string PostWaveUI_NextRoundStarting = "3";
	public const string PostWaveUI_WaveLabel = "Wave:";
	public const string PostWaveUI_WaveText = "1";
	public const string PostWaveUI_NextWaveLabel = "Next Wave:";
	public const string PostWaveUI_NextWaveText = "2";

	#endregion

	#region Field Of Fire - On Screen UI 
	public const string OnScreen_Ammunition = "20";
	public const string OnScreen_Health = "100";


	#endregion

	#endregion

}
