using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfantryHealth
{

	public float minimumHealth = 0, maximumHealth = 100;
	public bool isDead = true;
	public Color FullHealthColor = Color.green;
	public Color WarnHealthColor = Color.yellow;
	public Color NoHealthColor = Color.red;

	public Slider HealthSlider;
	private Image HealthBarImage;


	[SerializeField] private float Health;

	private Transform m_infantryReference;

	
	public float GetCurrentHealth
	{
		get
		{
			return Health;
		}
		set
		{
			Health = value;

			Health = Mathf.Clamp(Health, minimumHealth, maximumHealth);



			if (Health <= 0)
			{
				isDead = true;


				FireModeEvents.OnObjectDestroyedEvent?.Invoke(m_infantryReference);
			}
			else
			{
				isDead = false;
			}


			if (HealthSlider != null)
			{
				HealthSlider.value = Health;

				if (Health > 0 && Health <= 50f && HealthBarImage != null)
				{
					HealthBarImage.color = Color.Lerp(NoHealthColor, WarnHealthColor, Health / maximumHealth);
				}
				else if (Health > 50f && Health <= 100f && HealthBarImage != null)
				{
					HealthBarImage.color = Color.Lerp(WarnHealthColor, FullHealthColor, Health / maximumHealth);
				}
			}
			else
			{
				Debug.LogWarning("[InfantryHealth.GetCurrentHealth]: " + "No health bar slider could be found for infantry character!");
			}
		}
	}



	/// <summary>
	///		Sets up the infantry transform 
	/// </summary>
	/// <param name="InfantryTransform"></param>
	public void Setup(Transform InfantryTransform)
	{
		m_infantryReference = InfantryTransform;

		if (HealthSlider != null)
		{
			if (HealthSlider.fillRect != null)
			{
				HealthBarImage = HealthSlider.fillRect.transform.GetComponent<Image>();
				Debug.Log("[InfantryHealth.Setup]: " + "Found health bar image instance!");
			}
			else
			{
				Debug.LogWarning("[InfantryHealth.Setup]: " + "We could not find a health bar slider image!");
			}
		}

		Health = maximumHealth;
	}


	/// <summary>
	///		Applies health change to our infantry character 
	/// </summary>
	/// <param name="DamageAmount"></param>
	public void ApplyHealthChange(float DamageAmount)
	{
		Debug.Log("[InfantryHealth.ApplyHealthChange]: " + "Appling health change: " + DamageAmount);
		Health += DamageAmount;
	}
}