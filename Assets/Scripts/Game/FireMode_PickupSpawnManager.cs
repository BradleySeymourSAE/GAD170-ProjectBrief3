using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ventiii.DevelopmentTools;



public class FireMode_PickupSpawnManager : MonoBehaviour
{

	public static FireMode_PickupSpawnManager Instance;
   
	public Transform PickupSpawnContainer; // weapon pickup spawn reference 

	public GameObject HealthPackPrefab;
	public GameObject AmmunitionPackPrefab;

	[Range(50, 150)]
	public float range = 100f;

	[SerializeField] private List<GameObject> currentlySpawnedInCollectableItems = new List<GameObject>();

	[SerializeField] private Vector3 spawnPoint;

	/// <summary>
	///		Handles when an event is enabled 
	/// </summary>
	private void OnEnable()
	{
		FireModeEvents.SpawnPickupEvent += SpawnPickups;
		FireModeEvents.OnObjectPickedUpEvent += Collected;
	}

	/// <summary>
	///		Handles when an event is disabled 
	/// </summary>
	private void OnDisable()
	{
		FireModeEvents.SpawnPickupEvent -= SpawnPickups;
		FireModeEvents.OnObjectPickedUpEvent -= Collected;
	}

	/// <summary>
	///	Setup instance 
	/// </summary>
	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}

		if (PickupSpawnContainer.GetComponent<Transform>())
		{
			PickupSpawnContainer = PickupSpawnContainer.GetComponent<Transform>();
			spawnPoint = PickupSpawnContainer.transform.position;
		}
	}

	/// <summary>
	///		Gets the currently spawned in weapon pickups 
	/// </summary>
	public int CurrentPickupsSpawned
	{
		get
		{
			return currentlySpawnedInCollectableItems.Count;
		}
	}


	/// <summary>
	///		 Spawns ammo and health packs throughout the map! 
	/// </summary>
	/// <param name="HealthPacks">The amount of health packs to spawn</param>
	/// <param name="AmmunitionPacks">The amount of ammunition packs to spawn</param>
	private void SpawnPickups(int HealthPacks, int AmmunitionPacks)
	{
		for (int i = 0; i < HealthPacks; i++)
		{
			float xPos = Random.Range(10f, range);
			float zPos = Random.Range(10f, range);

			Vector3 newSpawnPosition = new Vector3(spawnPoint.x + xPos, spawnPoint.y, spawnPoint.z + zPos);

			GameObject newCollectableItem = Instantiate(HealthPackPrefab, newSpawnPosition, HealthPackPrefab.transform.rotation);

			currentlySpawnedInCollectableItems.Add(newCollectableItem);
		}

		for (int a = 0; a < AmmunitionPacks; a++)
		{
			float x = Random.Range(10f, range);
			float z = Random.Range(10f, range);

			Vector3 spawnPos = new Vector3(spawnPoint.x + x, spawnPoint.y, spawnPoint.z + z);
			GameObject newAmmoPack = Instantiate(AmmunitionPackPrefab, spawnPos, AmmunitionPackPrefab.transform.rotation);

			currentlySpawnedInCollectableItems.Add(newAmmoPack);
		}


		FireModeEvents.OnPickupSpawnedEvent?.Invoke(currentlySpawnedInCollectableItems);
	}


	/// <summary>
	///		Handles when an item is collected 
	/// </summary>
	/// <param name="CurrentPlayer"></param>
	private void Collected(Transform CurrentPlayer)
	{

		if (CurrentPlayer.GetComponent<MainPlayerTank>())
		{
			Debug.Log("Yeap! Its a tank player...");
		}

	}
}
