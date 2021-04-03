using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TankAI_Weapon
{
	public Transform FirePoint;
	public GameObject shellPrefab;

	public enum Difficulty { Easy, Hard };
	public Difficulty difficulty = Difficulty.Easy;

	private float bulletSpeed = 100f;
	private float reloadSpeed = 10;
	private float totalAmmunition = 100;
	private float ammunitionPerClip = 1;

	private int reloadTimeFactor = 3;

	[SerializeField] private float currentVelocity;
	[SerializeField] private float currentReload;
	[SerializeField] private float currentAmmoRemaining;
	[SerializeField] private float totalAmmoRemaining;

	[SerializeField] private bool isReloadingWeapon;
	[SerializeField] private bool isFiringWeapon;
	
	
	[SerializeField] private bool isEnabledAI;

	private Transform tankReference;


	public void Setup(Transform TankReference)
	{
		tankReference = TankReference;


		currentVelocity = Random.Range(bulletSpeed - 25f, bulletSpeed + 100f);
		currentReload = difficulty == Difficulty.Easy ? currentReload * reloadTimeFactor : currentReload;

		totalAmmoRemaining = totalAmmunition - ammunitionPerClip;
		currentAmmoRemaining = ammunitionPerClip;
		
		isReloadingWeapon = false;
		EnableAIWeaponFiring(false);
	}

	public void EnableAIWeaponFiring(bool EnableAI)
	{
		isEnabledAI = EnableAI;
	}

	
	/// <summary>
	///		Handles ai weapon reload 
	/// </summary>
	/// <returns></returns>
	IEnumerator Reloading()
	{
		isReloadingWeapon = true;


		if (AudioManager.Instance != null)
		{
			AudioManager.Instance.PlaySound(GameAudio.T90_PrimaryWeapon_Reload);
		}



		yield return new WaitForSeconds(currentReload);



		currentAmmoRemaining = ammunitionPerClip;
		totalAmmoRemaining -= currentAmmoRemaining;


		isReloadingWeapon = false;
	}
}