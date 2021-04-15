#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion


[System.Serializable]
/// <summary>
///		Minimap Camera for following player reference 
/// </summary>
public class MinimapCamera : MonoBehaviour
{

	#region Private Variables 
	
	/// <summary>
	///		Reference to the Main Player's Tank
	/// </summary>
	private Transform m_CurrentPlayerRef;

	/// <summary>
	///		The cameras new position 
	/// </summary>
	private Vector3 m_CameraPosition;

	/// <summary>
	///		Does the camera currently have a target? 
	/// </summary>
	private bool cameraHasTarget = false;

	public LayerMask m_MinimapMask;


	#endregion

	#region Unity References 

	/// <summary>
	///		Late Update Method
	/// </summary>
	private void LateUpdate()
	{

		if (!cameraHasTarget)
		{
			return;
		}


		m_CameraPosition = m_CurrentPlayerRef.position;

		m_CameraPosition.y = transform.position.y;

		transform.position = m_CameraPosition;

		transform.rotation = Quaternion.Euler(90f, m_CurrentPlayerRef.eulerAngles.y, 0f);
	}

	#region Unity Events  

	/// <summary>
///		On Enable Event Listeners 
/// </summary>
	private void OnEnable()
	{
		FireModeEvents.HandleOnPlayerSpawnedEvent += InitializeCamera;
	}

	/// <summary>
	///		On Disable Event Listeners 
	/// </summary>
	private void OnDisable()
	{
		FireModeEvents.HandleOnPlayerSpawnedEvent -= InitializeCamera;
	}

	#endregion

	#endregion

	#region Private Methods 
	
	/// <summary>
	///		Initializes the camera 
	/// </summary>
	/// <param name="PlayerReference"></param>
	private void InitializeCamera(Transform PlayerReference)
	{
		if (PlayerReference.GetComponent<MainPlayerTank>())
		{
			m_CurrentPlayerRef = PlayerReference.transform;
		
			cameraHasTarget = true;
		}
		else
		{
			Debug.LogWarning("[MinimapCamera.InitializeCamera]: " + "Could not find player reference!");
		}
	}

	#endregion

}
