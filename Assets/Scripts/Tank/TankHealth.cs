#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#endregion

/// <summary>
/// Handles everything in regards to our tanks health system
/// </summary>
[System.Serializable]
public class TankHealth 
{
	#region Public Variables 

	/// <summary>
	///     The minimum amount of health for our tank
	/// </summary>
	public float minHealth = 0; // our min health

    /// <summary>
    ///     The maximum amount of health for our tank 
    /// </summary>
    public float maxHealth = 100; // our max health

    /// <summary>
    ///     Is our tank character currently alive ? 
    /// </summary>
    public bool isDead = true; // is our character alive?

    #endregion

    #region Private Variables 

    /// <summary>
    ///     The current health for the tank 
    /// </summary>

    [SerializeField] private float m_CurrentHealth; // our current health

    /// <summary>
    ///     Reference to the current players tank transform 
    /// </summary>
    [SerializeField] private Transform m_tankReference; // reference to the tank that this script is attached to

	#endregion

	#region Public Methods 
	/// <summary>
	///     Returns the current health for the player character 
	/// </summary>
	public float Health
    {
        get
        {
            return m_CurrentHealth; // return our current health
        }
        set
        {
            m_CurrentHealth = value; // set our currenthealth to the value coming in.

          
            m_CurrentHealth = Mathf.Clamp(m_CurrentHealth, minHealth, maxHealth); // making sure that what our damage is, it clamps it between 0 and 100
            Debug.Log("[TankHealth]: " + "Health remaining " + m_CurrentHealth);

            // so if we have less than 0 health we must be dead. 
            if (m_CurrentHealth <= 0)
            {
                isDead = true;
                // if we are dead we'd want some explosions
                // call an event for the players death
                FireModeEvents.OnObjectDestroyedEvent?.Invoke(m_tankReference); // so pass in our tank's health script into the tank destroyed event
            }
            else
            {
                isDead = false;
            }


            if (m_tankReference.GetComponent<MainPlayerTank>())
			{
                // Then we know its a player and not the AI 

                FireModeEvents.UpdatePlayerHealthEvent?.Invoke(m_CurrentHealth);
			}

        }
    }

    /// <summary>
    ///     Setup Tank's Health 
    /// </summary>
    /// <param name="TankTransform"></param>
    public void Setup(Transform TankTransform)
    {
        m_tankReference = TankTransform;
       
        Health = maxHealth; // set our current health to max health

        FireModeEvents.UpdatePlayerHealthEvent?.Invoke(Health);
    }

    /// <summary>
    /// Applies the amount of health change, if negative it applies damage
    /// If positive it applies health
    /// </summary>
    /// <param name="Amount"></param>
    public void ApplyHealthChange(float Amount)
    {
        Debug.Log("Applying health change " + Amount);
        Health += Amount; // increase our health by the amount
    }

	#endregion
}
