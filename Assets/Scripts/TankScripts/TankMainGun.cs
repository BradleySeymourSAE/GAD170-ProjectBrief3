using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles firing the main weapon of the tank
/// </summary>
[System.Serializable]
public class TankMainGun
{
    public Transform mainGunTransform; // reference to the main gun of the tank
    public GameObject tankShellPrefab; // reference to the tank prefab we want to fire
   //  public GameObject tankBlastPrefab; // reference to second type of gun fire
   //  

    public float MinimumFireForce = 15f; // the minimum amount of force for our weapon
    public float MaximumFireForce = 50f; // the maximum amount of force for our weapon
    public float MaximumRechargeTime = 0.75f; // the maximum amount of time we will allow to charge up and fire

    public Slider MainWeaponArrowIndicator; // a reference to the main gun slider

    private float m_currentLaunchForce; // the force we should use to fire our shell
    private float m_chargeSpeed; // how fast we should charge up our weapon
    private bool m_weaponHasFired; // have we just fired our weapon?
    private bool m_aimingDownSight; // are we aiming down sight 

    public AudioSource WeaponSystemAudioSource; // reference to the audio source for the main gun
    public AudioClip Charge_SoundFX; // a charging up sound
    public AudioClip WeaponFire_SoundFX; // a firing weapon SFX.

    private bool m_enabledShooting; // should we be allowed to fire?
    private bool m_enableAimDownSight; // should we be allowed to ads? 
    /// <summary>
    /// Sets up all the necessary variables for our main gun script
    /// </summary>
    public void SetUp()
    {
        m_currentLaunchForce = MinimumFireForce; // set our current launch force to the min
        m_chargeSpeed = (MaximumFireForce - MinimumFireForce) / MaximumRechargeTime; // get the range between the max and min, and divide it by how quickly we charge
        MainWeaponArrowIndicator.minValue = MinimumFireForce; // set the min and max programatically
        MainWeaponArrowIndicator.maxValue = MaximumFireForce;
        WeaponSystemAudioSource.clip = Charge_SoundFX; // set the clip to the charging effect
        WeaponSystemAudioSource.loop = false; // don't set it to loop
        EnableShooting(false); // disable shooting
        EnableAimDownSight(false);
    }

    /// <summary>
    /// Called to enable/disable shooting
    /// </summary>
    /// <param name="Enabled"></param>
    public void EnableShooting(bool Enabled)
    {
        m_enabledShooting = Enabled;
    }

    /// <summary>
    ///     Called to Enable / Disable Aim down sight 
    /// </summary>
    /// <param name="Enabled"></param>
    public void EnableAimDownSight(bool Enabled)
	{
        m_enableAimDownSight = Enabled;
	}

    /// <summary>
    ///     Update's the Main Gun movement value 
    /// </summary>
    /// <param name="MainGunShootValue"></param>

    public void UpdateMainGun(float MainGunShootValue, float MainGunAimValue)
    {   
        if (m_enabledShooting != true)
        {
            return; // don't do anything
        }

        if (m_currentLaunchForce >= MaximumFireForce && !m_weaponHasFired)
        {
            // if we are at max charge and we haven't fired the weapon
            m_currentLaunchForce = MaximumFireForce;
            FireWeapon(); // fire our gun
        }
        // get the input from out main button press
        else if (MainGunShootValue > 0 && !m_weaponHasFired)
        {
            //Debug.Log("Weapon button pressed");

            // Charge up the weapon 
            // Increase the force of the weapon 
            m_currentLaunchForce += m_chargeSpeed * Time.deltaTime; 

            // Play the weapon audio from the weapon system audio source
            if (!WeaponSystemAudioSource.isPlaying)
            {
                // Play Weapon Charging Audio Clip
                WeaponSystemAudioSource.Play();
                Debug.Log("Charging");
            }
            // play a charging up sound effect
        }
        else if (MainGunShootValue < 0 && !m_weaponHasFired)
        {
           // Debug.Log("Weapon Button Released");
            // we've released our button
            // we want to fire our weapon
            FireWeapon(true);
        }
        else if (MainGunShootValue < 0 && m_weaponHasFired)
        {
            m_weaponHasFired = false;
        }
  
        MainWeaponArrowIndicator.value = m_currentLaunchForce; // set our arrow back to min at all times

        if (MainGunAimValue > 0 && !m_aimingDownSight)
		{
            Debug.Log("Aiming Down Sight Pressed!");
            AimDownSight();
		}
        else if (MainGunAimValue < 0 && !m_aimingDownSight)
		{
            Debug.Log("Aiming Down Sight Released!");
            AimDownSight(true);
		}
        else if (MainGunAimValue < 0 && m_aimingDownSight)
		{
            m_aimingDownSight = false;
		}
    }


    /// <summary>
    /// Called when the fire button has been released
    /// </summary>
    private void FireWeapon(bool ButtonReleased = false)
    {
        m_weaponHasFired = true; // we have fired our weapon
        // spawns in a tank shell at the main gun transform and matches the rotation of the main gun and stores it in the clone GameObject variable
        GameObject clone = Object.Instantiate(tankShellPrefab, mainGunTransform.position, mainGunTransform.rotation);

        // If the clone has a rigidbody, we want to add some velocity to it to make it fire!
        if(clone.GetComponent<Rigidbody>())
        {
            clone.GetComponent<Rigidbody>().velocity = m_currentLaunchForce * mainGunTransform.forward; // make the velocity of our bullet go in the direction of our gun at the launch force
        }
        Object.Destroy(clone,5f);
        
        WeaponSystemAudioSource.PlayOneShot(WeaponFire_SoundFX); // play the firing sound effect
        WeaponSystemAudioSource.Stop(); // stop charging up
        m_currentLaunchForce = MinimumFireForce;
        // only reset our weapon if we have released our fire button, don't allow it if we overcharged
        if (ButtonReleased)
        {
            m_weaponHasFired = false;
        }
    }

    /// <summary>
    ///     Called when the ADS button has been released 
    ///     TODO:
    /// </summary>
    /// <param name="ButtonReleased"></param>
    private void AimDownSight(bool ButtonReleased = false)
	{
        m_aimingDownSight = true;

        // We would probably get the clone's camera here? Then use some sort of zoom effect 

        // Then reset after releasing the aim down sight button 

        if (ButtonReleased)
		{
            m_aimingDownSight = false;
		}
	}
}
