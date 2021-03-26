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

    public float minimumBulletVelocity = 15f; // the minimum amount of force for our weapon
    public float maximumBulletVelocity = 50f; // the maximum amount of force for our weapon
    public float maximumReloadTime = 0.25f; // the maximum amount of time we will allow to charge up and fire

    public Slider MainWeaponArrowIndicator; // a reference to the main gun slider

    private float m_currentFireVelocity; // the force we should use to fire our shell
    private float m_reloadSpeed; // how fast we should charge up our weapon
    private bool weaponHasFired; // have we just fired our weapon?
    private bool isAimingDownSight; // are we aiming down sight 

    public AudioSource WeaponSystemAudioSource; // reference to the audio source for the main gun
    public AudioClip Charge_SoundFX; // a charging up sound
    public AudioClip WeaponFire_SoundFX; // a firing weapon SFX.

    private bool enableWeaponWiring; // should we be allowed to fire?
    private bool enableADSAiming; // should we be allowed to ads? 
    /// <summary>
    /// Sets up all the necessary variables for our main gun script
    /// </summary>
    public void SetUp()
    {
        m_currentFireVelocity = minimumBulletVelocity; // set our current launch force to the min
        m_reloadSpeed = (maximumBulletVelocity - minimumBulletVelocity) / maximumReloadTime; // get the range between the max and min, and divide it by how quickly we charge
        MainWeaponArrowIndicator.minValue = minimumBulletVelocity; // set the min and max programatically
        MainWeaponArrowIndicator.maxValue = maximumBulletVelocity;
        WeaponSystemAudioSource.clip = Charge_SoundFX; // set the clip to the charging effect
        WeaponSystemAudioSource.loop = false; // don't set it to loop
        EnableShooting(false); // disable shooting
        EnableAimDownSight(false); // disable ads 
    }

    /// <summary>
    /// Called to enable/disable shooting
    /// </summary>
    /// <param name="Enabled"></param>
    public void EnableShooting(bool Enabled)
    {
        enableWeaponWiring = Enabled;
    }

    /// <summary>
    ///     Called to Enable / Disable Aim down sight 
    /// </summary>
    /// <param name="Enabled"></param>
    public void EnableAimDownSight(bool Enabled)
	{
        enableADSAiming = Enabled;
	}

    /// <summary>
    ///     Update's the Main Gun movement value 
    /// </summary>
    /// <param name="MainGunShootValue"></param>

    public void UpdateMainGun(float MainGunShootValue, float MainGunAimValue)
    {   
        if (enableWeaponWiring != true)
        {
            return; // don't do anything
        }

        // If the current weapon fire velocity is greater than or equal to the maximum bullet velocity and the 
        // weapon has not yet fired
        if (m_currentFireVelocity >= maximumBulletVelocity && !weaponHasFired)
        {
            // Then the current fire velocity is equal to the maximum bullet velocity
            m_currentFireVelocity = maximumBulletVelocity;
            
            // Fire our main weapon 
            FireWeapon();
        }
        // Otherwise, If the player is trying to shoot and the weapon has not fired 
        else if (MainGunShootValue > 0 && !weaponHasFired)
        {
            //Debug.Log("Weapon button pressed");
            Debug.Log("[TankMainGun.UpdateMainGun]: " + "Firing Main Weapon!");

            // Charge the main weapon 
            // Set the current fire velocity to the reload speed multiplied by the time factor.
            m_currentFireVelocity += m_reloadSpeed * Time.deltaTime; 


            // If the weapon system audio system isn't currently playing the audio
            if (!WeaponSystemAudioSource.isPlaying)
            {
                // Then play the Weapon Fire Audio FX from the Weapon System Audio source
                WeaponSystemAudioSource.Play();
                Debug.Log("[TankMainGun.UpdateMainGun]: " + "Charging Main Weapon!");
            }
            // play a charging up sound effect
        }
        // Otherwise, If the main gun shoot value is less than 0 and the weapon has not been fire 
        else if (MainGunShootValue < 0 && !weaponHasFired)
        {
            // Then the player has released the weapon fire buttton
            // We want to fire the weapon. 
    
            Debug.Log("[TankMainGun.UpdateMainGun]: " + " Weapon Fire key has been released!");
            FireWeapon(true);
        }
        // Otherwise if the main gun fire value is less than 0 and the weapon has been fired 
        else if (MainGunShootValue < 0 && weaponHasFired)
        {
            // We want to reset the weapon fired value back to false. 
            weaponHasFired = false;
        }
  
        // Set our main weapon arrow ui to the current velocity value 
        MainWeaponArrowIndicator.value = m_currentFireVelocity; 

       
        // If aim down sight is not enabled 
        if (enableADSAiming != true)
		{
            // We want to return.
            return;
		}
        
        
        // If the Main Gun ADS value has been pressed and the tank is currently not aiming down sight 
        if (MainGunAimValue > 0 && !isAimingDownSight)
		{
            // Then we want to call the Aim Down Sight function 
            Debug.Log("[TankMainGun.UpdateMainGun]: " + "Main Weapon is now aiming down sight!");
            AimDownSight();
		}
        else if (MainGunAimValue < 0 && !isAimingDownSight)
		{
            Debug.Log("[TankMainGun.UpdateMainGun]: " + "Main weapon is still aiming down sight");
            AimDownSight(true);
		}
        // Otherwise if Aim Down Sight key has not been pressed and the tank is currently aiming down sight 
        else if (MainGunAimValue < 0 && isAimingDownSight)
		{
            // Reset aim down sight to false.
            Debug.Log("[TankMainGun.UpdateMainGun]: " + "Main Weapon has stopped aiming down sight!");
            isAimingDownSight = false;
		}
    }


    /// <summary>
    /// Called when the fire button has been released
    /// </summary>
    private void FireWeapon(bool ButtonReleased = false)
    {
        weaponHasFired = true; // we have fired our weapon
        // spawns in a tank shell at the main gun transform and matches the rotation of the main gun and stores it in the clone GameObject variable
        GameObject clone = Object.Instantiate(tankShellPrefab, mainGunTransform.position, mainGunTransform.rotation);

        // If the clone has a rigidbody, we want to add some velocity to it to make it fire!
        if(clone.GetComponent<Rigidbody>())
        {
            clone.GetComponent<Rigidbody>().velocity = m_currentFireVelocity * mainGunTransform.forward; // make the velocity of our bullet go in the direction of our gun at the launch force
        }
        Object.Destroy(clone,5f);
        
        WeaponSystemAudioSource.PlayOneShot(WeaponFire_SoundFX); // play the firing sound effect
        WeaponSystemAudioSource.Stop(); // stop charging up
        m_currentFireVelocity = minimumBulletVelocity;
        // only reset our weapon if we have released our fire button, don't allow it if we overcharged
        if (ButtonReleased)
        {
            weaponHasFired = false;
        }
    }

    /// <summary>
    ///     Called when the ADS button has been released 
    ///     TODO:
    /// </summary>
    /// <param name="ButtonReleased"></param>
    private void AimDownSight(bool ButtonReleased = false)
	{
        isAimingDownSight = true;

        // We would probably get the clone's camera here? Then use some sort of zoom effect 

        // Then reset after releasing the aim down sight button 

        if (ButtonReleased)
		{
            isAimingDownSight = false;
		}
	}
}
