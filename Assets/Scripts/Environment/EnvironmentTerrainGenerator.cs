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

	public Transform m_CurrentPlayer;
	public Material mapMaterial;

	[Header("Tree Spawning")]
	public int maximumTreeSpawnCount = 125;
	public float maximumSpawnTreeXPosition = 500;
	public float maximumSpawnTreeZPosition = 500;
	public float treeSpawnWaitTimer = 0.01f;

	public GameObject TreePrefab;

	private Transform m_CameraReference;


	private List<GameObject> spawnedTrees = new List<GameObject>();
	
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
		if (GameObject.Find("Main Camera").GetComponent<Camera>())
		{
			m_CameraReference = GameObject.Find("Main Camera").transform;

		}
	}


	private void Start()
	{

		spawnedTrees.Clear();

		// Set the texture to the maps material 
		textureSettings.ApplyTextureToMaterial(mapMaterial);
	
		// Update the mesh material heights using the map material and height map settings height curve 
		textureSettings.UpdateMeshHeights(mapMaterial, heightSettings.minimumHeight, heightSettings.maximumHeight);
		
		m_maximumViewDistance = DetailLevels[DetailLevels.Length - 1].visibleDistanceThreshold;
		
		// Set the reference mesh world size 
		m_meshWorldSize = meshSettings.meshWorldSize;

		VisibleTerrainInViewDistance = Mathf.RoundToInt(m_maximumViewDistance / m_meshWorldSize);

		// Generate the trees along the mesh 
		StartCoroutine(SpawnTreesOnMesh(maximumTreeSpawnCount));


		Initialize();
	}


	private void Update()
	{
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
	IEnumerator SpawnTreesOnMesh(int SpawnTreeAmount)
	{

		float baseY = 0;
		
		for (int i = 0; i < SpawnTreeAmount; i++)
		{
			yield return new WaitForSeconds(treeSpawnWaitTimer);

			spawnTreeXPosition = Random.Range(-maximumSpawnTreeXPosition, maximumSpawnTreeXPosition);
			spawnTreeZPosition = Random.Range(-maximumSpawnTreeZPosition, maximumSpawnTreeZPosition);
			
			float spawnTreeY = Random.Range(-baseY, baseY);

			// GameObject spawnedTree = Instantiate(treeSettings.treeDataSettings., treeSpawnPosition, treeSettings.treeDataSettings.tree.transform.rotation);

			Vector3 newSpawnPosition = new Vector3(spawnTreeXPosition, baseY, spawnTreeZPosition);

			m_CameraReference.transform.position = new Vector3(newSpawnPosition.x, m_CameraReference.transform.position.y, newSpawnPosition.z);

			RaycastHit hit;

			if (Physics.Raycast(m_CameraReference.position, -Vector3.up, out hit))
			{
				Debug.Log("[EnvironmentTerrainGenerator.SpawnTreesOnMesh]: " +  "Spawn Offset Y Raycast Hit: " + hit.point.y);
			
				Debug.DrawLine(m_CameraReference.position, hit.point, Color.red);

				// Set the new spawn position y to where the raycast hit an object. 
				newSpawnPosition.y += hit.point.y;
			}
			
			GameObject spawned = Instantiate(TreePrefab, newSpawnPosition, TreePrefab.transform.rotation);

			spawnedTrees.Add(spawned);
		}
	}



}