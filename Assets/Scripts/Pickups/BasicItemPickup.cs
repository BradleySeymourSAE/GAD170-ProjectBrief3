using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Item { Ammunition, Health };


public class BasicItemPickup : MonoBehaviour
{

	public CollectableItemData CollectableItem;



	private void OnEnable()
	{
		FireModeEvents.OnWaveStartedEvent += EnableItem;
	}

	private void OnDisable()
	{
		FireModeEvents.OnWaveStartedEvent -= EnableItem;
	}


	private void Start()
	{
		CollectableItem.startingPosition = transform.position;
	}


	/// <summary>
	///		Enables / Disables pickup of the item
	/// </summary>
	/// <param name="Allow"></param>
	private void AllowItemPickup(bool Allow) => CollectableItem.Enabled = Allow;


	/// <summary>
	///		Enables the item 
	/// </summary>
	private void EnableItem()
	{
		AllowItemPickup(true);
	}


	private void Update()
	{
		if (!CollectableItem.Enabled)
		{
			return;
		}
		
		SpinAndRotate();
	}



	/// <summary>
	///		Handles the spin / Rotate of the Object 
	/// </summary>
	private void SpinAndRotate()
	{
		Vector3 _rotate = new Vector3(0f, Time.deltaTime * CollectableItem.RotationsPerSecond, 0f);
		// Spin object on the y axis 
		transform.Rotate(_rotate, Space.World);


		// Float the object up and down using a sin curve 
		CollectableItem.Offset = CollectableItem.startingPosition;
		CollectableItem.Offset.y += Mathf.Sin(Time.fixedTime * Mathf.PI * CollectableItem.Frequency) * CollectableItem.Amplitude;

		transform.position = CollectableItem.Offset;
	}


	/// <summary>
	///		Handles what happens when a player is inside the trigger zone 
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerEnter(Collider other)
	{
		
		if (other.gameObject.CompareTag("Player"))
		{
			Debug.Log("Collided with player object - Doing the thing!");
		}
	}
}
