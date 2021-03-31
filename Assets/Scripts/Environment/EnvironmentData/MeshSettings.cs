using UnityEngine;
using System.Collections;


[CreateAssetMenu(menuName = "Environment/Settings", fileName = "New Terrain Mesh Settings")]
public class MeshSettings : UpdatableData
{ 

	public const int NumberSupportedLODs = 5;
	public const int NumberOfSupportedChunkSizes = 9;
	public static readonly int[] supportedChunkSizes = { 48, 72, 96, 120, 144, 168, 192, 216, 240 };

	public float MeshScalingFactor = 2.5f;
	
	[Range(0, NumberOfSupportedChunkSizes - 1)]
	public int ChunkSizeIndex;

	/// <summary>
	///		Returns the number of vertices per line 
	/// </summary>
	public int NumberOfVerticesPerLine
	{
		get
		{
			return supportedChunkSizes [ChunkSizeIndex] + 5;
		}
	}


	/// <summary>
	///		Returns the number of vertices per line and scales the mesh by the scaling factor accordingly 
	/// </summary>
	public float MeshWorldSize
	{
		get
		{
			return (NumberOfVerticesPerLine - 3) * MeshScalingFactor;
		}
	}

}