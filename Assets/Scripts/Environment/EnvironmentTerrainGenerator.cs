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
	/// <summary>
	///		TODO: Remove chunk updating from environment generator as the map will not be that 
	///		big, Which means I could probably remove the level of details (LODs) aswell 
	/// </summary>
	const float movementThresholdForChunkUpdate = 25f;
	const float squaredMovementThresholdForChunkUpdate = movementThresholdForChunkUpdate * movementThresholdForChunkUpdate;

	[Header("LOD Settings")]
	public int levelOfDetailColliderIndex;
	public LevelOfDetail[] DetailLevels;

	[Header("Environment Settings")]
	public MeshSettings meshSettings;
	public HeightMapSettings heightSettings;
	public TextureData2DSettings textureSettings;
	public TreeSettings treeSettings;
	public Material mapMaterial;
	public NavMeshSurface NavMeshSurface;

	[Header("Tree Spawning")]
	public int maximumTreeSpawnCount;
	public float maximumSpawnTreeXPosition = 500;
	public float maximumSpawnTreeYPosition = 500;
	public float treeSpawnWaitTimer = 1.5f;

	private List<GameObject> spawnedTrees = new List<GameObject>();
	
	float spawnTreeXPosition;
	float spawnTreeYPosition;
	float m_maximumViewDistance;
	float m_meshWorldSize;

	private void Start()
	{

		spawnedTrees.Clear();

		// Set the texture to the maps material 
		textureSettings.ApplyTextureToMaterial(mapMaterial);
	
		// Update the mesh material heights using the map material and height map settings height curve 
		textureSettings.UpdateMeshHeights(mapMaterial, heightSettings.minimumHeight, heightSettings.maximumHeight);
		
		m_maximumViewDistance = DetailLevels[DetailLevels.Length - 1].visibleDistanceThreshold;
		
		// Set the mesh world size 
		m_meshWorldSize = meshSettings.MeshWorldSize;


		// Generate the trees along the mesh 
		StartCoroutine(SpawnTreesOnMesh(maximumTreeSpawnCount));

		// Builds navigation Mesh 
		NavMeshSurface.BuildNavMesh();
	}


	/// <summary>
	///		Spawns trees along the mesh material 
	/// </summary>
	/// <param name="Trees"></param>
	/// <returns></returns>
	IEnumerator SpawnTreesOnMesh(int SpawnTreeAmount)
	{
		
		for (int i = 0; i < SpawnTreeAmount; i++)
		{
			yield return new WaitForSeconds(treeSpawnWaitTimer);

			spawnTreeXPosition = Random.Range(1, maximumSpawnTreeXPosition);
			spawnTreeYPosition = Random.Range(1, maximumSpawnTreeYPosition);

			Vector3 treeSpawnPosition = new Vector3(spawnTreeXPosition, 0, spawnTreeYPosition);

			GameObject spawnedTree = Instantiate(treeSettings.treeData.tree, treeSpawnPosition, treeSettings.treeData.tree.transform.rotation);



			Instantiate(treeSettings.treeData.tree, treeSpawnPosition, Quaternion.identity);
			
			spawnedTrees.Add(spawnedTree);
		}
	}



}