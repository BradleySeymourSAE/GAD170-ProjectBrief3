using System.Collections;
using UnityEngine;

[System.Serializable]
/// <summary>
///		Tree Object Data class 
/// </summary>
public class TerrainTreeDataSettings 
{ 
	/// <summary>
	///		The amount of trees to spawn in 
	/// </summary>
	public int Amount = 50;

	[Range(0, 1)]
	/// <summary>
	///		The scale of the tree game object  
	/// </summary>
	public float Scale;

	public int Seed;
	public Vector2 Offset;


	public void ValidateValues()
	{
		Amount = Mathf.Max(Amount, 1);
		Scale = Mathf.Clamp01(Scale);
	}
}