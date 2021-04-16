#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion


/// <summary>
///     Handles the Tank Shell Explosion 
/// </summary>
public class ShellExplosion : MonoBehaviour
{
    public GameObject explosionPrefab; // the explosion we want to spawn in
    public LayerMask AILayer; // the layer of the game object to effect
    public LayerMask PlayerLayer;
    public float maxDamage = 100f; // the maximum amount of damage that my shell can do.
    public float explosionForce = 1000f; // the amount of force this shell has
    public float maxShellLifeTime = 2f; // how long should the shell live for before it goes boom!
    public float explosionRadius = 5f; // how big is our explosion

	#region Private Methods 
	// is called when the trigger hits an object
	private void OnTriggerEnter(Collider other)
    {
        if (other.transform == transform)
        {
            // if we some how hit ourselves or another bullet
            // ignore it
            return;
        }
        else
        {
            Boom(); // we hit something go boom
        }
    }

    /// <summary>
    /// Called when the shell has hit an object in our scene
    /// </summary>
    private void Boom()
    {
        Collider[] aiColliders = Physics.OverlapSphere(transform.position, explosionRadius, AILayer); // draw a sphere, if any objects are on the tank layer, grab them and store them in this array
        Collider[] playerColliders = Physics.OverlapSphere(transform.position, explosionRadius, PlayerLayer); // draw a sphere if any objects are on the player layer, grab them and store them in this array 

        for (int i = 0; i < aiColliders.Length; i++)
		{
            Rigidbody targetAI = aiColliders[i].GetComponent<Rigidbody>();


            if (!targetAI)
			{
                continue;
			}

            targetAI.AddExplosionForce(explosionForce, transform.position, explosionRadius);

            float damage = CalculateDamage(targetAI.position);

              
            FireModeEvents.HandleAIDamageEvent?.Invoke(targetAI.transform, -damage);
		}

        for (int i = 0; i < playerColliders.Length; i++) // loop through all the colliders in the explosion
        {
            if (playerColliders[i].GetComponent<MainPlayerTank>())
            { 
                Rigidbody playerTargetRigidbody = aiColliders[i].GetComponent<Rigidbody>(); // grab a reference to the rigidbody
           
            if (!playerTargetRigidbody)
            {
                Debug.Log("Player Target Has No Rigidbody Ignoring");
                continue; // if there is no rigidbody continue on to the next element, so skip the rest of this code below.
            }

                 playerTargetRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius); // add a force at the point of impact


                 float damage = CalculateDamage(playerTargetRigidbody.position); // calculate the damage based on the distanc
             
                // Assign the damage value to the player 
                FireModeEvents.HandlePlayerDamageEvent?.Invoke(playerTargetRigidbody.transform, -damage);
            }
        }

        // spawn in our explosion effect
        GameObject clone = Instantiate(explosionPrefab, transform.position, explosionPrefab.transform.rotation);
        Destroy(clone, maxShellLifeTime);

    }

    /// <summary>
    /// based on the target position, calculate the amount of damage to deal
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <returns></returns>
    private float CalculateDamage(Vector3 targetPosition)
    {
        Vector3 explosionToTarget = targetPosition - transform.position; // get the direction of the explosion compared to our main explosion point
        float explosionDistance = explosionToTarget.magnitude; // the length of the explosion target vector
        float relativeDistance = (explosionRadius - explosionDistance) / explosionRadius; // calculate the portoion of the explosion distance that we are engulfed in

        float damage = relativeDistance * maxDamage; // multiple the distance by the max damage
        damage = Mathf.Max(0f, damage); // get biggest value between the two
        return damage;
    }

	#endregion

}
