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

	/// <summary>
	///		The minimum amount of health points to add to the player 
	/// </summary>
	public float minimumHealthPoints = 25f;

	/// <summary>
	///		The minimum amount of ammunition rounds to give to a player 
	/// </summary>
	public int minimumAmmunitionRounds = 10;

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
		CollectableItem.startingPosition = transform.localPosition;
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
	private void OnTriggerEnter(Collider col)
	{
		
		if (!col.gameObject.GetComponent<MainPlayerTank>())
		{
			//	Unless its the player, we want to return.
			return;
		}

		if (col.gameObject.tag == "Player" && col.gameObject.GetComponent<MainPlayerTank>())
		{
			Debug.Log("[BasicItemPickup.OnTriggerEnter]: " + "Collided with a player - Item Type: " + CollectableItem.ItemType);
			
			//	Find the players transform component 
			Transform s_PlayerFound = col.gameObject.transform;


			switch (CollectableItem.ItemType)
			{
				case SpecialItemPickup.Health:
					{ 
						float randomHealthAmount = Random.Range(minimumHealthPoints, CollectableItem.HP);
						Debug.Log("[BasicItemPickup.OnTriggerEnter]: " + "Player Current Health: " + s_PlayerFound.GetComponent<MainPlayerTank>().Health.PlayersCurrentHealth + "! Increasing players health by " + randomHealthAmount);
						
						
						// Try play the audio clip! 
						if (AudioManager.Instance)
						{
							// Play Health Pickup up Audio Effect! HAHAHA
							AudioManager.Instance.PlayHealthPickupAudio();
						}
						
						// Invoke an event to increase the players health 
						FireModeEvents.IncreasePlayerHealthEvent?.Invoke(s_PlayerFound, randomHealthAmount);
					}
					break;
				case SpecialItemPickup.Ammunition:
					{ 
						int increaseAmmoAmount = Random.Range(minimumAmmunitionRounds, CollectableItem.Rounds);
						Debug.Log("[BasicItemPickup.OnTriggerEnter]: " + "Current Ammunition: " + s_PlayerFound.GetComponentInChildren<MainPlayerTank>().Weapons.CurrentAmmunitionRemaining + "! Increasing by " + increaseAmmoAmount);

						if (AudioManager.Instance)
						{
							// Play Ammunition Picked Up Audio Effect HAHAHA
							AudioManager.Instance.PlayAmmunitionPickupAudio();
						}

						// Invoke an event to increase the players ammunition 
						FireModeEvents.IncreasePlayerAmmunitionEvent?.Invoke(s_PlayerFound, increaseAmmoAmount);
					}
					break;
			}


			// Could instantiate a item pickup effect here?

			gameObject.SetActive(false);


			FireModeEvents.HandleOnGameItemDestroyed?.Invoke(gameObject);
		}
	}
	#endregion


	#endregion
}
