using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu()]
public class TreeSettings : UpdatableData
{
	public TerrainTreeDataSettings treeDataSettings; // class for holding tree data settings 

	public float treeSpawnMultiplier; // multiplier for spawning trees 
	public AnimationCurve treeSpawnCurve; // height curve to show min max amount of trees to spawn 

	/// <summary>
	///  Minimum Tree Spawn Curve Value 
	/// </summary>
	public float MinimumTreeSpawnHeight
	{
		get
		{
			return treeSpawnMultiplier * treeSpawnCurve.Evaluate(0);
		}
	}

	/// <summary>
	///		Maximum Tree Spawn Curve Value 
	/// </summary>
	public float MaximumTreeSpawnHeight
	{
		get
		{
			return treeSpawnMultiplier * treeSpawnCurve.Evaluate(1);
		}
	}



#if UNITY_EDITOR

	protected override void OnValidate()
	{
		treeDataSettings.ValidateValues();
		base.OnValidate();
	}


#endif
}