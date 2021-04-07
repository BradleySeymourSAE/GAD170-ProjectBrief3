using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InfantryWeapons
{

	public Transform BarrelEnd;
	public GameObject bulletPrefab;
	public GameObject muzzleFlashPrefab;
	
	public float bulletSpeed;
	public float bulletSpread;
	public float bulletRange;
	public int maximumAmmunitionPerClip = 30;
	public float reloadSpeed = 2.5f; // seconds in time for reload 


	[SerializeField] private float currentBulletVelocity;
	[SerializeField] private float currentBulletSpread;
	[SerializeField] private float currentWeaponRange;
	[SerializeField] private float currentAmmunition;
	[SerializeField] private float currentReloadSpeed;
	[SerializeField] private bool weaponFired;
	[SerializeField] private bool weaponReloading;
	[SerializeField] private float destroyBulletTimer = 5f;

	[SerializeField] private bool enableWeaponFiring;



	public void Setup(Transform AI)
	{

		currentBulletVelocity = bulletSpeed;
		currentBulletSpread = Random.Range(0, bulletSpread);
		currentWeaponRange = Random.Range(bulletRange - 10f, bulletRange + 5f);
		currentAmmunition = maximumAmmunitionPerClip;
		currentReloadSpeed = Random.Range(reloadSpeed - 1f, reloadSpeed + 1f);
		weaponReloading = false;
		EnableWeaponFiring(false);
	}

	/// <summary>
	///		Enable / Disable Weapon Fire
	/// </summary>
	/// <param name="EnableWeapon"></param>
	public void EnableWeaponFiring(bool EnableWeapon)
	{
		enableWeaponFiring = EnableWeapon;
	}
	
	public void UpdateWeapon(float FiringWeapon)
	{
		if (enableWeaponFiring != true || weaponReloading == true)
		{
			return;
		}
		
		if (FiringWeapon > 0 && !weaponFired)
		{
			Debug.Log("[InfantryWeapon.UpdateWeapon]: " + "Firing main weapon!");
		}


	}

	/// <summary>
	///		Handles reloading for the weapon 
	/// </summary>
	/// <returns></returns>
	IEnumerator ReloadWeapon()
	{
		weaponReloading = true;

		if (AudioManager.Instance)
		{
			AudioManager.Instance.PlaySound(GameAudio.AEK971_PrimaryWeapon_Reload);
		}

		yield return new WaitForSeconds(currentReloadSpeed);

		currentAmmunition = maximumAmmunitionPerClip;
		
		weaponReloading = false;
	}


	/// <summary>
	///		Handles firing of the weapon 
	/// </summary>
	private void FireWeapon()
	{
		weaponFired = true;

		GameObject cloneBullet = Object.Instantiate(bulletPrefab, BarrelEnd.position, BarrelEnd.rotation);

		// Get the rigidbody attached to the bullet prefab 

		if (cloneBullet.GetComponent<Rigidbody>())
		{
			Rigidbody bulletRigidbody = cloneBullet.GetComponent<Rigidbody>();


			bulletRigidbody.velocity = currentBulletVelocity * BarrelEnd.forward;
		}



		Object.Destroy(cloneBullet, destroyBulletTimer);

		currentBulletVelocity = Random.Range(bulletSpeed - 10f, bulletSpeed + 5f);

		currentAmmunition--;

		if (AudioManager.Instance)
		{
			AudioManager.Instance.PlaySound(GameAudio.AEK971_PrimaryWeapon_Fire);
		}



		weaponFired = false;
	}

}