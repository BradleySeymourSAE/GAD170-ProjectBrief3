using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCameraControls : MonoBehaviour
{
   
	[Range(0.01f, 1f)]
	public float mouseSensitivity = 0.7f;

	bool isCameraLocked;

	private void Start()
	{
		isCameraLocked = true;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	private void Update()
	{
		// Cursor lock mode 

		if (isCameraLocked && Input.GetKeyDown(KeyCode.Escape))
		{
			isCameraLocked = false;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		else if (!isCameraLocked && Input.GetKeyDown(KeyCode.Return))
		{
			isCameraLocked = true;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}


	public Vector2 mouseControls()
	{
		// If the camera is locked we want to return.
		if (isCameraLocked)
		{
			return Vector3.zero;
		}


		float x = Input.GetAxis("Mouse X") * mouseSensitivity * 20f;
		float y = Input.GetAxis("Mouse Y") * mouseSensitivity * 20f;


		return new Vector2(x, y);
	}

}
