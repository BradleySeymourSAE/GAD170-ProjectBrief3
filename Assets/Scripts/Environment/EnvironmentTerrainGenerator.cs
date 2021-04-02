using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	[Header("Tree Spawning")]
	public int spawnTreeCount;
	public float treeSpawnWaitTimer = 1.5f;

	
	int spawnTreeXPosition;
	int spawnTreeYPosition;
	int currentTreeSpawnIndex;
	float m_maximumViewDistance;
	float m_meshWorldSize;

	private void Start()
	{
		// Set the texture to the maps material 
		textureSettings.ApplyTextureToMaterial(mapMaterial);
	
		// Update the mesh material heights using the map material and height map settings height curve 
		textureSettings.UpdateMeshHeights(mapMaterial, heightSettings.minimumHeight, heightSettings.maximumHeight);
		
		m_maximumViewDistance = DetailLevels[DetailLevels.Length - 1].visibleDistanceThreshold;
		
		// Set the mesh world size 
		m_meshWorldSize = meshSettings.MeshWorldSize;


		// Generate the trees along the mesh 
		StartCoroutine(SpawnTreesOnMesh());
	}


	/// <summary>
	///		WIP -  Spawns trees along the mesh material 
	/// </summary>
	/// <param name="Trees"></param>
	/// <returns></returns>
	IEnumerator SpawnTreesOnMesh()
	{
		while (currentTreeSpawnIndex < spawnTreeCount)
		{
			spawnTreeXPosition = Random.Range(1, 50);
			spawnTreeYPosition = Random.Range(1, 30);

			Instantiate(treeSettings.treeData.tree, new Vector3(spawnTreeXPosition, 0, spawnTreeYPosition), Quaternion.identity);
			yield return new WaitForSeconds(treeSpawnWaitTimer);

			spawnTreeCount += 1;
		}
	}



}