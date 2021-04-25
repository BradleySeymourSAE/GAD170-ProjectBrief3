using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

/// <summary>
///		Environment Terrain Generator 
/// </summary>
[System.Serializable]
public class EnvironmentTerrainGenerator : MonoBehaviour
{

	const float viewerMoveThresholdForChunkUpdate = 25f;
	const float sqrViewerMoveThresholdForChunkUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;



	[Header("LOD Settings")]
	public int levelOfDetailColliderIndex;
	public LevelOfDetail[] DetailLevels;

	[Header("Environment Settings")]
	public MeshSettings meshSettings;
	public HeightMapSettings heightSettings;
	public TextureData2DSettings textureSettings;
	public TreeSettings treeSettings;

	[SerializeField] private Transform m_CurrentPlayer;
	public Material mapMaterial;

	[Header("Tree Spawning")]
	public int maximumTreeSpawnCount = 1000;
	public float maximumSpawnTreeXPosition = 1200;
	public float maximumSpawnTreeZPosition = 1200;

	[Range(1, 10)]
	public float timerBeforeTreesSpawned = 10f;
	public float timer = 0;
	private bool timerHasEnded = false;


	public GameObject TreePrefab;
	private Transform m_CameraReference;
	private List<GameObject> spawnedTrees = new List<GameObject>();
	[SerializeField] private List<Vector3> spawnedTreePositions = new List<Vector3>();
	
	float spawnTreeXPosition;
	float spawnTreeZPosition;

	float m_maximumViewDistance;
	float m_meshWorldSize;

	Vector2 viewerPosition;
	Vector2 viewerPositionOld;

	int VisibleTerrainInViewDistance;

	Dictionary<Vector2, TerrainChunk> visibleTerrainDictionary = new Dictionary<Vector2, TerrainChunk>();
	List<TerrainChunk> currentlyVisibleTerrains = new List<TerrainChunk>();


	private void Awake()
	{
		if (GameObject.Find("SpawnCamera").GetComponent<Camera>())
		{
			m_CameraReference = GameObject.Find("SpawnCamera").transform;
		}
	}

	private void OnEnable()
	{
		EnvironmentEvents.SpawnTreesEvent += SpawnTreesOnMesh;
	}

	private void OnDisable()
	{
		EnvironmentEvents.SpawnTreesEvent -= SpawnTreesOnMesh;
	}


	private void Start()
	{
		spawnedTrees.Clear();
		spawnedTreePositions.Clear();

		// Set the texture to the maps material 
		textureSettings.ApplyTextureToMaterial(mapMaterial);

		// Update the mesh material heights using the map material and height map settings height curve 
		textureSettings.UpdateMeshHeights(mapMaterial, heightSettings.minimumHeight, heightSettings.maximumHeight);

		m_maximumViewDistance = DetailLevels[DetailLevels.Length - 1].visibleDistanceThreshold;

		// Set the reference mesh world size 
		m_meshWorldSize = meshSettings.meshWorldSize;

		VisibleTerrainInViewDistance = Mathf.RoundToInt(m_maximumViewDistance / m_meshWorldSize);

		Initialize();
	}

	private void Update()
	{

		if (!m_CurrentPlayer && FindObjectOfType<MainPlayerTank>())
		{
			m_CurrentPlayer = FindObjectOfType<MainPlayerTank>().transform;
		}
		

		viewerPosition = new Vector2 (m_CurrentPlayer.position.x, m_CurrentPlayer.position.z);


		if (viewerPosition != viewerPositionOld)
		{
			foreach (TerrainChunk VisibleTerrain in currentlyVisibleTerrains)
			{
				VisibleTerrain.UpdateCollisionMesh();
			}
		}


		if ((viewerPositionOld - viewerPosition).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate)
		{
			viewerPositionOld = viewerPosition;

			Initialize();
		}


		// Once the countdown timer has ended we want to call the spawn tree's event using teh maximum tree spawn count 
		if (!timerHasEnded)
		{ 
			timer += Time.deltaTime;
			

			if (timer > timerBeforeTreesSpawned)
			{
				EnvironmentEvents.SpawnTreesEvent(maximumTreeSpawnCount);

				// We want to stop the timer here 
				timerHasEnded = true;
			}
		
		}


	}


	private void Initialize()
	{
			HashSet<Vector2> alreadyUpdatedChunkCoords = new HashSet<Vector2>();
			for (int i = currentlyVisibleTerrains.Count - 1; i >= 0; i--)
			{
				alreadyUpdatedChunkCoords.Add(currentlyVisibleTerrains[i].coord);
				currentlyVisibleTerrains[i].UpdateTerrainChunk();
			}

			int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / m_meshWorldSize);
			int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / m_meshWorldSize);

			Debug.Log("Current Chunk Coordinate X: " + currentChunkCoordX + "Current Chunk Coordinate Y: " + currentChunkCoordY);


			for (int yOffset = -VisibleTerrainInViewDistance; yOffset <= VisibleTerrainInViewDistance; yOffset++)
			{
				for (int xOffset = -VisibleTerrainInViewDistance; xOffset <= VisibleTerrainInViewDistance; xOffset++)
				{
					Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);
					if (!alreadyUpdatedChunkCoords.Contains(viewedChunkCoord))
					{
						if (visibleTerrainDictionary.ContainsKey(viewedChunkCoord))
						{
							visibleTerrainDictionary[viewedChunkCoord].UpdateTerrainChunk();
						}
						else
						{
							TerrainChunk newChunk = new TerrainChunk(viewedChunkCoord, heightSettings, meshSettings, treeSettings, DetailLevels, levelOfDetailColliderIndex, transform, m_CurrentPlayer, mapMaterial);
							visibleTerrainDictionary.Add(viewedChunkCoord, newChunk);
							newChunk.onVisibilityChanged += HandleOnTerrainVisibilityChanged;
							newChunk.Load();
						}
					}

				}
			}
	}


	private void HandleOnTerrainVisibilityChanged(TerrainChunk chunk, bool isVisible)
	{
		if (isVisible)
		{
			currentlyVisibleTerrains.Add(chunk);
		}
		else
		{
			currentlyVisibleTerrains.Remove(chunk);
		}
	}

	/// <summary>
	///		Spawns trees along the mesh material 
	/// </summary>
	/// <param name="Trees"></param>
	/// <returns></returns>
	private void SpawnTreesOnMesh(int SpawnTreeAmount)
	{

		float baseY = 0;

		for (int i = 0; i < SpawnTreeAmount; i++)
		{
			spawnTreeXPosition = Random.Range(-maximumSpawnTreeXPosition, maximumSpawnTreeXPosition);
			spawnTreeZPosition = Random.Range(-maximumSpawnTreeZPosition, maximumSpawnTreeZPosition);

			// GameObject spawnedTree = Instantiate(treeSettings.treeDataSettings., treeSpawnPosition, treeSettings.treeDataSettings.tree.transform.rotation);

			Vector3 newSpawnPosition = new Vector3(spawnTreeXPosition, baseY, spawnTreeZPosition);

			m_CameraReference.transform.position = new Vector3(newSpawnPosition.x, m_CameraReference.transform.position.y, newSpawnPosition.z);

			RaycastHit hit;

			if (Physics.Raycast(m_CameraReference.position, -Vector3.up, out hit))
			{
				// Debug.Log("[EnvironmentTerrainGenerator.SpawnTreesOnMesh]: " + "Spawn Offset Y Raycast Hit: " + hit.point.y);

				Debug.DrawLine(m_CameraReference.position, hit.point, Color.red);

				// Set the new spawn position y to where the raycast hit an object. 
				newSpawnPosition.y += hit.point.y;
			}

			GameObject spawned = Instantiate(TreePrefab, newSpawnPosition, TreePrefab.transform.rotation);

			if (!spawned.GetComponent<NavMeshObstacle>())
			{	
				NavMeshObstacle obstacle;
				// Add navigation mesh obstacle script to the game object 
				spawned.AddComponent<NavMeshObstacle>();

				// After the component has been added to the spawned in tree game object, we need to get a reference to it 
				obstacle = spawned.transform.GetComponent<NavMeshObstacle>();

				// Set the obstacle to be carving out an area within the terrain 
				obstacle.carving = true;

				// Change the obstacles shape to the navigation mesh obstacle shape capsule property
				// I don't know this sort of just makes sense for trees right ?? haha
				obstacle.shape = NavMeshObstacleShape.Capsule;
			}

			// Add the spawned tree gameobjects to the spawned in list 
			spawnedTrees.Add(spawned);

			// Add each tree to a list of Vector 3 tree spawn positions ( mostly for debugging ) 
			spawnedTreePositions.Add(spawned.transform.position);
		}


		// This could be used to generate the trees asyncronously while the game is loading in the main scene 
		// EnvironmentEvents.HandleTreesSpawnedEvent?.Invoke(spawnedTrees);
		
		// This is more for debugging at this point and I am just experimenting with ways to go about this!  
		// EnvironmentEvents.HandleTreeSpawnPositionsEvent?.Invoke(spawnedTreePositions);
	}
}