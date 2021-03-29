using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]

public class TankSecondaryWeapon
{
	public Transform TurretFirePoint; // reference to the turret gun of the tank 
	public GameObject turretBulletPrefab; // reference to the projectile we want to fire 
	
	[SerializeField] private float destroyBulletTime = 5f; // seconds to wait until destroying the bullet object instance. 

	[Header("Weapon Settings")]
	public float minimumBulletSpeed = 60f; // minimum amount of force for our bullet 
	public float maximumBulletSpeed = 120f; // maximum amount of force for our bullet
	public float bulletSpread = 0.1f;

	[Header("Ammunition Settings")]
	public float maximumAmmunitionPerClip = 50; // maximum amount of ammunition per clip 
	public float overheatingWaitPeriod = 3f; // maximum time to reload the weapon 


	// Current variables 
	[SerializeField] private float m_currentBulletSpeed; // bullet travel speed (velocity) 
	[SerializeField] private float overheatingTime; // how long it takes to reload (overheating time period)
	[SerializeField] private float ammunitionRemaining; // the amount of ammunition remaining


	// States 
	[SerializeField] private bool isWeaponFiring; // have we fired the turret? 
	[SerializeField] private bool isWeaponAimingDownSight; // are we aiming down sight 
	[SerializeField] private bool isWeaponOverheating; // is the turret overheating? 
	

	[Header("Audio Settings")]
	public AudioSource TurretSystemAudioSource; // reference to the audio source for the turret 
	public AudioClip OverheatingSoundEffect; // reload sound fx 
	public AudioClip TurretFireSoundEffect; // turret fire sound fx 

	private Tank m_tankReference; // reference to the tank 
	private bool enableWeaponFiring; // can we fire the secondary weapon 
	private bool enableAimDownSightZooming; // can we aim down sight with secondary weapon
	private bool enableWeaponOverheating; // can the weapon be reloading 

	#region Public Methods 
	/// <summary>
	///		Sets up the starting variables for our tanks secondary weapon 
	/// </summary>
	public void Setup(Tank Tank)
	{
		m_tankReference = Tank;

		m_currentBulletSpeed = minimumBulletSpeed; // Set the current bullet speed to the minimum bullet velocity speed.
		overheatingTime = overheatingWaitPeriod; // set the reloading speed to the maximum reloading time 
		ammunitionRemaining = maximumAmmunitionPerClip; // set the current ammunition value to ammunition per clip 


		TurretSystemAudioSource.clip = OverheatingSoundEffect; // Set the default clip to the reload sound fx. 
		TurretSystemAudioSource.loop = false; // we dont want this clip to look on default. 
		
		EnableSecondaryWeaponFiring(false); // Disable Secondary Weapon Firing 
		EnableAimDownSight(false); // Disable Aim Down Sight for secondary weapon 
		EnableWeaponOverheat(false); // Disable Reloading for secondary weapon 
	}

	/// <summary>
	///		Enable / Disable Secondary Weapon Firing 
	/// </summary>
	/// <param name="Enable"></param>
	public void EnableSecondaryWeaponFiring(bool Enabled)
	{
		enableWeaponFiring = Enabled;
	}

	/// <summary>
	///		Enable Aim Down Sight Secondary Weapon Zoom
	/// </summary>
	/// <param name="Enabled"></param>
	public void EnableAimDownSight(bool Enabled)
	{
		enableAimDownSightZooming = Enabled;
	}
	
	/// <summary>
	///		Enable / Disable Overheating of the weapon 
	/// </summary>
	/// <param name="Enabled"></param>
	public void EnableWeaponOverheat(bool Enabled)
	{
		enableWeaponOverheating = Enabled;
	}
	/// <summary>
	///		Updates Turrets Movement Value 
	/// </summary>
	/// <param name="SecondaryWeaponShootingValue"></param>
	/// <param name="SecondaryWeaponAimValue"></param>
	public void UpdateSecondaryWeapon(float SecondaryWeaponShootingValue, float SecondaryWeaponAimValue)
	{
		// If the secondary weapon is not enabled or enable down sight zooming is not enabled then we want to return.
		if (enableWeaponFiring != true || enableAimDownSightZooming != true || enableWeaponOverheating != true)
		{
			return;
		}


		// --- TURRET FIRING ---
		// If the tank is trying to shoot and weapon has not yet fired 
		if (SecondaryWeaponShootingValue > 0 && !isWeaponFiring)
		{
			
			bool canFireWeapon;
			
			// If there is no ammunition remaining
			if (ammunitionRemaining <= 0)
			{
				// The weapon is overheating, Start a coroutine 
				Debug.Log("[TankSecondaryWeapon.UpdateSecondaryWeapon]: " + "Weapon is overheating!");
				m_tankReference.StartCoroutine(WeaponOverheating());
			}
			

			// We can fire the weapon is the weapon is NOT overheating.
			canFireWeapon = !isWeaponOverheating;


			if (canFireWeapon == true)
			{ 
				Debug.Log("[TankSecondaryWeapon.UpdateSecondaryWeapon]: " + "Can fire secondary weapon? " + canFireWeapon);
				FireSecondaryWeapon(canFireWeapon);
			}
		}
		// Otherwise if secondary weapon fire value is less than or equal to 0 and turret is firing 
		else if (SecondaryWeaponShootingValue <= 0 && isWeaponFiring)
		{
			// Reset turret firing to false.
			isWeaponFiring = false;
		}


		// --- ADS AIMING ----
		// If the turret aim down sight value has been pressed and the tank is not currently aiming down sight
		if (SecondaryWeaponAimValue > 0 && !isWeaponAimingDownSight)
		{
			// Then we want to aim down sight 
			Debug.Log("[TankSecondaryWeapon.UpdateSecondaryWeapon]: " + "Turret is aiming down sight!");
			AimDownSight(true);
		}
		// Otherwise if aim down sight key has not been pressed and tank is aiming down sight 
		else if (SecondaryWeaponAimValue < 0 && isWeaponAimingDownSight)
		{
			Debug.Log("[TankSecondaryWeapon.UpdateUpdateSecondaryWeaponMainGun]: " + "Turret has stopped aiming down sight!");
			isWeaponAimingDownSight = false;
		}
	}
	#endregion

	#region Private Methods 
	/// <summary>
	///		Handles the firing of the Tanks Turret 
	/// </summary>
	/// <param name="ButtonReleased"></param>
	private void FireSecondaryWeapon(bool ButtonReleased = false)
	{
		isWeaponFiring = true; // secondary weapon is being fired 

		// Spawns in a bullet at the turrets fire point 
		GameObject clonedBullet = Object.Instantiate(turretBulletPrefab, TurretFirePoint.position, TurretFirePoint.rotation); 

		// Apply bullet spread rotation, more movement = more spread 
		clonedBullet.transform.Rotate(0f, 0f, Random.Range(-bulletSpread, bulletSpread));

		// Check if the bullet has a rigidbody, we want to add some velocity to the rigidbody to make it shoot!
		if (clonedBullet.GetComponent<Rigidbody>())
		{
			// Add Force to the rigidbody 
			// Add velocity of the bullets go in the direction of our turret at the bullet speed velocity that we have set .
			clonedBullet.GetComponent<Rigidbody>().velocity = m_currentBulletSpeed * TurretFirePoint.forward; 
		}

		ammunitionRemaining--;

		// Destroyed the cloned bullet at the time set .
		Object.Destroy(clonedBullet, destroyBulletTime);


		// Play the weapon firing sound 
		TurretSystemAudioSource.PlayOneShot(TurretFireSoundEffect);
			
		// Reset the current fire velocity to a random range within minimum and maximum velocity 
		m_currentBulletSpeed = Random.Range(minimumBulletSpeed, maximumBulletSpeed);


		// Reset the weapon if the button has been released, otherwise continue 
		if (ButtonReleased)
		{
			isWeaponFiring = false;
		}
	}

	/// <summary>
	///		Handles ADS Aiming when ADS Button has been pressed 
	/// </summary>
	/// <param name="ButtonReleased"></param>
	private void AimDownSight(bool ButtonReleased = false)
	{
		isWeaponAimingDownSight = true;

		// Get camera from here and set some sort of zoom effect 

		// Change the UI for our aiming reticle
		 


		// If the aiming down sight button is released we want to reset the turrets ads 
		if (ButtonReleased)
		{
			isWeaponAimingDownSight = false;
		}
	}

	/// <summary>
	///		Handles Reload of Weapon when Reload Button has been pressed 
	/// </summary>
	/// <param name="ButtonPressed"></param>
	private IEnumerator WeaponOverheating()
	{
		
		isWeaponOverheating = true;


		yield return new WaitForSeconds(overheatingTime);


		ammunitionRemaining = maximumAmmunitionPerClip;


		isWeaponOverheating = false;
	}
	#endregion
}
