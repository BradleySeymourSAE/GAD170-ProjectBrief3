using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		Stores LevelOfDetail Information
/// </summary>
[System.Serializable]
public struct LevelOfDetail
{
	/// <summary>
	///		The level of detail 
	/// </summary>
	[Range(0, MeshSettings.numSupportedLODs - 1)]
	public int lod;
	
	/// <summary>
	///		Visible view distance threshold for the level of detail
	/// </summary>
	public float visibleDistanceThreshold;

	/// <summary>
	///		Returns the square visible distance threshold 
	/// </summary>
	public float VisibleDistanceThresholdSquared
	{
		get
		{
			return visibleDistanceThreshold * visibleDistanceThreshold;
		}
	}
}