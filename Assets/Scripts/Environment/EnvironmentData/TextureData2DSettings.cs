using UnityEngine;
using System.Collections;
using System.Linq;


[CreateAssetMenu(fileName = "New 2D Texture Settings")]
public class TextureData2DSettings : UpdatableData
{ 

	const int m_textureSize = 512;
	const TextureFormat m_textureFormat = TextureFormat.RGB565;

	public TextureLayers[] textureLayers;

	float savedHeightMinimum;
	float savedHeightMaximum;


	public void ApplyTextureToMaterial(Material mat)
	{
		mat.SetInt("layerCount", textureLayers.Length);
		mat.SetColorArray("baseColours", textureLayers.Select(x => x.tint).ToArray());
		mat.SetFloatArray("baseStartHeights", textureLayers.Select(x => x.height).ToArray());
		mat.SetFloatArray("baseBlends", textureLayers.Select(x => x.blendColorStrength).ToArray());
		mat.SetFloatArray("baseColourStrength", textureLayers.Select(x => x.tintColorStrength).ToArray());
		mat.SetFloatArray("baseTextureScales", textureLayers.Select(x => x.textureScale).ToArray());
		Texture2DArray texturesArray = CreateTexture2DArray(textureLayers.Select(x => x.texture).ToArray());
		mat.SetTexture("baseTextures", texturesArray);

		UpdateMeshHeights(mat, savedHeightMinimum, savedHeightMaximum);
	}

	/// <summary>
	///		Updates the mesh material height using a min and max value 
	/// </summary>
	/// <param name="mat"></param>
	/// <param name="min"></param>
	/// <param name="max"></param>
	public void UpdateMeshHeights(Material mat, float min, float max)
	{
		savedHeightMinimum = min;
		savedHeightMaximum = max;

		mat.SetFloat("minHeight", min);
		mat.SetFloat("maxHeight", max);
	}


	/// <summary>
	///		Creates an array of 2d textures 
	/// </summary>
	/// <param name="textures"></param>
	/// <returns></returns>
	Texture2DArray CreateTexture2DArray(Texture2D[] textures)
	{
		// Creates an array of 2d textures 
		Texture2DArray arr = new Texture2DArray(m_textureSize, m_textureSize, textures.Length, m_textureFormat, true);
		
		// Loop through the array of textures and set the pixel index 
		for (int i = 0; i < textures.Length; i++)
		{
			arr.SetPixels(textures[i].GetPixels(), i);
		}

		arr.Apply();
		return arr;
	}
}