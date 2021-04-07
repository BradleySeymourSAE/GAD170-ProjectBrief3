using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu_AnimationStateController : MonoBehaviour
{
    
    public Transform Turret;

    public float desiredLeftTurretRotation = -30f;
    public float desiredRightTurretRotation = 40f;

	private float rotationSpeed = 0.5f;

	private void Start()
	{
		if (Turret.GetComponent<Transform>() != null)
		{
			Turret = Turret.GetComponent<Transform>();
		}
	}


	private void Update()
	{
		StartCoroutine(RotateTurret());
	}


	IEnumerator RotateTurret()
	{
		float timer = 0;

		while (true)
		{
			float angle = Mathf.Sin(timer) * 50;


			Turret.transform.localRotation = Quaternion.AngleAxis(angle, -Vector3.up);

			timer += rotationSpeed * Time.deltaTime;
			yield return null;
		}
		
	}
}
