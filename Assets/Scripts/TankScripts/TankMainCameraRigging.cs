using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMainCameraRigging : MonoBehaviour
{

	private const string styled = "---";
  
	[Header(styled + " Camera Viewpoints " + styled)]
	public Transform FirstPersonCameraViewpoint;
	public Transform ThirdPersonCameraViewpoint;
	public enum CameraView { FirstPerson, ThirdPerson };


	[Header(styled + " Camera View Settings " + styled)]
	public CameraView CameraViewType = CameraView.ThirdPerson;

	private Transform cameraRigTransform;
	private Camera m_camera;
	private Vector3 MainCameraPosition;
	private bool isThirdPerson = false, isFirstPerson = false;


	private void Start()
	{
		cameraRigTransform = transform;
		m_camera = GetComponentInChildren<Camera>();
	}


	public void Update()
	{
		

	}
}
