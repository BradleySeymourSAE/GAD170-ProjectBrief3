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
    public Transform barrelEnd; // reference to the main gun of the tank
    public GameObject tankShellPrefab; // reference to the tank prefab we want to fire

    public float minimumBulletSpeed = 50f; // the minimum amount of force for our weapon
    public float maximumBulletSpeed = 100f; // the maximum amount of force for our weapon
    public float maximumReloadTime = 0.25f; // the maximum amount of time we will allow to charge up and fire

    private const string PrimaryWeaponFireKey = "T90_PrimaryWeaponFire";
   
   [SerializeField] private float currentBulletVelocity; // the force we should use to fire our shell
   [SerializeField] private float currentReloadSpeed; // how fast we should charge up our weapon
   [SerializeField] private bool weaponHasFired; // have we just fired our weapon?
   [SerializeField] private bool weaponAimingDownSight; // are we aiming down sight 

    private bool enableWeaponFiring; // should we be allowed to fire?


    /// <summary>
    /// Sets up all the necessary variables for our main gun script
    /// </summary>
    public void Setup()
    {
        currentBulletVelocity = minimumBulletSpeed; // set our current launch force to the min
        currentReloadSpeed =  maximumReloadTime; // get the range between the max and min, and divide it by how quickly we charge
        
        
        EnableShooting(false); // disable shooting
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
    ///     Update's the Main Gun movement value 
    /// </summary>
    /// <param name="MainGunShootValue"></param>

    public void UpdateMainGun(float MainGunShootValue)
    {   
        if (enableWeaponFiring != true)
        {
            return; // don't do anything
        }

        if (MainGunShootValue > 0 && !weaponHasFired)
        {
            //Debug.Log("Weapon button pressed");
            Debug.Log("[TankMainGun.UpdateMainGun]: " + "Firing primary Weapon!");

			

            // Set the current fire velocity to a random value between minimum bullet velocity and maximum bullet velocity * time scaling factor
            currentBulletVelocity += Random.Range(minimumBulletSpeed, maximumBulletSpeed) * Time.deltaTime;
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
 }


    /// <summary>
    /// Called when the fire button has been released
    /// </summary>
    private void FireWeapon(bool ButtonReleased = false)
    {
        weaponHasFired = true; // we have fired our weapon
        // spawns in a tank shell at the main gun transform and matches the rotation of the main gun and stores it in the clone GameObject variable
        GameObject clone = Object.Instantiate(tankShellPrefab, barrelEnd.position, barrelEnd.rotation);

        // If the clone has a rigidbody, we want to add some velocity to it to make it fire!
        if(clone.GetComponent<Rigidbody>())
        {
            clone.GetComponent<Rigidbody>().velocity = currentBulletVelocity * barrelEnd.forward; // make the velocity of our bullet go in the direction of our gun at the launch force
        }


        Object.Destroy(clone,5f);
        if (AudioManager.Instance != null)
		{
            AudioManager.Instance.PlaySound(PrimaryWeaponFireKey);
		}

        currentBulletVelocity = minimumBulletSpeed;



        // Reset weapon after button release only 
        if (ButtonReleased)
        {
            weaponHasFired = false;
        }
    }
}
