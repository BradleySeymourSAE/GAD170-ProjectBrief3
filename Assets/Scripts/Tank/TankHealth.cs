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
    [SerializeField] private float currentHealth; // our current health
    public bool isDead = true; // is our character alive?
    public Color fullHealthColour = Color.green; // our full health colour
    public Color warningHealthColor = Color.yellow; // warning health colour (50%)
    public Color zeroHealthColour = Color.red; // colour of no health
    private Transform m_tankReference; // reference to the tank that this script is attached to
    public Slider healthSlider; // reference to the health Slider
    private Image fillImage; // reference to the fill image component of our slider;

    public float CurrentHealth
    {
        get
        {
            return currentHealth; // return our current health
        }
        set
        {
            currentHealth = value; // set our currenthealth to the value coming in.

            currentHealth = Mathf.Clamp(currentHealth, minHealth, maxHealth); // making sure that what our damage is, it clamps it between 0 and 100

            // so if we have less than 0 health we must be dead. 
            if (currentHealth <= 0)
            {
                isDead = true;
                // if we are dead we'd want some explosions
                // call an event for the players death
                TankGameEvents.OnObjectDestroyedEvent?.Invoke(m_tankReference); // so pass in our tank's health script into the tank destroyed event
            }
            else
            {
                isDead = false;
            }

            if(healthSlider != null)
            {
                healthSlider.value = CurrentHealth;

                if (CurrentHealth > 0 && CurrentHealth <= 50f && fillImage != null)
				{
                    fillImage.color = Color.Lerp(zeroHealthColour, warningHealthColor, CurrentHealth / maxHealth);
				}
                else if (CurrentHealth > 50f && CurrentHealth <= 100f && fillImage != null)
				{
                    fillImage.color = Color.Lerp(warningHealthColor, fullHealthColour, CurrentHealth / maxHealth);
				}
                
            }
            else
            {
                Debug.LogError("There is no health slider");
            }
        }
    }
    public void Setup(Transform TankTransform)
    {
        m_tankReference = TankTransform;
       
        if(healthSlider != null)
        {
            if(healthSlider.fillRect != null)
            {
                fillImage = healthSlider.fillRect.transform.GetComponent<Image>(); // grab a reference to our health slider image
                Debug.Log("Got instance");
            }
            else
            {
                Debug.LogError("There is no health slider image");
            }
        }
        CurrentHealth = maxHealth; // set our current health to max health

    }

    /// <summary>
    /// Applies the amount of health change, if negative it applies damage
    /// If positive it applies health
    /// </summary>
    /// <param name="Amount"></param>
    public void ApplyHealthChange(float Amount)
    {
        Debug.Log(Amount);
        CurrentHealth += Amount; // increase our health by the amount
    }
}
