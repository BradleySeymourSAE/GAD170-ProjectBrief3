using UnityEngine;
using System;
using System.Collections;
using System.IO;
using SharpConfig;


/// <summary>
///		Creates or loads a user configuration file 
/// </summary>
public class Config 
{
	private Configuration UserConfig = new Configuration();
	private string configFilePath;
	private const string m_configFileName = "PROFSAVE_Profile.cfg";

	/// <summary>
	///		The Profile Config instance 
	/// </summary>
	public static Config Instance;


	/// <summary>
	///		Returns Configuration File 
	/// </summary>
	public Config()
	{
		configFilePath = Application.dataPath + "/" + m_configFileName;


		// Attempt to find the configuration file path
		if (!File.Exists(m_configFileName))
		{ 
			// If the file doesnt exist, Create the file 
			CreateUserProfileConfig();
		}
		else
		{
			// We have the configuration file 
			UserConfig = Configuration.LoadFromFile(m_configFileName);
		}
	}

	/// <summary>
	///		Loads the user configuration 
	/// </summary>
	/// <returns></returns>
	public Configuration LoadConfig()
	{
		return Configuration.LoadFromFile(m_configFileName);
	}

	/// <summary>
	///		Saves a user profile config to the games main directory 
	/// </summary>
	/// <param name="Config"></param>
	/// <param name="FilePath"></param>
	public void SaveConfiguration(Configuration Config, string FilePath)
	{
		Debug.Log("[Config.SaveConfiguration]: " + "Saving User Profile Configuration!");
		Config.SaveToFile(FilePath);
	}


	/// <summary>
	///		Creates the user config file 
	/// </summary>
	private void CreateUserProfileConfig()
	{
		try
		{
			var video = UserConfig["Video"];
			var audio = UserConfig["Audio"];

			/// <summary>
			///		Users Video Settings
			/// </summary>
			video["VSyncEnable"].IntValue = 0;
			video["VSync"].IntValue = 0;
			video["AnisotrophicFilter"].IntValue = 0;
			video["AntiAliasingQuality"].IntValue = 0;
			video["MeshQuality"].IntValue = 0;
			video["FieldOfView"].FloatValue = 0.500000f;
			video["FullscreenEnabled"].IntValue = 1;
			video["OverallGraphicsQuality"].IntValue = 5;
			video["ResolutionHeight"].IntValue = 1080;
			video["ResolutionHertz"].IntValue = 0;
			video["ResolutionWidth"].IntValue = 1920;
			video["ShaderQuality"].IntValue = 0;
			video["ShadowQuality"].IntValue = 0;
			video["TextureQuality"].IntValue = 0;

			///<summary>
			///		Audio Settings
			/// </summary>


			audio["AudioQuality"].IntValue = 1;
			audio["DialogueVolume"].DoubleValue = 0.700000;
			audio["SFXVolume"].DoubleValue = 0.700000;
			audio["MasterVolume"].DoubleValue = 0.500000;


			SaveConfiguration(UserConfig, configFilePath);
		}
		catch (Exception errorException)
		{
			Debug.LogError("[Config]: " + "There was an error while trying to create your profile config! Error: " + errorException);
		}
	}
}
