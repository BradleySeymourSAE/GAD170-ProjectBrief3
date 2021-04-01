using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///		Handles the Infantry AI's Health System 
/// </summary>
[System.Serializable]
public class InfantryAI_Health
{
	private float currentHealth; // current health of the infantry character
	private Transform infantryAI; // the parent of the infantry character 

	[Header("Health Settings")]
	public float minimumHealth = 0, maximumHealth = 100;
	public bool isDead = true; // default to being dead

	[Header("Health Color Settings")]
	public Color FullHealth = Color.green;
	public Color WarningHealth = Color.yellow;
	public Color ZeroHealth = Color.red;
	
	public Slider healthBarSlider;
	private Image healthBarFill;


	/// <summary>
	///		Returns the Infantry AI Characters Current HP 
	/// </summary>
	public float HP
	{ 
		get
		{
			return currentHealth;
		}
		set
		{
			currentHealth = value; // set current health to the value 

			// Clamp health value between min and max value 
			currentHealth = Mathf.Clamp(currentHealth, minimumHealth, maximumHealth);
		

			if (currentHealth <= 0)
			{
				isDead = true;

				// Call the infantry death event 
				FieldOfFireEvents.OnEnemyAIInfantryDeathEvent?.Invoke(infantryAI);
			}
			else
			{
				isDead = false;
			}


			// Health Slider
			if (healthBarSlider != null)
			{
				healthBarSlider.value = HP;

				if (HP > 0 && HP <= 50f && healthBarFill != null)
				{
					healthBarFill.color = Color.Lerp(ZeroHealth, WarningHealth, HP / maximumHealth);
				}
				else if (HP > 50f && HP <= 100f && healthBarFill != null)
				{
					healthBarFill.color = Color.Lerp(WarningHealth, FullHealth, HP / maximumHealth);
				}
			}
			else
			{
				Debug.LogError("[InfantryAI_Health]: " + "There is currently no health slider for this infantry AI Character!");
			}
		}
	}

	/// <summary>
	///		Sets up the enemy ai's health 
	/// </summary>
	/// <param name="InfantryAI"></param>
	public void Setup(Transform InfantryAI)
	{
		infantryAI = InfantryAI;


		if (healthBarSlider != null)
		{
			if (healthBarSlider.fillRect != null)
			{
				healthBarFill = healthBarSlider.fillRect.transform.GetComponent<Image>();
				Debug.Log("Got Health Bar!");
			}
			else
			{
				Debug.LogError("Could not find health bar for infantry AI!");
			}
		}


		// Set the default health for infantry character 
		HP = maximumHealth;
	}

	/// <summary>
	///		Applys health changes to the infantry AI characters
	/// </summary>
	/// <param name="Amount"></param>
	public void ApplyHealth(float Amount)
	{
		Debug.Log("Applying Health Change " + Amount);
		HP += Amount;
	}
}
