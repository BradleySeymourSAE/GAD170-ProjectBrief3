using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		TODO: Need to finish implementing this 
/// </summary>
public class Pickup : MonoBehaviour
{

	public bool enableItemSpawning = false;


	/// <summary>
	/// 
	/// </summary>
	private void OnEnable()
	{
		WeaponPickupEvents.OnWeaponPickupCollectedEvent += CollectWeaponPickup;
		WeaponPickupEvents.OnGameStartedEvent += EnableItemSpawning;
	}
	/// <summary>
	/// 
	/// </summary>
	private void OnDisable()
	{
		WeaponPickupEvents.OnWeaponPickupCollectedEvent -= CollectWeaponPickup;
		WeaponPickupEvents.OnGameStartedEvent -= EnableItemSpawning;
	}

	// Called once 
	void Start()
	{
		
		// itemType.Setup(transform) 
		// specialItemType.Setup(transform) 
		


		if (enableItemSpawning)
		{
			EnableItemSpawning();
		}	
	}


	// Update is called every frame 
	private void Update()
	{
			
	}

	/// <summary>
	///		Called on game start to enable item spawning 
	/// </summary>
	private void EnableItemSpawning()
	{

	}

	private void CollectWeaponPickup(Transform Pickup)
	{

	}
}
