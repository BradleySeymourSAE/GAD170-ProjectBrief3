using UnityEngine;
using UnityEngine.AI;
using System.Collections;


/// <summary>
///		Preview Map Terrain 
/// </summary>
public class PreviewMap : MonoBehaviour
{
	public Renderer textureRender;
	public MeshFilter meshFilter;
	public MeshRenderer meshRenderer;


	public enum DrawMode { NoiseMap, Mesh, FalloffMap };
	public DrawMode drawMode;

	[Header("Adjustable Map Settings")]
	public MeshSettings meshSettings;
	public HeightMapSettings heightMapSettings;
	public TextureData2DSettings textureData;
	public TreeSettings treeSettings;

	[Header("Material Settings")]
	public Material terrainMaterial;

	[Range(0, MeshSettings.numSupportedLODs - 1)]
	public int editorPreviewLOD;
	public bool autoUpdate;


	public void DrawPreviewMapInEditor()
	{
		// Apply the texture material 
		textureData.ApplyTextureToMaterial(terrainMaterial);
	
		// Update the mesh's height 

		textureData.UpdateMeshHeights(terrainMaterial, heightMapSettings.minimumHeight, heightMapSettings.maximumHeight);

		HeightMap heightMap = EnvironmentHeightMapGenerator.CreateHeightMap(meshSettings.numberOfVerticesPerLine, meshSettings.numberOfVerticesPerLine, heightMapSettings, Vector2.zero);

	
		if (drawMode == DrawMode.NoiseMap)
		{
			// Draw noise texture map 
			Debug.LogWarning("[PreviewMap]: " + "Creating noise texture!");
			DrawTexture2D(EnvironmentTextureGenerator.ReturnTextureFromHeightMap(heightMap));
		}
		else if (drawMode == DrawMode.Mesh)
		{
			// Debug.LogWarning("[PreviewMap]: " + "Creating terrain mesh!");
			DrawMesh(EnvironmentMeshGenerator.CreateTerrainMesh(heightMap.values, meshSettings, editorPreviewLOD));
		}
		else if (drawMode == DrawMode.FalloffMap)
		{
			DrawTexture2D(EnvironmentTextureGenerator.ReturnTextureFromHeightMap(new HeightMap(FalloffGenerator.GenerateFalloffMap(meshSettings.numberOfVerticesPerLine), 0, 1)));
		}
			
	}

	/// <summary>
	///		Draws a 2D Noise map texture in the editor 
	/// </summary>
	/// <param name="texture"></param>
	public void DrawTexture2D(Texture2D texture)
	{
		textureRender.sharedMaterial.mainTexture = texture;
		textureRender.transform.localScale = new Vector3(texture.width, 1, texture.height) / 10f;

		textureRender.gameObject.SetActive(true);
		meshFilter.gameObject.SetActive(false);
	}

	/// <summary>
	///		Creates a mesh using the mesh data object 
	/// </summary>
	public void DrawMesh(MeshData Mesh)
	{
		// Creates a new mesh 
		meshFilter.sharedMesh = Mesh.CreateMesh();

		// Disables the texture renderer game object 
		textureRender.gameObject.SetActive(false);
		// Sets the mesh active 
		meshFilter.gameObject.SetActive(true);
	}

	/// <summary>
	///		Updates the map preview 
	/// </summary>
	void OnValuesUpdated()
	{
		// Check that the application is not currently playing 
		if (!Application.isPlaying)
		{
			// If it isnt playing, draw the map preview in the editor 
			DrawPreviewMapInEditor();
		}
	}

	/// <summary>
	///		Event listener for texture values changing. 
	/// </summary>
	void OnTextureValuesUpdated()
	{
		// Apply texture to the terrain material  
		textureData.ApplyTextureToMaterial(terrainMaterial);
	}

	/// <summary>
	///		Event handler on validation event listeners 
	/// </summary>
	void OnValidate()
	{

		// Check mesh settings is not null
		if (meshSettings != null)
		{
			// Add mesh setting event listeners 
			meshSettings.OnValuesUpdated -= OnValuesUpdated;
			meshSettings.OnValuesUpdated += OnValuesUpdated;
		}

		// Check height map data is not null
		if (heightMapSettings != null)
		{
			// Add height setting event listeners 
			heightMapSettings.OnValuesUpdated -= OnValuesUpdated;
			heightMapSettings.OnValuesUpdated += OnValuesUpdated;
		}	

		// Check texture data is not null
		if (textureData != null)
		{
			// Add texture data event listeners 
			textureData.OnValuesUpdated -= OnTextureValuesUpdated;
			textureData.OnValuesUpdated += OnTextureValuesUpdated;
		}


	}
}