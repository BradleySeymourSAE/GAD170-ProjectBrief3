using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///		Main Collectable Item Pickup Class 
/// </summary>
public class CollectableItemsManager : MonoBehaviour
{
   
	public bool enableItemPickup = false; // can the item be picked up 
	public bool enableSpecialItems = false; // enable 'special items' (Floating? idk - screw gravity HAHA)

	// TODO - Need to update the collectable items manager pronto 

	/// <summary>
	///		OnEnable Event Methods for the Collectable Item
	/// </summary>
	private void OnEnable()
	{
		// OnCollectableSpawnEvent += SpawnItems;
		// OnCollectableDestroyedEvent += DespawnItems;
	}

	/// <summary>
	///		OnDisable Event Methods for the Collectable Item
	/// </summary>
	private void OnDisable()
	{
		// OnCollectableSpawnEvent -= SpawnItems;
		// OnCollectableDestroyedEvent -= DespawnItems;
	}


	/// <summary>
	///		Called when the collectable items are spawned
	/// </summary>
	/// <param name="spawnedCollectableItems"></param>
	private void SpawnedCollectableItems(List<GameObject> spawnedCollectableItems)
	{

	}

	/// <summary>
	/// Called when a collectable item is despawned 
	/// </summary>
	/// <param name="collectableItemsDespawned"></param>
	private void DespawnedCollectableItems(Transform collectableItemsDespawned)
	{

	}
}
