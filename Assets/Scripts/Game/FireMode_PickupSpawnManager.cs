using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ventiii.DevelopmentTools;



public class FireMode_PickupSpawnManager : MonoBehaviour
{

	public static FireMode_PickupSpawnManager Instance;
   
	public Transform PickupSpawnContainer; // weapon pickup spawn reference 

	public GameObject HealthPackPrefab;
	[Range(50, 150)]
	public float range = 100f;

	[SerializeField] private int spawnAmount = 10;
	[SerializeField] private float pickupSpawnTimer = 10f;

	[SerializeField] private int totalPickupsBeforeRespawn = 5;
	[SerializeField] private int m_spawnedIndex;


	[SerializeField] private List<GameObject> currentPickupsSpawnedIn = new List<GameObject>();

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
			return currentPickupsSpawnedIn.Count;
		}
	}


	/// <summary>
	///		Amount of pickups to spawn 
	/// </summary>
	/// <param name="SpawnAmount"></param>
	private void SpawnPickups(int SpawnAmount)
	{
		for (int i = 0; i < SpawnAmount; i++)
		{

			float xPos = Random.Range(10f, range);
			float zPos = Random.Range(10f, range);

			Vector3 newSpawnPosition = new Vector3(spawnPoint.x + xPos, spawnPoint.y, spawnPoint.z + zPos);

			GameObject newCollectableItem = Instantiate(HealthPackPrefab, newSpawnPosition, HealthPackPrefab.transform.rotation);

			currentPickupsSpawnedIn.Add(newCollectableItem);
		}


		FireModeEvents.OnPickupSpawnedEvent?.Invoke(currentPickupsSpawnedIn);
	}
	/// <summary>
	///		Handles when an item is collected 
	/// </summary>
	/// <param name="CurrentPlayer"></param>
	private void Collected(Transform CurrentPlayer)
	{

		if (CurrentPlayer.GetComponent<Tank>())
		{
			Debug.Log("Yeap! Its a tank player...");
		}

	}
}
