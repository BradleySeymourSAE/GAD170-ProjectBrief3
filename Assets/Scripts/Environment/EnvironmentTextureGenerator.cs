using UnityEngine;
using System.Collections;

public static class EnvironmentTextureGenerator
{
	/// <summary>
	///		Creates a 2D Texture using an array of color values, width and height 
	/// </summary>
	/// <param name="map"></param>
	/// <param name="Width"></param>
	/// <param name="Height"></param>
	/// <returns></returns>
	public static Texture2D CreateTexture(Color[] map, int Width, int Height)
	{ 
		// new texture reference using the width and height 
		Texture2D newTexture = new Texture2D (Width, Height);

		newTexture.filterMode = FilterMode.Point; // set the new textures filter mode to point 
		newTexture.wrapMode = TextureWrapMode.Clamp; // clamp the texture wrapping mode 
		newTexture.SetPixels(map); // apply the new texture with the color map array 
		
		// Apply the new texture 
		newTexture.Apply();
		return newTexture;
	}


	/// <summary>
	///		Returns a 2d texture map 
	/// </summary>
	/// <param name="map"></param>
	/// <returns></returns>
	public static Texture2D ReturnTextureFromHeightMap(HeightMap map)
	{
		int w = map.values.GetLength(0);
		int h = map.values.GetLength(1);

		Color[] newTextureMap = new Color[w * h];

		// Loop through the height values  
		for (int y = 0; y < h; y++)
		{
			// Loop through the width values 
			for (int x = 0; x < w; x++)
			{
				// Create a new texture color map using an inverse lerp function
				newTextureMap[y * w + x] = Color.Lerp(Color.black, Color.white, Mathf.InverseLerp(map.minimumValue, map.maximumValue, map.values[x, y]));
			}
		}


		// Create a new texture using the width and height 
		return CreateTexture(newTextureMap, w, h);
	}
}