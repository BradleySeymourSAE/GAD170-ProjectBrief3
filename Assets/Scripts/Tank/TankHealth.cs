using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Handles everything in regards to our tanks health system
/// </summary>
[System.Serializable]
public class TankHealth 
{
    public float minHealth = 0; // our min health
    public float maxHealth = 100; // our max health
    [SerializeField] private float CurrentHealth; // our current health
    public bool isDead = true; // is our character alive?
    public Color fullHealthColour = Color.green; // our full health colour
    public Color warningHealthColor = Color.yellow; // warning health colour (50%)
    public Color zeroHealthColour = Color.red; // colour of no health
    private Transform m_tankReference; // reference to the tank that this script is attached to




    public float Health
    {
        get
        {
            return CurrentHealth; // return our current health
        }
        set
        {
            CurrentHealth = value; // set our currenthealth to the value coming in.

            CurrentHealth = Mathf.Clamp(CurrentHealth, minHealth, maxHealth); // making sure that what our damage is, it clamps it between 0 and 100

            // so if we have less than 0 health we must be dead. 
            if (CurrentHealth <= 0)
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

    }

    /// <summary>
    /// Applies the amount of health change, if negative it applies damage
    /// If positive it applies health
    /// </summary>
    /// <param name="Amount"></param>
    public void ApplyHealthChange(float Amount)
    {
        Debug.Log(Amount);
        Health += Amount; // increase our health by the amount
    }
}
