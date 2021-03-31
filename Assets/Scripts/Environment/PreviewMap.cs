using UnityEngine;
using System.Collections;


/// <summary>
///		Preview Map Terrain 
/// </summary>
public class PreviewMap : MonoBehaviour
{
	[SerializeField] private Renderer m_textureRenderer;
	[SerializeField] private MeshFilter m_meshFilter;
	[SerializeField] private MeshRenderer m_meshRenderer;

	public enum DrawPreviewMode { Noise, Mesh };
	[Header("Draw Preview")]
	public DrawPreviewMode Mode; 

	[Header("Adjustable Map Settings")]
	public MeshSettings meshSettings;
	public HeightMapSettings heightMapSettings;
	public TextureData2DSettings textureData;
	
	[Range(0, MeshSettings.NumberSupportedLODs - 1)]
	public int PreviewLevelOfDetail;
	public bool AutoUpdate;

	[Header("Material Settings")]
	public Material terrainMaterial;


	public void DrawPreviewMapInEditor()
	{
		// Apply the texture material 
		textureData.ApplyTextureToMaterial(terrainMaterial);
	
		// Update the mesh's height 

		textureData.UpdateMeshHeights(terrainMaterial, heightMapSettings.minimumHeight, heightMapSettings.maximumHeight);

		HeightMap heightMap = EnvironmentHeightMapGenerator.CreateHeightMap(meshSettings.NumberOfVerticesPerLine, meshSettings.NumberOfVerticesPerLine, heightMapSettings, Vector2.zero);

	
		if (Mode == DrawPreviewMode.Noise)
		{
			// Draw noise texture map 
			Debug.LogWarning("[PreviewMap]: " + "Creating noise texture!");
			DrawNoiseTexture(EnvironmentTextureGenerator.ReturnTextureFromHeightMap(heightMap));
		}
		else if (Mode == DrawPreviewMode.Mesh)
		{
			Debug.LogWarning("[PreviewMap]: " + "Creating terrain mesh!");
			DrawMeshPreview(EnvironmentMeshGenerator.CreateTerrainMesh(heightMap.values, meshSettings, PreviewLevelOfDetail));
		}
		else
		{
			// Log a warning, something must have gone seriously wrong 
			Debug.LogWarning("[PreviewMap]: " + "Could not create noise or mesh preview!");
		}
			
	}

	/// <summary>
	///		Draws a 2D Noise map texture in the editor 
	/// </summary>
	/// <param name="texture"></param>
	public void DrawNoiseTexture(Texture2D texture)
	{
		m_textureRenderer.sharedMaterial.mainTexture = texture;
		m_textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height) / 10f;

		m_textureRenderer.gameObject.SetActive(true);
		m_meshFilter.gameObject.SetActive(false);
	}

	/// <summary>
	///		Creates a mesh using the mesh data object 
	/// </summary>
	public void DrawMeshPreview(MeshData Mesh)
	{
		// Creates a new mesh 
		m_meshFilter.sharedMesh = Mesh.CreateMesh();

		// Disables the texture renderer game object 
		m_textureRenderer.gameObject.SetActive(false);
		// Sets the mesh active 
		m_meshFilter.gameObject.SetActive(true);
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
	void OnTextureValuesChanged()
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
			meshSettings.OnValuesUpdatedEvent -= OnValuesUpdated;
			meshSettings.OnValuesUpdatedEvent += OnValuesUpdated;
		}

		// Check height map data is not null
		if (heightMapSettings != null)
		{
			// Add height setting event listeners 
			heightMapSettings.OnValuesUpdatedEvent -= OnValuesUpdated;
			heightMapSettings.OnValuesUpdatedEvent += OnValuesUpdated;
		}	

		// Check texture data is not null
		if (textureData != null)
		{
			// Add texture data event listeners 
			textureData.OnValuesUpdatedEvent -= OnTextureValuesChanged;
			textureData.OnValuesUpdatedEvent += OnTextureValuesChanged;
		}


	}
}