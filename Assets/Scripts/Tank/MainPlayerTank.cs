#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion


[System.Serializable]
/// <summary>
/// Main Player Tank Character
/// </summary>
public class MainPlayerTank : MonoBehaviour
{

	#region Public Variables 
	/// <summary>
	///     Enables / Disables the tanks movement 
	/// </summary>
	public bool enableTankMovement = false;

    /// <summary>
    ///     Tank Controls Data Class Instance
    /// </summary>
    public TankControls tankControls = new TankControls(); 
    
    /// <summary>
    ///  Tank Health Data Class Instance
    /// </summary>
    public TankHealth tankHealth = new TankHealth();
   
    /// <summary>
    ///     Tank Movement Data Class Instance 
    /// </summary>
    public TankMovement tankMovement = new TankMovement();
   
    /// <summary>
    ///     Tank Primary Weapon Data Class Instance 
    /// </summary>
    public TankPrimaryWeapon tankPrimary = new TankPrimaryWeapon(); 
    
    /// <summary>
    ///     Prefab for handling when a player has been eliminated 
    /// </summary>
    public GameObject deathExplosionPrefab;

	#endregion

	#region Private Variables 
	/// <summary>
	///     Whether the cursor should be locked on play
	/// </summary>
	private bool CursorIsLocked = false;

	#endregion

	#region Unity References 

	#region Unity Event Methods 
	/// <summary>
	///     OnEnable Event Methods for the Tank Instance
	/// </summary>
	private void OnEnable()
    {
        FireModeEvents.OnObjectDestroyedEvent += OnDeath; // called when a player is destroyed
        FireModeEvents.OnReceivedDamageEvent += OnDamageReceived; // called when a player is hit 
        FireModeEvents.OnWaveStartedEvent += EnableInput; // assign our tank movement function to the game started event
    }

    /// <summary>
    ///  OnDisable Event Methods for the Tank Instance 
    /// </summary>
    private void OnDisable()
    {
        FireModeEvents.OnObjectDestroyedEvent -= OnDeath; 
        FireModeEvents.OnReceivedDamageEvent -= OnDamageReceived; 
        FireModeEvents.OnWaveStartedEvent -= EnableInput; 
    }

	#endregion


	#region Unity Methods 
	   
    /// <summary>
    ///     Checks for the cursor's lock state 
    /// </summary>
    private void Awake()
	{
		if (Cursor.lockState == CursorLockMode.None)
		{
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            CursorIsLocked = true;
		}
	}

	/// <summary>
    ///     Called before the first frame update - Calls the tanks setup 
    /// </summary>
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

    /// <summary>
    ///     Locks the mouse cursor
    ///     Handles the tanks aiming and mouse input 
    /// </summary>
    private void Update()
    {
        LockCursor();

        if (!enableTankMovement)
		{
            return;
		}

        // Handles the aiming for the turret on the horizontal and vertical axis 
        tankMovement.HandleAiming(tankControls.ReturnMouseInput());
    }

    /// <summary>
    ///     Handles the tanks movement at a fixed update rate ( Physics ) 
    /// </summary>
	private void FixedUpdate()
	{
        if (!enableTankMovement)
		{
            return;
		}


        // Handles Basic movement position & rotation from player input (W,A,S,D)
        tankMovement.HandleMovement(tankControls.ReturnKeyValue(TankControls.KeyType.Movement), tankControls.ReturnKeyValue(TankControls.KeyType.Rotation));
        // Handles shooting of the primary weapon (Mouse0, Mouse1)
        tankPrimary.UpdateMainGun(tankControls.ReturnKeyValue(TankControls.KeyType.Fire));
    }

	#endregion

	#endregion

	#region Private Methods 

	/// <summary>
	/// Enables our tank to recieve input via the OnEnable Event 
	/// </summary>
	private void EnableInput()
    {
        tankMovement.EnableTankMovement(true);
        tankMovement.EnableTankAiming(true);
        tankPrimary.EnableShooting(true);
    }
    
    /// <summary>
    ///     Locks & hides the players cursor on game start 
    /// </summary>
    private void LockCursor()
	{
        if (Input.GetKeyDown(KeyCode.Escape) && !CursorIsLocked)
		{
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            CursorIsLocked = true;
		}
        else if (Input.GetKeyDown(KeyCode.Escape) && CursorIsLocked)
		{
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            CursorIsLocked = false;
		}
	}

    /// <summary>
    /// Called when an objects takes damage, if the object taking damage is this tank
    /// deal damage to it, else ignore it.
    /// </summary>
    /// <param name="TankTransform"></param>
    /// <param name="AmountOfDamage"></param>
    private void OnDamageReceived(Transform TankTransform, float AmountOfDamage)
    {
        if (TankTransform != transform)
        {
            return;
        }
        else
        {
            // if the Tank transform coming in, isn't this particular tank, ignore it.
            Debug.Log("[MainPlayerTank.TankTakenDamage]: " + "Tank has taken damage: " + AmountOfDamage);
            tankHealth.ApplyHealthChange(AmountOfDamage);
        }
    }

    /// <summary>
    /// Called when the object destroyed event has been called
    /// </summary>
    /// <param name="PlayerToKill"></param>
    private void OnDeath(Transform PlayerToKill)
    {
        // If the object that has been destroyed is not equal to the current transform 
        if (PlayerToKill != transform)
        {
            // Then we want to return.
            return;
        }
        else
		{
            // Clone the explosion gameobject prefab 
            GameObject clone = Instantiate(deathExplosionPrefab, transform.position, deathExplosionPrefab.transform.rotation); // spawn in our explosion effect
            Destroy(clone, 2); // just cleaning up our particle effect

            gameObject.SetActive(false); // turn off our game object as the player is now dead 

            // Call the game over event, as the player has died! 
            FireModeEvents.OnResetWaveEvent?.Invoke();
        }
    }

	#endregion

}
