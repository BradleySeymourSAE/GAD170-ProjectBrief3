using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Ventiii.DevelopmentTools;

/// <summary>
///		Handles Pathfinding for enemy infantry characters  
/// </summary>
[System.Serializable]
public class InfantryAI_Pathfinder
{
	public List<Transform> movementWaypoints;
	private const float Speed = 30f;
	private InfantryAI_Main m_InfantryMain;
	
	[Header("Movement Paths")]
	private int currentPathIndex;
	private float findPathTimer;
	private Vector3 moveDirection;
	private Vector3 lastMoveDirection;
	private NavMeshAgent agent;

	private bool enablePathfinding;

	private bool isDebugging = true;

	public void Setup(Transform InfantryAI)
	{
		m_InfantryMain = InfantryAI.GetComponent<InfantryAI_Main>();


		movementWaypoints = new List<Transform>();
		movementWaypoints.Clear();

		
		if (m_InfantryMain.GetComponent<NavMeshAgent>() != null)
		{
			Debug.Log("[InfantryAI_Pathfinder]: " + "Navigation Mesh Agent has been found!");
			agent = m_InfantryMain.GetComponent<NavMeshAgent>();


			agent.autoBraking = false;
		}
		else
		{
			Debug.LogError("[InfantryAI_Pathfinder]: " + "Could not find Navigation Mesh Agent!");
		}

	

		EnablePathfinding(false); // Disable pathfinding initially 
	}

	/// <summary>
	///		Is the Enemy AI able to find paths? 
	/// </summary>
	/// <param name="Enabled"></param>
	public void EnablePathfinding(bool Enabled)
	{
		enablePathfinding = Enabled;
	}

	/// <summary>
	///		Handles the movement of the AI character 
	/// </summary>
	public void HandleMovement()
	{
		if (enablePathfinding == false)
		{
			// If we cant move, dont.
			return;
		}

		// TODO: Need to implement the logic for pathfinding
		// However we are almost there


		// Need to finish up the game logic and start working on UI before finishing this off 


	}
}
