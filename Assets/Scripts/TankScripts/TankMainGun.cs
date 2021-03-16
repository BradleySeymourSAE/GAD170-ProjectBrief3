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

    public float minLaunchForce = 15f; // the minimum amount of force for our weapon
    public float maxLaunchForce = 30f; // the maximum amount of force for our weapon
    public float maxChargeTime = 0.75f; // the maximum amount of time we will allow to charge up and fire

    public Slider mainGunArrowIndicator; // a reference to the main gun slider

    private float currentLaunchForce; // the force we should use to fire our shell
    private float chargeSpeed; // how fast we should charge up our weapon
    private bool weaponFired; // have we just fired our weapon?

    public AudioSource weaponSystemSource; // reference to the audio source for the main gun
    public AudioClip chargingSFX; // a charging up sound
    public AudioClip firingSFX; // a firing weapon SFX.

    private bool enableShooting; // should we be allowed to fire?

    /// <summary>
    /// Sets up all the necessary variables for our main gun script
    /// </summary>
    public void SetUp()
    {
        currentLaunchForce = minLaunchForce; // set our current launch force to the min
        chargeSpeed = (maxLaunchForce - minLaunchForce) / maxChargeTime; // get the range between the max and min, and divide it by how quickly we charge
        mainGunArrowIndicator.minValue = minLaunchForce; // set the min and max programatically
        mainGunArrowIndicator.maxValue = maxLaunchForce;
        weaponSystemSource.clip = chargingSFX; // set the clip to the charging effect
        weaponSystemSource.loop = false; // don't set it to loop
        EnableShooting(false); // disable shooting
    }

    /// <summary>
    /// Called to enable/disable shooting
    /// </summary>
    /// <param name="Enabled"></param>
    public void EnableShooting(bool Enabled)
    {
        enableShooting = Enabled;
    }

    public void UpdateMainGun(float MainGunValue)
    {   
        if(enableShooting != true)
        {
            return; // don't do anything
        }

        if(currentLaunchForce >= maxLaunchForce && !weaponFired)
        {
            // if we are at max charge essentially and we haven't fired the weapon
            currentLaunchForce = maxLaunchForce;
            FireWeapon(); // fire our gun
        }
        // get the input from out main button press
        else if(MainGunValue > 0 && !weaponFired)
        {
            //Debug.Log("Weapon button pressed");

            // we want to charge up our weapon
            currentLaunchForce += chargeSpeed * Time.deltaTime; // increase the current force
            if(!weaponSystemSource.isPlaying)
            {
                weaponSystemSource.Play();// start playing the charging up sound effect
                Debug.Log("Charging");
            }
            // play a charging up sound effect
        }
        else if(MainGunValue < 0 && !weaponFired)
        {
           // Debug.Log("Weapon Button Released");
            // we've released our button
            // we want to fire our weapon
            FireWeapon(true);
        }
        else if(MainGunValue < 0 && weaponFired)
        {
            weaponFired = false;
        }
  
        mainGunArrowIndicator.value = currentLaunchForce; // set our arrow back to min at all times
    }


    /// <summary>
    /// Called when the fire button has been released
    /// </summary>
    private void FireWeapon(bool ButtonReleased = false)
    {
        weaponFired = true; // we have fired our weapon
        // spawns in a tank shell at the main gun transform and matches the rotation of the main gun and stores it in the clone GameObject variable
        GameObject clone = Object.Instantiate(tankShellPrefab, mainGunTransform.position, mainGunTransform.rotation);

        // If the clone has a rigidbody, we want to add some velocity to it to make it fire!
        if(clone.GetComponent<Rigidbody>())
        {
            clone.GetComponent<Rigidbody>().velocity = currentLaunchForce * mainGunTransform.forward; // make the velocity of our bullet go in the direction of our gun at the launch force
        }
        Object.Destroy(clone,5f);
        
        weaponSystemSource.PlayOneShot(firingSFX); // play the firing sound effect
        weaponSystemSource.Stop(); // stop charging up
        currentLaunchForce = minLaunchForce;
        // only reset our weapon if we have released our fire button, don't allow it if we overcharged
        if (ButtonReleased)
        {
            weaponFired = false;
        }
    }

}
