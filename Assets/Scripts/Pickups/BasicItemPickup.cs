using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Item { Ammunition, Health };


public class BasicItemPickup : MonoBehaviour
{

	public float rotationPerSecond = 60f; // rotation of coin per second 
	public float amplitude = 0.15f; // aplitude (of y axis)
	public float frequency = 1.50f; // amount of times 
	public Item itemType = Item.Health; // set the current item to health as default 

	[SerializeField] private Item currentItem; // Check what the current item is 

	Vector3 m_positionOffset = new Vector3(); // offset start pos
	Vector3 m_movePosition = new Vector3(); // default pos 

	private bool itemEnabled = false;

	private void OnEnable()
	{
		FireModeEvents.OnWaveStartedEvent += OnEnableItem;
	}

	private void OnDisable()
	{
		FireModeEvents.OnWaveStartedEvent -= OnEnableItem;
	}


	private void Start()
	{
		m_positionOffset = transform.position;
	}


	/// <summary>
	///		Enables / Disables pickup of the item
	/// </summary>
	/// <param name="Allow"></param>
	private void AllowItemPickup(bool Allow)
	{
		itemEnabled = Allow;
	}


	private void OnEnableItem()
	{
		AllowItemPickup(true);
	}


	private void Update()
	{
		
		SpinAndRotate();
	}



	/// <summary>
	///		Handles the spin / Rotate of the Object 
	/// </summary>
	private void SpinAndRotate()
	{
		Vector3 _rotate = new Vector3(0f, Time.deltaTime * rotationPerSecond, 0f);
		// Spin object on the y axis 
		transform.Rotate(_rotate, Space.World);


		// Float the object up and down using a sin curve 
		m_movePosition = m_positionOffset;
		m_movePosition.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

		transform.position = m_movePosition;
	}


	/// <summary>
	///		Handles what happens when a player is inside the trigger zone 
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerEnter(Collider other)
	{
		
		if (other.gameObject.tag == "Player")
		{
			Debug.Log("Collided with player object - Doing the thing!");
		}
	}
}
