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
    public float maximumReloadTime = 5f; // the maximum amount of time we will allow to charge up and fire
    public float maximumAmmunition = 20; // the maximum amount of ammunition in a tank 
    public float maximumAmmunitionPerClip = 1; // the maximum amount of ammunition per shot 

   [SerializeField] private float currentBulletVelocity; // the force we should use to fire our shell
   [SerializeField] private float currentReloadSpeed; // how fast we should charge up our weapon
   [SerializeField] private float currentAmmunitionLoaded; // current ammunition remaining in primary weapon 
   [SerializeField] private float totalAmmunitionRemaining; // total ammunition remaining 

   [SerializeField] private bool weaponReloading; // are we currently reloading?
   [SerializeField] private bool weaponHasFired; // have we just fired our weapon?

   [SerializeField] private bool enableWeaponFiring; // should we be allowed to fire?
   [SerializeField] private float nextTimeToFire;

    private Transform m_tankReference;




    /// <summary>
    ///  Returns the current reload speed for the tank 
    /// </summary>
    public float returnReloadSpeed
	{
        get
		{
            return currentReloadSpeed;
		}
	}

    /// <summary>
    ///     Gets the currently loaded ammunition for the tank
    /// </summary>
    public string ReturnCurrentLoadedAmmunition
	{
        get
		{
            return currentAmmunitionLoaded.ToString() + "/" + totalAmmunitionRemaining.ToString();
        }
	}


    /// <summary>
    /// Sets up all the necessary variables for our main gun script
    /// </summary>
    public void Setup(Transform Tank)
    {
        m_tankReference = Tank;


        currentBulletVelocity = minimumBulletSpeed; // set our current launch force to the min
        currentReloadSpeed =  maximumReloadTime; // get the range between the max and min, and divide it by how quickly we charge
        totalAmmunitionRemaining = maximumAmmunition - maximumAmmunitionPerClip; // max ammo is 19 (20 - 1)
        currentAmmunitionLoaded = maximumAmmunitionPerClip;  // set the current chamber ammo to 1

        // Set the weapon to not currently reloading 
        weaponReloading = false;
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
    ///     Update's the Main Gun shooting value 
    /// </summary>
    /// <param name="MainGunShootValue"></param>
    public void UpdateMainGun(float MainGunShootValue)
    {   
        if (enableWeaponFiring != true)
        {
            Debug.Log("[TankPrimaryWeapon.UpdateMainGun]: " + "Weapon cant be fired");
            return; // don't do anything
        }

        // We want to return if the weapon is reloading 
        if (weaponReloading == true)
		{
            return;
		}

        // If fire weapon key has been pressed and the weapon isn't currently firing 
        if (MainGunShootValue > 0 && !weaponHasFired)
		{

            // If the total ammo we have is less than or equal to 0 and there isnt any ammo in the chamber
            if (totalAmmunitionRemaining <= 0 && currentAmmunitionLoaded <= 0)
            {
                totalAmmunitionRemaining = 0;
                currentAmmunitionLoaded = 0;
                Debug.Log("[TankPrimaryWeapon.UpdateMainGun]: " + "You have run out of ammunition! Ammo in Primary: " + currentAmmunitionLoaded + "Total Ammunition Left: " + totalAmmunitionRemaining);
                // Then we want to return.
                return;
            }

            // If there is no ammunition left in the chamber we want to try and reload 
            // EDIT: This is actually just useless code, as we are reloading straight after firing the primary weapon. 
            // However, this could be moreso useful for the turret weapon? 

            Debug.Log("[TankPrimaryWeapon.UpdateMainGun]: " + "Firing primary weapon!");
            FireWeapon();


            // If the weapon has fired we want to reload. 
            if (weaponHasFired)
			{
                // Otherwise we can fire the primary weapon
                Debug.Log("[TankPrimaryWeapon.UpdateMainGun]: " + "Weapon has fired! Reloading primary weapon...");
                m_tankReference.GetComponent<Tank>().StartCoroutine(ReloadWeapon());
			}
		}
        else if (MainGunShootValue <= 0 && weaponHasFired)
		{
            weaponHasFired = false;
		}
    
 }

	#region Private Weapon Methods
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
            Rigidbody cloneRb = clone.GetComponent<Rigidbody>();

            cloneRb.velocity = currentBulletVelocity * barrelEnd.forward; // make the velocity of our bullet go in the direction of our gun at the launch force 
        }


        Object.Destroy(clone,5f);
        if (AudioManager.Instance != null)
		{
            AudioManager.Instance.PlaySound(GameAudio.T90_PrimaryWeapon_Fire);
		}

        // Reset the bullet velocity 
        currentBulletVelocity = Random.Range(minimumBulletSpeed, maximumBulletSpeed);

        // Decrease the amount of ammunition remaining 
        currentAmmunitionLoaded--;

        

        // Reset weapon after button release only 
        if (ButtonReleased)
        {
            weaponHasFired = false;
        }
    }
    

    // TODO: Could also add the weapon fire here aswell as an IEnumerator ? 

    /// <summary>
    ///     Handles the tanks primary weapon reload 
    /// </summary>
    /// <returns></returns>
    IEnumerator ReloadWeapon()
	{
        weaponReloading = true;

        // Check if audio instance is not null & play the reloading sound 
        if (AudioManager.Instance != null)
        {
            // If the audio manager instance isnt null play the weapon reload sound 
            AudioManager.Instance.PlaySound(GameAudio.T90_PrimaryWeapon_Reload);
        }

        // Wait X amount of reload seconds 
        yield return new WaitForSeconds(currentReloadSpeed);

       
        currentAmmunitionLoaded = maximumAmmunitionPerClip; // set current ammunition in clip to max ammo in clip 
        totalAmmunitionRemaining -= currentAmmunitionLoaded;
      

    
        weaponReloading = false;
     }

	#endregion
}
