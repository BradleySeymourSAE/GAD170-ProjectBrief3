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
	[SerializeField] private Transform m_CurrentPlayerRef;

	/// <summary>
	///		The cameras new position 
	/// </summary>
	[SerializeField] private Vector3 m_CameraPosition;

	/// <summary>
	///		Does the camera currently have a target? 
	/// </summary>
	[SerializeField] private bool cameraHasTarget = false;

	public LayerMask m_MinimapMask;


	#endregion

	#region Unity References 

	private void OnEnable()
	{
		FireModeEvents.HandleOnPlayerSpawnedEvent += InitializeCamera;
	}

	private void OnDisable()
	{
		FireModeEvents.HandleOnPlayerSpawnedEvent -= InitializeCamera;
	}


	/// <summary>
	///		Late Update Method
	/// </summary>
	private void LateUpdate()
	{

		if (!cameraHasTarget || !m_CurrentPlayerRef)
		{
			return;
		}
	

		m_CameraPosition = m_CurrentPlayerRef.position;

		m_CameraPosition.y = transform.position.y;

		transform.position = m_CameraPosition;

		transform.rotation = Quaternion.Euler(90f, m_CurrentPlayerRef.eulerAngles.y, 0f);
	}


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
			m_CurrentPlayerRef = PlayerReference;

			if (m_CurrentPlayerRef != null)
			{
				cameraHasTarget = true;
			}
		}
		else
		{
			Debug.LogWarning("[MinimapCamera.InitializeCamera]: " + "Could not find player reference!");
		}
	}

	#endregion

}
