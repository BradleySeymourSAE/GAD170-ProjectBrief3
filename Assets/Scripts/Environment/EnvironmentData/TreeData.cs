using System.Collections;
using UnityEngine;

[System.Serializable]
/// <summary>
///		Tree Object Data class 
/// </summary>
public class TreeData { 
	
	/// <summary>
	///		The tree object prefab
	/// </summary>
	public GameObject tree;
	
	/// <summary>
	///		The amount of trees
	/// </summary>
	public float amount = 50;

	/// <summary>
	///		The tree offset position 
	/// </summary>
	public Vector3 offset;

	/// <summary>
	///		The scale of the tree game object  
	/// </summary>
	public float treeScale;
}