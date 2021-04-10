#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#endregion

/// <summary>
///		Infantry Health AI Data Class
/// </summary>
[System.Serializable]
public class InfantryHealth
{
	/// <summary>
	///		Minimum & Maximum Health Clamped Values
	/// </summary>
	public float minimumHealth = 0, maximumHealth = 100;
	
	/// <summary>
	///		Is the player currently dead? 
	/// </summary>
	public bool isDead = true;

	/// <summary>
	///		The current health of the player 
	/// </summary>
	[SerializeField] private float m_CurrentHealth;

	/// <summary>
	///		References to the current infantry character 
	/// </summary>
	private Transform m_infantryReference;

	#region Public Methods 

	/// <summary>
	///		Returns the current health of the character 
	/// </summary>
	public float Health
	{
		get
		{
			return m_CurrentHealth;
		}
		set
		{
			m_CurrentHealth = value;

			m_CurrentHealth = Mathf.Clamp(m_CurrentHealth, minimumHealth, maximumHealth);

			if (m_CurrentHealth <= 0)
			{
				isDead = true;

				FireModeEvents.OnObjectDestroyedEvent?.Invoke(m_infantryReference);
			}
			else
			{
				isDead = false;
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

	#endregion
}