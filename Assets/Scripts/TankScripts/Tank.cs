using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerNumber { One = 1, Two = 2, Three = 3, Four = 4 } // the number for our players
/// <summary>
/// The main class of our tank
/// Everything should be run from here.
/// </summary>
public class Tank : MonoBehaviour
{
    public bool enableTankMovement = false;
    public bool enableTankAimDownSight = false;
    public PlayerNumber playerNumber; // the number of our players tank
    public float MouseSensitivity = 1f;
    public TankControls tankControls = new TankControls(); // creating a new instance of our tank controls
    public TankHealth tankHealth = new TankHealth(); // creating a new instance of our tank health data class.
    public TankMovement tankMovement = new TankMovement(); // creating a new instance of our tank movement script
    public TankMainGun tankMainGun = new TankMainGun(); // creating a new instance of our tank main gun script
    public GameObject deathExplosionPrefab; // the prefab we will use when we have 0 left to make it go boom!
   
    /// <summary>
    ///     OnEnable Event Methods for the Tank Instance
    /// </summary>
    private void OnEnable()
    {
        TankGameEvents.OnObjectDestroyedEvent += Dead; // add dead function to the event for when a tank is destroyed
        TankGameEvents.OnObjectTakeDamageEvent += TankTakenDamage; // assign our health function to our event so we can take damage
        TankGameEvents.OnGameStartedEvent += EnableInput; // assign our tank movement function to the game started event
    }

    /// <summary>
    ///  OnDisable Event Methods for the Tank Instance 
    /// </summary>
    private void OnDisable()
    {
        TankGameEvents.OnObjectDestroyedEvent -= Dead; // add dead function to the event for when a tank is destroyed
        TankGameEvents.OnObjectTakeDamageEvent -= TankTakenDamage; // assign our health function to our event so we can take damage
        TankGameEvents.OnGameStartedEvent -= EnableInput; // assign our tank movement function to the game started event
    }

    // Start is called before the first frame update
    void Start()
    {   
        tankHealth.SetUp(transform); // call the set up function of our tank health script
        tankMovement.SetUp(transform); // calls the set up function of our tank health script
        tankMainGun.SetUp(); // calls the set up function of our tank main gun script

        // If the tank is allowed to move 
        if(enableTankMovement && enableTankAimDownSight)
        {
            // Enable tank input 
            EnableInput();
        }
    }

    // Fixed Update is called once per frame at a fixed rate (Mouse Sensitivity is smoother at a fixed update)
    private void FixedUpdate()
    {
      
        // Handles Basic movement position & rotation from player input (W,A,S,D)
        tankMovement.HandleMovement(tankControls.ReturnKeyValue(TankControls.KeyType.Movement), tankControls.ReturnKeyValue(TankControls.KeyType.Rotation)); 

        // Handles Aiming the Tank's Turret - Mouse X & Y Axis  
        tankMovement.HandleAiming(tankControls.ReturnMouseInput(MouseSensitivity));

        // Handles the Tanks Main Weapon Fire & Aim Down Sight (Mouse0, Mouse1)
        tankMainGun.UpdateMainGun(tankControls.ReturnKeyValue(TankControls.KeyType.Fire), tankControls.ReturnKeyValue(TankControls.KeyType.Aim)); // grab the input from the fire key
    }

    /// <summary>
    /// Enables our tank to recieve input
    /// </summary>
    private void EnableInput()
    {
        tankMovement.EnableTankMovement(true);
        tankMovement.EnableTankAiming(true);
        tankMainGun.EnableShooting(true);
        tankMainGun.EnableAimDownSight(true);
    }

    /// <summary>
    /// Called when an objects takes damage, if the object taking damage is this tank
    /// deal damage to it, else ignore it.
    /// </summary>
    /// <param name="TankTransform"></param>
    /// <param name="AmountOfDamage"></param>
    private void TankTakenDamage(Transform TankTransform, float AmountOfDamage)
    {
        Debug.Log("[Tank.TankTakenDamage]: " + " Tank has taken damage!");
        // if the Tank transform coming in, isn't this particular tank, ignore it.
        if (TankTransform != transform)
        {
            Debug.LogWarning("[Tank.TankTakenDamage]: " + " Thats not the correct tank transform");
            return;
        }
        else
        {
            Debug.Log("[Tank.TankTakenDamage]: " + AmountOfDamage);
            tankHealth.ApplyHealthChange(AmountOfDamage);
        }
    }

    /// <summary>
    /// Called when the object destroyed event has been called
    /// </summary>
    /// <param name="ObjectDestroyed"></param>
    private void Dead(Transform ObjectDestroyed)
    {
        // If the object that has been destroyed is not equal to the current transform 
        if (ObjectDestroyed != transform)
        {
            // Then we want to return.
            return;
        }

        // Clone the explosion gameobject prefab 
        GameObject clone = Instantiate(deathExplosionPrefab, transform.position, deathExplosionPrefab.transform.rotation); // spawn in our explosion effect
        Destroy(clone, 2); // just cleaning up our particle effect
        gameObject.SetActive(false); // turn off our tank as we are dead
    }
}
