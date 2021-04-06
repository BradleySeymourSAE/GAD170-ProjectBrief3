using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShell : MonoBehaviour
{
    

        public LayerMask Tank, Infantry;
        public float maximumDamage = 40f;
        public float force = 1000f;
        public float maximumShellLifetime = 10f;
        public float explosionRadius = 50f;
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

		Collider[] TankColliders = Physics.OverlapSphere(transform.position, explosionRadius, Tank);
		Collider[] InfantryColliders = Physics.OverlapSphere(transform.position, explosionRadius, Infantry);


		for (int i = 0; i < TankColliders.Length; i++)
		{
			Rigidbody targetRigidbody = TankColliders[i].GetComponent<Rigidbody>();


			if (!targetRigidbody)
			{
				Debug.LogWarning("[TankShell.ExplodeShell]: " + "This T90 Tank doesn't have a rigidbody!! Continuing anyway..");
				continue;
			}


			targetRigidbody.AddExplosionForce(force, transform.position, explosionRadius);

			float inflictedDamage = CalculateDamageFromRange(targetRigidbody.position);

			FireModeEvents.OnDamageReceivedEvent?.Invoke(targetRigidbody.transform, -inflictedDamage);
		}

		for (int i = 0; i < InfantryColliders.Length; i++)
		{
			Rigidbody targetInfantryRigidbody = InfantryColliders[i].GetComponent<Rigidbody>();

			if (!targetInfantryRigidbody)
			{
				Debug.LogWarning("[TankShell.ExplodeShell]: " + "This Infantry Character doesn't have a rigidbody!! Continuing anyway..");
				continue;
			}


			targetInfantryRigidbody.AddExplosionForce(force, transform.position, explosionRadius);

			float inflictDamage = CalculateDamageFromRange(targetInfantryRigidbody.position);

			FireModeEvents.OnDamageReceivedEvent?.Invoke(targetInfantryRigidbody.transform, -inflictDamage);
		}

		GameObject clonedExplosion = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
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
