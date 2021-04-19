using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Tree Settings")]
public class TreeSettings : UpdatableData
{
	public TreeData treeData; // class for holding tree data settings 

	public float treeSpawnMultiplier; // multiplier for spawning trees 
	public AnimationCurve treeSpawnCurve; // height curve to show min max amount of trees to spawn 

	/// <summary>
	///  Minimum Tree Spawn Curve Value 
	/// </summary>
	public float minimumTreeSpawnCurve
	{
		get
		{
			return treeSpawnMultiplier * treeSpawnCurve.Evaluate(0);
		}
	}

	/// <summary>
	///		Maximum Tree Spawn Curve Value 
	/// </summary>
	public float maximumTreeSpawnCurve
	{
		get
		{
			return treeSpawnMultiplier * treeSpawnCurve.Evaluate(1);
		}
	}
}