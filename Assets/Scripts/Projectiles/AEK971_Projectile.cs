#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
///     AEK971 Bullet Projectile Class 
/// </summary>
public class AEK971_Projectile : MonoBehaviour
{
	#region Public Variables 
	/// <summary>
	///		Layer for Events 
	/// </summary>
	public LayerMask DestroyableObject;
	
	/// <summary>
	///		The maximum amount of damage a bullet can do 
	/// </summary>
	public float maximumDamage = 3f;

	/// <summary>
	///		Force to apply 
	/// </summary>
	public float force = 3f;

	/// <summary>
	///		Radius of damage 
	/// </summary>
	public float damageRadius = 0.2f;

	/// <summary>
	///		Explosive impact prefab 
	/// </summary>
	public GameObject Explosion;

	#endregion

	#region Private Variables 
	
	/// <summary>
	///		Lifetime of the bullet 
	/// </summary>
	private float lifetime = 4f;

	#endregion

	#region Unity References 

	private void OnTriggerEnter(Collider Object)
	{
		if (Object.transform == transform)
		{
			return;
		}
		else
		{
			HandleCollision();
		}
	}

	/// <summary>
	///		Handles the collision of game objects 
	/// </summary>
	private void HandleCollision()
	{
		Collider[] affected = Physics.OverlapSphere(transform.position, damageRadius, DestroyableObject);

		for (int i = 0; i < affected.Length; i++)
		{
			Rigidbody rb = affected[i].GetComponent<Rigidbody>();

			if (!rb)
			{
				Debug.LogWarning("Could not find Rigidbody in AEK projectile!");
				continue;
			}

			rb.AddExplosionForce(force, transform.position, damageRadius);

			float damageInflicted = CalculateDamageFromRange(rb.position);

			FireModeEvents.OnReceivedDamageEvent?.Invoke(rb.transform, -damageInflicted);
		}

		// Create explosion 
		GameObject explode = Instantiate(Explosion, transform.position, Explosion.transform.rotation);

		// Destroy the game object
		Destroy(explode, lifetime);
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
		float relativeDistance = (damageRadius - explosiveDist) / damageRadius;

		float damage = relativeDistance * maximumDamage;

		damage = Mathf.Max(0, damage);

		return damage;
	}

	#endregion
}
