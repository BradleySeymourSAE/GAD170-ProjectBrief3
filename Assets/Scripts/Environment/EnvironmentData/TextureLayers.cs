using UnityEngine;
using System.Collections;


[System.Serializable]
public class TextureLayers
{ 
	/// <summary>
	///		The 2D texture to apply the tint to 
	/// </summary>
	public Texture2D texture;
	
	[Header("Tint Settings")]
	/// <summary>
	///		The tint color for the texture layer 
	/// </summary>
	public Color tint;

	/// <summary>
	///		The tints strength 
	/// </summary>
	[Range(0, 1)]
	public float tintColorStrength;

	/// <summary>
	///		The starting height for the blend
	/// </summary>
	[Range(0, 1)]
	public float height;

	[Header("Blend Settings")]
	/// <summary>
	///		The strength of the blend
	/// </summary>
	[Range(0, 1)]
	public float blendColorStrength;

	/// <summary>
	///		The scale of the texture 2d   
	/// </summary>
	public float textureScale;
}