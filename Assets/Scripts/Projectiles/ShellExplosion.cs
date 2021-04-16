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
    public LayerMask ObjectLayer;
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
       
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, ObjectLayer);



        for (int i = 0; i < colliders.Length; i++)
		{

            Rigidbody s_Target = colliders[i].GetComponent<Rigidbody>();


            if (!s_Target)
			{
                // We could log something here to debug test, however every time the shell hits the ground this will be called as it doesnt have a rb 
                continue;
			}


            s_Target.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            float s_DamageToApply = CalculateDamage(s_Target.position);

            if (colliders[i].GetComponent<AI>())
			{
                Debug.LogWarning("[ShellExplosion.Boom]: " + "Applying " + s_DamageToApply + " to AI CHARACTER!");
                FireModeEvents.HandleAIDamageEvent?.Invoke(colliders[i].transform, -s_DamageToApply);
			}
            else if (colliders[i].GetComponent<MainPlayerTank>())
			{
                Debug.LogWarning("[ShellExplosion.Boom]: " + "Applying " + s_DamageToApply + " to Main Player!");
                FireModeEvents.HandlePlayerDamageEvent?.Invoke(colliders[i].transform, -s_DamageToApply);
			}
        }




        // spawn in our explosion! 
        GameObject clone = Instantiate(explosionPrefab, transform.position, explosionPrefab.transform.rotation);
        
        //  Destroy the game object!
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
