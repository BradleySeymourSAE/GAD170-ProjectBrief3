using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShell : MonoBehaviour
{
    

        public LayerMask Tank;
        public float maximumDamage = 40f;
        public float force = 1000f;
        public float maximumShellLifetime = 4f;
        public float explosionRadius = 25f;
		public GameObject ExplosionPrefab;




	// Called once a trigger hits an object...


	private void OnTriggerEnter(Collider ObjectCollided)
	{
		
        if (ObjectCollided.transform == transform)
		{
			return;
		}
		else
		{
			ExplodeShell();
		}
	}


	/// <summary>
	///		Handles the tank shells explosion (When shell hits an object) 
	/// </summary>
	private void ExplodeShell()
	{

		Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, Tank);


		for (int i = 0; i < colliders.Length; i++)
		{
			Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();


			if (!targetRigidbody)
			{
				// Debug.LogWarning("[TankShell.ExplodeShell]: " + "This T90 Tank doesn't have a rigidbody!! Continuing anyway..");
				continue;
			}


			targetRigidbody.AddExplosionForce(force, transform.position, explosionRadius);

			float inflictedDamage = CalculateDamageFromRange(targetRigidbody.position);

			FireModeEvents.OnReceivedDamageEvent?.Invoke(targetRigidbody.transform, -inflictedDamage);
		}



		GameObject clonedExplosion = Instantiate(ExplosionPrefab, transform.position, ExplosionPrefab.transform.rotation);

		Destroy(clonedExplosion, maximumShellLifetime);
	}

	/// <summary>
	///		Calculates the damage to inflict based on the targets position (range)
	/// </summary>
	/// <param name="TargetPosition"></param>
	/// <returns></returns>
	private float CalculateDamageFromRange(Vector3 TargetPosition)
	{
		Vector3 targetToExplosionDistance = TargetPosition - transform.position;

		float explosiveDist = targetToExplosionDistance.magnitude;
		float relativeDistance = (explosionRadius - explosiveDist) / explosionRadius;

		float damage = relativeDistance * maximumDamage;

		damage = Mathf.Max(0, damage);

		return damage;
	}
}
