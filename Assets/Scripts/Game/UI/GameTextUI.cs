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
	public static string MainMenu_GameVersion = "Development - Version 1.0.0-Alpha";
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
	public const string PreGameWaveUI_PreWaveLabel = "Wave starting in...";
	public const string PreGaemWaveUI_PreWaveCounter = "3";

	#endregion

	#region In Game Wave UI 

	#region In Game Wave UI - Player Kills 
	/// <summary>
	///		In Game Wave UI - Player Kills Menu 
	/// </summary>
	public const string InGameWaveUI_PlayerKills_TotalKillsLabel = "Total Kills:";
	public const string InGameWaveUI_PlayerKills_TotalAllTimeKills = "20";
	public const string InGameWaveUI_PlayerKills_WaveKillsLabel = "Kills:";
	public const string InGameWaveUI_PlayerKills_WaveKills = "2";

	#endregion


	#region In Game Wave UI - Wave Counter Menu UI 
	/// <summary>
	///	 Wave Counter Menu UI Text 
	/// </summary>
	public const string InGameWaveUI_WaveCounter_CurrentWaveLabel = "Current Wave:";
	public const string InGameWaveUI_WaveCounter_CurrentWave = "1";
	public const string InGameWaveUI_WaveCounter_EnemiesAliveLabel = "Enemies Remaining:";
	public const string InGameWaveUI_WaveCounter_EnemiesAlive = "0";
	#endregion

	#endregion

	#region Post Wave UI 
	
	/// <summary>
	///		Post Wave UI Menu - Text
	/// </summary>
	public const string PostWaveUI_NextRoundStartingLabel = "Next Round Starting in ...";
	public const string PostWaveUI_NextRoundStarting = "3";
	public const string PostWaveUI_WaveLabel = "Wave:";
	public const string PostWaveUI_Wave = "1";
	public const string PostWaveUI_NextWaveLabel = "Next Wave:";
	public const string PostWaveUI_NextWave = "2";

	#endregion

	#endregion

	#region Field Of Fire - On Screen UI 

	public const string OnScreen_Ammunition = "0";
	public const string OnScreen_AmmunitionTotal = "40";

	#endregion
}
