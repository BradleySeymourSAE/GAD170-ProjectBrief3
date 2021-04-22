#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#endregion

[System.Serializable]
/// <summary>
///		On Screen UI - Ammunition Counters etc 
/// </summary>
public class OnScreenUI
{
	#region Public Variables 
	public GameObject OnScreenUIContainer;

	public MinimapUI Minimap;

	public AmmunitionUI Ammunition;

	public CrosshairUI Crosshair;

	public HealthBarUI Health;

	#endregion

	#region Private Variables 
	
	/// <summary>
	///		Refernce to fire mode UI instance 
	/// </summary>
	private FireModeUI m_FireModeUI;

	#endregion

	#region Public Methods 

	/// <summary>
	///		Setup references 
	/// </summary>
	/// <param name="FireModeUI"></param>
	public void Setup(FireModeUI FireModeUI)
	{
		m_FireModeUI = FireModeUI;

		Minimap.Setup(m_FireModeUI);
		Ammunition.Setup(m_FireModeUI);
		Crosshair.Setup(m_FireModeUI);
		Health.Setup(m_FireModeUI);
	}

	/// <summary>
	///		Shows the Minimap, Ammunition Counter, Crosshair and Health UI 
	/// </summary>
	/// <param name="ShouldDisplay"></param>
	public void ShowScreen(bool ShouldDisplay)
	{
		OnScreenUIContainer.SetActive(ShouldDisplay);
		Minimap.Show(ShouldDisplay); // Show minimap UI 
		Ammunition.Show(ShouldDisplay); // Show Ammo Counter UI 
		Crosshair.Show(ShouldDisplay); // Show Crosshair UI 
		Health.Show(ShouldDisplay); // Display Health UI
	}
	#endregion
}


/// <summary>
///		Heads up Display - Minimap UI 
/// </summary>
[System.Serializable]
public class MinimapUI
{
	#region Public Variables 
	
	public GameObject minimapUI;
	public Image minimapBorder;
	public RawImage mapRender;

	#endregion

	#region Private Variables 

	/// <summary>
	///		Reference to fire mode ui instance 
	/// </summary>
	private FireModeUI m_FireModeUI;

	#endregion

	#region Public Methods 
	/// <summary>
	///		Sets up the fire mode ui instance 
	/// </summary>
	/// <param name="FireModeUI"></param>
	public void Setup(FireModeUI FireModeUI)
	{
		m_FireModeUI = FireModeUI;
	}

	/// <summary>
	///		Toggles showing of the minimap 
	/// </summary>
	/// <param name="ShouldDisplay"></param>
	public void Show(bool ShouldDisplay)
	{
		minimapUI.SetActive(ShouldDisplay);
	}

	#endregion
}

/// <summary>
///		Heads up Display - Ammunition UI 
/// </summary>
[System.Serializable]
public class AmmunitionUI
{
	#region Public Variables 

	/// <summary>
	///		Ammunition UI Container Game Object 
	/// </summary>
	public GameObject ammunitionUI;

	/// <summary>
	///		Ammunition Background Image 
	/// </summary>
	public Image backgroundImage;

	/// <summary>
	///		The amount of ammunition in clip & the ammount of ammunition in total COMBINED 
	/// </summary>
	public TMP_Text ammunitionText;

	#endregion

	#region Private Variables 

	/// <summary>
	///		Reference to Fire Mode UI Instance 
	/// </summary>
	private FireModeUI m_FireModeUI;

	#endregion

	#region Public Methods 
	/// <summary>
	///		Sets up the fire mode instance and colors 
	/// </summary>
	/// <param name="FireModeUI"></param>
	public void Setup(FireModeUI FireModeUI)
	{
		m_FireModeUI = FireModeUI;
		ammunitionText.GetComponentInChildren<TMP_Text>().text =  GameTextUI.OnScreen_Ammunition;
	}

	/// <summary>
	///		Toggle whether to display the ammunition UI 
	/// </summary>
	/// <param name="ShouldDisplay"></param>
	public void Show(bool ShouldDisplay) => ammunitionUI.SetActive(ShouldDisplay);

	/// <summary>
	///		Sets the Ammunition On Screen UI  
	/// </summary>
	/// <param name="Ammunition"></param>
	public void SetAmmunition(int Ammunition) => ammunitionText.GetComponentInChildren<TMP_Text>().text = Ammunition.ToString("0");
	
	#endregion
}

/// <summary>
///		Heads up Display Crosshair UI 
/// </summary>
[System.Serializable]
public class CrosshairUI
{
	#region Public Variables 
	
	/// <summary>
	///		Crosshair UI Container Game Object 
	/// </summary>
	public GameObject crosshairUI;

	/// <summary>
	///		The crosshair reticle image 
	/// </summary>
	public Image crosshair;

	#endregion

	#region Private Variables 

	/// <summary>
	///		Reference to the Fire Mode UI Instance 
	/// </summary>
	private FireModeUI m_FireModeUI;

	/// <summary>
	///		Default Color for the reticle 
	/// </summary>
	[SerializeField] private Color m_ReticleColor = Color.white;

	#endregion

	#region Public Methods 
	/// <summary>
	///		Sets up the fire mode instance and colors 
	/// </summary>
	/// <param name="FireModeUI"></param>
	public void Setup(FireModeUI FireModeUI)
	{
		m_FireModeUI = FireModeUI;
		crosshair.color = m_ReticleColor;
	}

	/// <summary>
	///		Toggle Display Crosshair
	/// </summary>
	/// <param name="ShouldDisplay"></param>
	public void Show(bool ShouldDisplay)
	{
		crosshairUI.SetActive(ShouldDisplay);
	}
	
	#endregion
}

/// <summary>
///		Heads up Display - Health Bar UI  
/// </summary>
[System.Serializable]
public class HealthBarUI
{
	#region Public Variables 

	/// <summary>
	///		The Health UI Container Game Object 
	/// </summary>
	public GameObject healthUI;
	
	/// <summary>
	///		The current health of the character 
	/// </summary>
	public TMP_Text currentHealth;

	/// <summary>
	///		The health bar slider 
	/// </summary>
	public Slider healthBarSlider;

	[SerializeField] private Color NoHealth = Color.red;
	[SerializeField] private Color FullHealth = Color.blue;


	private Image fillImage; 

	#endregion

	#region Private Variables 

	/// <summary>
	///		Reference to the Fire Mode UI Instance 
	/// </summary>
	private FireModeUI m_FireModeUI;

	#endregion

	#region Public Methods

	/// <summary>
	///		Sets up the fire mode instance and colors 
	/// </summary>
	/// <param name="FireModeUI"></param>
	public void Setup(FireModeUI FireModeUI)
	{
		m_FireModeUI = FireModeUI;

		if (healthBarSlider != null)
		{
			fillImage = healthBarSlider.fillRect.transform.GetComponent<Image>();
		}
		else
		{
			Debug.LogWarning("[FireModeUI.OnScreenUI.Setup]: " + "Could not find the health bar slide image!");
		}

		currentHealth.GetComponentInChildren<TMP_Text>().text = GameTextUI.OnScreen_Health;
	}

	
	/// <summary>
	///		Toggles Show / Hiding of the Health UI 
	/// </summary>
	/// <param name="ShouldDisplay"></param>
	public void Show(bool ShouldDisplay)
	{
		healthUI.SetActive(ShouldDisplay);
	}

	/// <summary>
	///		Sets the health AMOUNT as an integer 
	/// </summary>
	/// <param name="amount"></param>
	public void SetHeathBarText(float amount) => currentHealth.GetComponentInChildren<TMP_Text>().text = amount.ToString("0");
	#endregion

	/// <summary>
	///		Sets the health bar slider UI 
	/// </summary>
	/// <param name="CurrentHealthAmount"></param>
	/// <param name="Maximum"></param>
	public void SetHealthBarSlider(float CurrentHealthAmount = 0, float Maximum = 100)
	{
		if (healthBarSlider != null)
		{ 
			healthBarSlider.value = CurrentHealthAmount;

			if (fillImage != null)
			{
				fillImage.color = Color.Lerp(NoHealth, FullHealth, CurrentHealthAmount / Maximum);
			}
		}
		else
		{
			Debug.LogWarning("[FireModeUI.OnScreenUI.SetHealthBarSlider]: " + "There is no Health Bar slider currently set up!");
		}
	}

}