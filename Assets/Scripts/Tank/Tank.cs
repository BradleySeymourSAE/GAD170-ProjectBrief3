using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Player { One = 1, Two = 2, Three = 3, Four = 4 };


/// <summary>
/// The main class of our tank
/// Everything should be run from here.
/// </summary>
public class Tank : MonoBehaviour
{
    public bool enableTankMovement = false;
    public Player playerNumber; // the number of our players tank
    public TankControls tankControls = new TankControls(); // creating a new instance of our tank controls
    public TankHealth tankHealth = new TankHealth(); // creating a new instance of our tank health data class.
    public TankMovement tankMovement = new TankMovement(); // creating a new instance of our tank movement script
    public TankPrimaryWeapon tankPrimary = new TankPrimaryWeapon(); // primary weapon instance 
    public GameObject deathExplosionPrefab; // the prefab we will use when we have 0 left to make it go boom

    [SerializeField] private bool CursorIsLocked;

	/// <summary>
	///     OnEnable Event Methods for the Tank Instance
	/// </summary>
	private void OnEnable()
    {
        FireModeEvents.OnObjectDestroyedEvent += OnDeath; // add dead function to the event for when a tank is destroyed
        FireModeEvents.OnDamageReceivedEvent += OnHit; // assign our health function to our event so we can take damage
        FireModeEvents.SpawnPlayerEvent += EnableInput; // assign our tank movement function to the game started event
    }

    /// <summary>
    ///  OnDisable Event Methods for the Tank Instance 
    /// </summary>
    private void OnDisable()
    {
        FireModeEvents.OnObjectDestroyedEvent -= OnDeath; // add dead function to the event for when a tank is destroyed
        FireModeEvents.OnDamageReceivedEvent -= OnHit; // assign our health function to our event so we can take damage
        FireModeEvents.SpawnPlayerEvent -= EnableInput; // assign our tank movement function to the game started event
    }

    // Start is called before the first frame update
    void Start()
    {  
        tankHealth.Setup(transform); // call the set up function of our tank health script
        tankMovement.Setup(transform); // calls the set up function of our tank health script
        tankPrimary.Setup(transform); // calls primary weapon setup


        // If the tank is allowed to move 
        if(enableTankMovement)
        {
            // Enable tank input 
            EnableInput();
        }
    }

    //  Update is called once per frame at a fixed rate (Mouse Sensitivity is smoother at a fixed update)
    private void Update()
    {
        // Hanldes locking of the cursor...
        LockCursor();


        // Handles Basic movement position & rotation from player input (W,A,S,D)
        tankMovement.HandleMovement(tankControls.ReturnKeyValue(TankControls.KeyType.Movement), tankControls.ReturnKeyValue(TankControls.KeyType.Rotation)); 
        

        // Handles the aiming for the turret on the horizontal and vertical axis 
        tankMovement.HandleAiming(tankControls.ReturnMouseInput());

        // Handles shooting of the primary weapon (Mouse0, Mouse1)
        tankPrimary.UpdateMainGun(tankControls.ReturnKeyValue(TankControls.KeyType.Fire));
    }

    /// <summary>
    /// Enables our tank to recieve input via the OnEnable Event 
    /// </summary>
    private void EnableInput()
    {
        tankMovement.EnableTankMovement(true);
        tankMovement.EnableTankAiming(true);
        tankPrimary.EnableShooting(true);
    }

    private void LockCursor()
	{
        if (Input.GetKeyDown(KeyCode.Escape) && !CursorIsLocked)
		{
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = (false);
            CursorIsLocked = true;
		}
        else if (Input.GetKeyDown(KeyCode.Escape) && CursorIsLocked)
		{
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = (true);
            CursorIsLocked = false;
		}
	}

    /// <summary>
    /// Called when an objects takes damage, if the object taking damage is this tank
    /// deal damage to it, else ignore it.
    /// </summary>
    /// <param name="TankTransform"></param>
    /// <param name="AmountOfDamage"></param>
    private void OnHit(Transform TankTransform, float AmountOfDamage)
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
    private void OnDeath(Transform ObjectDestroyed)
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
    
    
    
       // Call the game over event, as the player has died! 
       FireModeEvents.OnGameOverEvent?.Invoke();
    }
}
