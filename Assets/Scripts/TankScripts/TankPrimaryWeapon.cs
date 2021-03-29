using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles firing the main weapon of the tank
/// </summary>
[System.Serializable]
public class TankPrimaryWeapon
{
    public Transform PrimaryWeaponFirePoint; // reference to the main gun of the tank
    public GameObject tankShellPrefab; // reference to the tank prefab we want to fire
   //  public GameObject tankBlastPrefab; // reference to second type of gun fire
   //  

    public float minimumBulletSpeed = 50f; // the minimum amount of force for our weapon
    public float maximumBulletSpeed = 100f; // the maximum amount of force for our weapon
    public float maximumChargeTimer = 0.25f; // the maximum amount of time we will allow to charge up and fire

    // public Slider MainWeaponArrowIndicator; // a reference to the main gun slider

   
   [SerializeField] private float currentBulletVelocity; // the force we should use to fire our shell
   [SerializeField] private float currentReloadSpeed; // how fast we should charge up our weapon
   [SerializeField] private bool weaponHasFired; // have we just fired our weapon?
   [SerializeField] private bool weaponAimingDownSight; // are we aiming down sight 

    [Header("Audio")]
    public AudioSource WeaponSystemAudioSource; // reference to the audio source for the main gun
    public AudioClip WeaponReloadSound; // a charging up sound
    public AudioClip WeaponFireSound; // a firing weapon SFX.

    private bool enableWeaponFiring; // should we be allowed to fire?
    private bool enableADSAiming; // should we be allowed to ads? 


    /// <summary>
    /// Sets up all the necessary variables for our main gun script
    /// </summary>
    public void SetUp()
    {
        currentBulletVelocity = minimumBulletSpeed; // set our current launch force to the min
        currentReloadSpeed = (maximumBulletSpeed - minimumBulletSpeed) / maximumChargeTimer; // get the range between the max and min, and divide it by how quickly we charge
        
        
        // TODO: At the moment I am switch around the UI for the Tank Object 
        // so i have commented out the weapon arrow indicator slider 
       // I will be replacing this with some sort of aim reticle of some sort :) 
       //  MainWeaponArrowIndicator.minValue = minimumBulletVelocity; // set the min and max programatically
       // MainWeaponArrowIndicator.maxValue = maximumBulletVelocity;
        
        
        WeaponSystemAudioSource.clip = WeaponReloadSound; // set the clip to the charging effect
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
        enableWeaponFiring = Enabled;
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
        if (enableWeaponFiring != true)
        {
            return; // don't do anything
        }

        if (MainGunShootValue > 0 && !weaponHasFired)
        {
            //Debug.Log("Weapon button pressed");
            Debug.Log("[TankMainGun.UpdateMainGun]: " + "Firing primary Weapon!");

            // Charge the main weapon 
            // Set the current fire velocity to the reload speed multiplied by the time factor.
            currentBulletVelocity += currentReloadSpeed * Time.deltaTime; 


            // If the weapon system audio system isn't currently playing the audio
            if (!WeaponSystemAudioSource.isPlaying)
            {
                // Then play the Weapon Fire Audio FX from the Weapon System Audio source
                WeaponSystemAudioSource.Play();
                Debug.Log("[TankMainGun.UpdateMainGun]: " + "Reloading primary Weapon!");
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
       // MainWeaponArrowIndicator.value = m_currentFireVelocity; 

       
        // If aim down sight is not enabled 
        if (enableADSAiming != true)
		{
            // We want to return.
            return;
		}
        
        
        // If the Main Gun ADS value has been pressed and the tank is currently not aiming down sight 
        if (MainGunAimValue > 0 && !weaponAimingDownSight)
		{
            // Then we want to call the Aim Down Sight function 
            Debug.Log("[TankMainGun.UpdateMainGun]: " + "Main Weapon is now aiming down sight!");
            AimDownSight();
		}
        else if (MainGunAimValue < 0 && !weaponAimingDownSight)
		{
            Debug.Log("[TankMainGun.UpdateMainGun]: " + "Main weapon is still aiming down sight");
            AimDownSight(true);
		}
        // Otherwise if Aim Down Sight key has not been pressed and the tank is currently aiming down sight 
        else if (MainGunAimValue < 0 && weaponAimingDownSight)
		{
            // Reset aim down sight to false.
            Debug.Log("[TankMainGun.UpdateMainGun]: " + "Main Weapon has stopped aiming down sight!");
            weaponAimingDownSight = false;
		}
    }


    /// <summary>
    /// Called when the fire button has been released
    /// </summary>
    private void FireWeapon(bool ButtonReleased = false)
    {
        weaponHasFired = true; // we have fired our weapon
        // spawns in a tank shell at the main gun transform and matches the rotation of the main gun and stores it in the clone GameObject variable
        GameObject clone = Object.Instantiate(tankShellPrefab, PrimaryWeaponFirePoint.position, PrimaryWeaponFirePoint.rotation);

        // If the clone has a rigidbody, we want to add some velocity to it to make it fire!
        if(clone.GetComponent<Rigidbody>())
        {
            clone.GetComponent<Rigidbody>().velocity = currentBulletVelocity * PrimaryWeaponFirePoint.forward; // make the velocity of our bullet go in the direction of our gun at the launch force
        }
        Object.Destroy(clone,5f);
        
        WeaponSystemAudioSource.PlayOneShot(WeaponFireSound); // play the firing sound effect
        WeaponSystemAudioSource.Stop(); // stop charging up
        currentBulletVelocity = minimumBulletSpeed;
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
        weaponAimingDownSight = true;

        // We would probably get the clone's camera here? Then use some sort of zoom effect 

        // Then reset after releasing the aim down sight button 

        if (ButtonReleased)
		{
            weaponAimingDownSight = false;
		}
	}
}
