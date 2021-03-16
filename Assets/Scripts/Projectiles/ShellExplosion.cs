using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    public GameObject explosionPrefab; // the explosion we want to spawn in
    public LayerMask tankLayer; // the layer of the game object to effect
    public float maxDamage = 100f; // the maximum amount of damage that my shell can do.
    public float explosionForce = 1000f; // the amount of force this shell has
    public float maxShellLifeTime = 2f; // how long should the shell live for before it goes boom!
    public float explosionRadius = 5f; // how big is our explosion


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
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, tankLayer); // draw a sphere, if any objects are on the tank layer, grab them and store them in this array

        for (int i = 0; i < colliders.Length; i++) // loop through all the colliders in the explosion
        {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>(); // grab a reference to the rigidbody
            if(!targetRigidbody)
            {
                Debug.Log("Target Has No Rigidbody Ignoring");
                continue; // if there is no rigidbody continue on to the next element, so skip the rest of this code below.
            }

            targetRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius); // add a force at the point of impact


            float damage = CalculateDamage(targetRigidbody.position); // calculate the damage based on the distance
            TankGameEvents.OnObjectTakeDamageEvent?.Invoke(targetRigidbody.transform, -damage); // invoke our take damage event
        }

        // spawn in our explosion effect
        GameObject clone = Instantiate(explosionPrefab, transform.position, explosionPrefab.transform.rotation);
        Destroy(clone,maxShellLifeTime);

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

}
