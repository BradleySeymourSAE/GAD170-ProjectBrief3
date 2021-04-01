using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		Handles Infantry Weapon for the Enemy AI Characters 
/// </summary>
[System.Serializable]
public class InfantryAI_Weapon
{ 

	private const string AEKWeaponFireKey = "AEK971WeaponFire";

	public Transform FiringPoint;  // reference to the firing point on the weapon 
	public GameObject BulletPrefab; // reference to the bullet prefab 

	public float bulletVelocity = 300f;
	public float reloadWaitTimer = 2f;
	public float bulletSpread = 0.1f;


	private float m_currentBulletVelocity;
	private float m_currentReloadSpeed;
	private bool weaponIsFiring;


	private bool enableWeaponFiring;
	private bool enableAggression;

	/// <summary>
	///		Handles the setup of the AI Infantry's weapon 
	/// </summary>
	public void Setup()
	{
		m_currentBulletVelocity = bulletVelocity;
		m_currentReloadSpeed = reloadWaitTimer;

		EnableFiring(false); // disable firing by default 
	}

	/// <summary>
	///		Enables firing of the Weapon 
	/// </summary>
	/// <param name="Enabled"></param>
	public void EnableFiring(bool Enabled)
	{
		enableWeaponFiring = Enabled;
	}

	/// <summary>
	///		Enables / Disable AI Aggression
	/// </summary>
	/// <param name="Enabled"></param>
	public void EnableAggression(bool Enabled)
	{
		enableAggression = Enabled;
	}


	public void UpdateWeapon(float ShouldFireWeapon)
	{
		if (enableWeaponFiring != true || enableAggression != true)
		{
			return;
		}


		if (ShouldFireWeapon > 0 && !weaponIsFiring)
		{
			m_currentBulletVelocity += Random.Range(bulletVelocity - 10f, bulletVelocity + 10f) * Time.deltaTime;
		}
		else if (ShouldFireWeapon < 0 && weaponIsFiring)
		{
			weaponIsFiring = false;
		}
	}


	private void FireWeapon(bool FiringWeapon = false)
	{
		weaponIsFiring = true;

		GameObject bulletClone = GameObject.Instantiate(BulletPrefab, FiringPoint.position, FiringPoint.rotation);


		if (bulletClone.GetComponent<Rigidbody>())
		{
			bulletClone.GetComponent<Rigidbody>().velocity = m_currentBulletVelocity * FiringPoint.forward; // velocity of bullet 
		}

		
	   Object.Destroy(bulletClone, 6f);


		if (AudioManager.Instance != null)
		{
			AudioManager.Instance.PlaySound(AEKWeaponFireKey);
		}


		// Add variation to the current bullet velocity 
		m_currentBulletVelocity = Random.Range(bulletVelocity - 10f, bulletVelocity + 10f);
	
	
		if (FiringWeapon)
		{
			weaponIsFiring = false;
		}
	}
}
