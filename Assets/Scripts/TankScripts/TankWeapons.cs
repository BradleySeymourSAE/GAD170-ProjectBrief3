using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Primary, Secondary };   

public class TankWeapons : MonoBehaviour
{
  
	[Header("Weapon Settings")]
	public Transform[] Weapons; // array of weapon references 
	public GameObject tankShellPrefab; // tank shell prefab
	public GameObject turretShellPrefab; // turret bullet prefab 
	public int maxAllowedWeapons = 2;

	private List<Transform> weapons = new List<Transform>();

	[SerializeField] private WeaponData[] weaponData;

	private float currentBulletVelocity;
	private float currentReloadSpeed;
	private bool isFiringWeapon;
	private bool isAimingDownSight;

	private int currentlySelectedWeapon;
	
	private bool enableWeaponFiring; // should we be allowed to fire 
	private bool enableAimingDownSight; // should we be allowed to ads zoom 
	
	private Tank m_tankReference; // reference to the tank gameobject 


	public void Setup(Tank Tank)
	{
		m_tankReference = Tank;
		currentlySelectedWeapon = 0;

		weaponData = new WeaponData[maxAllowedWeapons];
		Weapons = new Transform[maxAllowedWeapons];
		
		if (weapons.Count > 0)
		{
			// Clear all weapon references 
			weapons.Clear();
		}

		for (int i = 0; i < Weapons.Length; i++)
		{
			Transform weapon = Weapons[i];

		

			weapons.Add(weapon);
		}
	}






	
}


[System.Serializable]
public struct WeaponData
{
	public WeaponType Weapon;
	public int WeaponID;
	public float FireRate;
	public float ReloadTime;
	public float Spread;
	public int totalAmmunitionCount;
	public int totalAmmunitionPerClip;
	public float minimumBulletVelocity;
	public float maximumBulletVelocity;
	public AudioSource WeaponAudioSource;
	public AudioClip ReloadSound;
	public AudioClip FireSound;


	/// <summary>
	///		Weapon Data Object 
	/// </summary>
	/// <param name="type"></param>
	/// <param name="_id"></param>
	/// <param name="fireRate"></param>
	/// <param name="reloadTime"></param>
	/// <param name="spread"></param>
	/// <param name="maxAmmoCount"></param>
	/// <param name="maxAmmoPerClip"></param>
	/// <param name="minimumVelocity"></param>
	/// <param name="maximumVelocity"></param>
	/// <param name="weaponAudio"></param>
	/// <param name="reload"></param>
	/// <param name="fire"></param>
	public WeaponData(WeaponType type, int _id, float fireRate, float reloadTime, float spread, int maxAmmoCount, int maxAmmoPerClip,float minimumVelocity,float maximumVelocity,AudioSource weaponAudio, AudioClip reload, AudioClip fire)
	{
		Weapon = type;
		WeaponID = _id;
		FireRate = fireRate;
		ReloadTime = reloadTime;
		Spread = spread;
		totalAmmunitionCount = maxAmmoCount;
		totalAmmunitionPerClip = maxAmmoPerClip;
		minimumBulletVelocity = minimumVelocity;
		maximumBulletVelocity = maximumVelocity;
		WeaponAudioSource = weaponAudio;
		ReloadSound = reload;
		FireSound = fire;
	}
}
