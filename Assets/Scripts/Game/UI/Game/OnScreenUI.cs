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
	
	// Add minimap UI references 


	// Add Ammunition counter UI references 

	// Link them up 

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

	/// <summary>
	///		The minimaps border color 
	/// </summary>
	[SerializeField] private Color m_BorderColor;

	/// <summary>
	///		The minimaps background color 
	/// </summary>
	[SerializeField] private Color m_MapColor;

	#endregion

	#region Public Methods 
	/// <summary>
	///		Sets up the fire mode instance and colors 
	/// </summary>
	/// <param name="FireModeUI"></param>
	public void Setup(FireModeUI FireModeUI)
	{
		m_FireModeUI = FireModeUI;
		m_BorderColor = minimapBorder.color;
		m_MapColor = mapRender.color;
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

	/// <summary>
	///		Background Color of the Ammuniton UI 
	/// </summary>
	[SerializeField] private Color m_BackgroundColor;

	#endregion

	#region Public Methods 
	/// <summary>
	///		Sets up the fire mode instance and colors 
	/// </summary>
	/// <param name="FireModeUI"></param>
	public void Setup(FireModeUI FireModeUI)
	{
		m_FireModeUI = FireModeUI;
		m_BackgroundColor = backgroundImage.color;
		ammunitionText.GetComponentInChildren<TMP_Text>().text = GameTextUI.OnScreen_Ammunition + "/" + GameTextUI.OnScreen_AmmunitionTotal;
	}


	/// <summary>
	///		Toggle whether to display the ammunition UI 
	/// </summary>
	/// <param name="ShouldDisplay"></param>
	public void Show(bool ShouldDisplay)
	{
		ammunitionUI.SetActive(ShouldDisplay);
	}


	/// <summary>
	///		Sets the Ammunition On Screen UI  
	/// </summary>
	/// <param name="AmmoLoaded"></param>
	/// <param name="AmmoTotal"></param>
	public void SetAmmunition(int AmmoLoaded, int AmmoTotal)
	{
		ammunitionText.GetComponentInChildren<TMP_Text>().text = AmmoLoaded.ToString() + " / " + AmmoTotal.ToString();
	}
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
		m_ReticleColor = crosshair.color;
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
	
	/// <summary>
	///		The fill image component of the slider 
	/// </summary>
	private Image m_FillImage;

	#endregion

	#region Private Variables 

	/// <summary>
	///		Full Health Color Value 
	/// </summary>
	[SerializeField] private Color FullHealth;

	/// <summary>
	///		Half Health Color Value 
	/// </summary>
	[SerializeField] private Color HalfHealth;

	/// <summary>
	///		Low Health Color value 
	/// </summary>
	[SerializeField] private Color LowHealth;

	/// <summary>
	///		Reference to the Fire Mode UI Instance 
	/// </summary>
	private FireModeUI m_FireModeUI;

	/// <summary>
	///		Store local max health variable 
	/// </summary>
	private float maximumHealth = 100;

	#endregion

	#region Public Methods

	/// <summary>
	///		Sets up the fire mode instance and colors 
	/// </summary>
	/// <param name="FireModeUI"></param>
	public void Setup(FireModeUI FireModeUI)
	{
		m_FireModeUI = FireModeUI;


		if (healthBarSlider && healthBarSlider.fillRect)
		{
				m_FillImage = healthBarSlider.fillRect.transform.GetComponent<Image>();
		}
		else
		{
				Debug.LogError("No health bar could be found!");
		}
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
	///		Updates the Health Bar of the player 
	/// </summary>
	/// <param name="CurrentHealth"></param>
	public void UpdateHealth(float CurrentHealth)
	{
		healthBarSlider.value = CurrentHealth;

		if (m_FillImage != null)
		{
			// If the current health is greater than 0 and current health is less than or equal to 50 
			if (CurrentHealth > 50f && CurrentHealth <= 100f)
			{ 
				m_FillImage.color = Color.Lerp(HalfHealth, FullHealth, CurrentHealth / maximumHealth);
			}
			// otherwise if current health is greater than 50 but less than 100 
			else if (CurrentHealth < 0f && CurrentHealth >= 50f)
			{
				m_FillImage.color = Color.Lerp(LowHealth, HalfHealth, CurrentHealth / maximumHealth);
			}
		}
	}
	#endregion

}