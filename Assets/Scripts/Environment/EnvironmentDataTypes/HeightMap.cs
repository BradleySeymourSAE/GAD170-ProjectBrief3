using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
///		Object for holding height map data 
/// </summary>
public struct HeightMap
{ 
	public readonly float[,] values;
	public readonly float minimumValue;
	public readonly float maximumValue;

	public HeightMap(float[,] values, float min, float max)
	{
		this.values = values;
		this.minimumValue = min;
		this.maximumValue = max;
	}
}
