using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
/// <summary>
///		Basic Collectable Items Class 
/// </summary>
public class BasicItemPickup : MonoBehaviour
{
	/// <summary>
	///		Public Object for holding collectable Item data 
	/// </summary>
	public CollectableItemData CollectableItem;

	#region Unity References 
	
	private void OnEnable()
	{
		FireModeEvents.GameStartedEvent += EnableItem;
	}

	private void OnDisable()
	{
		FireModeEvents.GameStartedEvent -= EnableItem;
	}

	/// <summary>
	///		Called before the first frame update 
	/// </summary>
	private void Start()
	{
		CollectableItem.startingPosition = transform.position;
	}

	/// <summary>
	///		Called every frame, Checks if the item is enabled 
	/// </summary>
	private void Update()
	{
		if (!CollectableItem.Enabled)
		{
			return;
		}

		SpinAndRotate();
	}

	#endregion

	#region Private Methods 
	
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


	#region Triggers 
	/// <summary>
	///		Handles what happens when a player is inside the trigger zone 
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerEnter(Collider other)
	{
		
		if (other.gameObject.tag == "Player" || other.gameObject.GetComponent<MainPlayerTank>())
		{
			Debug.Log("Collided with player object - Doing the thing!");
			
			MainPlayerTank _player = other.gameObject.GetComponent<MainPlayerTank>();

			Debug.Log("[BasicItemPickup.OnTriggerEnter]: " + "On Object picked up event called!!!");
			

			Debug.Log("[BasicItemPickup.OnTriggerEnter]: " + "Players current Ammo " + _player);


		}
	}
	#endregion


	#endregion
}
