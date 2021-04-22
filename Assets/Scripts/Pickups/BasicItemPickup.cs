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
	public GameItemData GameItem;

	/// <summary>
	///		The minimum amount of health points to add to the player 
	/// </summary>
	public float maximumHealthPoints = 25f;

	/// <summary>
	///		The minimum amount of ammunition rounds to give to a player 
	/// </summary>
	public int maximumAmmunitionRounds = 10;

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
		GameItem.startingPosition = transform.localPosition;
	}

	/// <summary>
	///		Called every frame, Checks if the item is enabled 
	/// </summary>
	private void Update()
	{
		if (!GameItem.Enabled)
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
	private void AllowItemPickup(bool Allow) => GameItem.Enabled = Allow;

	/// <summary>
	///		Enables the item 
	/// </summary>
	private void EnableItem() => AllowItemPickup(true);

	/// <summary>
	///		Handles the spin / Rotate of the Object 
	/// </summary>
	private void SpinAndRotate()
	{
		Vector3 spinningRotation = new Vector3(0f, Time.deltaTime * GameItem.RotationsPerSecond, 0f);
		// Spin object on the y axis 
		transform.Rotate(spinningRotation, Space.World);


		// Float the object up and down using a sin curve 
		GameItem.Offset = GameItem.startingPosition;
		GameItem.Offset.y += Mathf.Sin(Time.fixedTime * Mathf.PI * GameItem.Frequency) * GameItem.Amplitude;

		transform.position = GameItem.Offset;
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
			Debug.Log("[BasicItemPickup.OnTriggerEnter]: " + "Collided with Main Player - Item Type: " + GameItem.ItemType);
			
			//	Find the players transform component 
			Transform s_PlayerFound = col.transform;


			switch (GameItem.ItemType)
			{
				case GameItemType.Health:
					{ 
						float randomHealthAmount = Mathf.RoundToInt(Random.Range(GameItem.HP, maximumHealthPoints));
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
				case GameItemType.Ammunition:
					{ 
						int randomAmmunitionAmount = Random.Range(GameItem.Rounds, maximumAmmunitionRounds);
						Debug.Log("[BasicItemPickup.OnTriggerEnter]: " + "Current Ammunition: " + s_PlayerFound.GetComponent<MainPlayerTank>().Weapons.CurrentAmmunitionRemaining + "! Increasing by " + randomAmmunitionAmount);

						if (AudioManager.Instance)
						{
							// Play Ammunition Picked Up Audio Effect HAHAHA
							AudioManager.Instance.PlayAmmunitionPickupAudio();
						}

						// Invoke an event to increase the players ammunition 
						FireModeEvents.IncreasePlayerAmmunitionEvent?.Invoke(s_PlayerFound, randomAmmunitionAmount);
					}
					break;
			}


			FireModeEvents.HandleOnGameItemDestroyed?.Invoke(gameObject);

			Destroy(gameObject);
		}
	}
	#endregion


	#endregion
}
