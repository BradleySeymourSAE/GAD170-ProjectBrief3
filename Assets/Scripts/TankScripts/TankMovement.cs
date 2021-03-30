using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Handle everything to do with our tank movement
/// </summary>
[System.Serializable]
public class TankMovement 
{
    [Header("General Tank Movement Settings")]
    public float speed = 12f; // the speed our tank moves
    [Range(50f, 180f)]
    public float turnSpeed = 75f; // the speed that we can turn in degrees in seconds.
    public float mouseSensitivity = 100f;
   
    [Header("Camera Settings")]
    public float cameraTurnSmoothingTime = 0.1f;
    [SerializeField] float cameraTurnSmoothVelocity;

    [Header("Special Effects")]
    private TankParticleEffects tankParticleEffects = new TankParticleEffects(); // creating a new instance of our tank particle effects class
    public TankSoundEffects tankSoundEffects = new TankSoundEffects(); // creating a new instance of our tank sound effects class

    private Rigidbody m_rigidbody;// a reference to the rigidbody on our tank
    private bool enableMovement = true; // if this is true we are allowed to accept input from the player
    private bool enabledAiming = true; // Allowed to accept input for weapon aim down sight 
    private Transform m_tankReference; // a reference to the tank transform 
    public Transform MainTurret; // reference to the tanks turret
    private Transform m_cameraReference; // reference to the tanks camera  

    float desiredXAxisRotation;


    #region Public Methods 
    /// <summary>
    /// Handles the set up of our tank movement script
    /// </summary>
    /// <param name="Tank"></param>
    public void Setup(Transform Tank)
    {
        m_tankReference = Tank; // Reference to the tank transform

        if (m_tankReference.GetComponent<Rigidbody>())
        {
            // Get the rigidbody component attached to the gameobject
            m_rigidbody = m_tankReference.GetComponent<Rigidbody>(); // grab a reference to our tanks rigidbody
        }
        else
        {
            Debug.LogError("No Rigidbody attached to the tank");
        }


        // Check to see whether the tank reference has a camera component transform attached
        // to it 
        if (m_tankReference.GetComponentInChildren<Camera>().transform)
		{
            // Set the tanks camera reference 
            m_cameraReference = m_tankReference.GetComponentInChildren<Camera>().transform;
		}
        else
		{
            Debug.LogError("[TankMovement.SetUp]: " + "Could not find a camera component attached to the game object!");
		}


        tankParticleEffects.SetUpEffects(m_tankReference); // set up our tank particle effects
        tankSoundEffects.Setup(m_tankReference); // Set up the sounds effects for the tank
        tankParticleEffects.PlayDustTrails(true);// start playing tank particle effects
        EnableTankMovement(false); // Initially set enable tank movement to false.
        EnableTankAiming(false); // Initially set enable tank aiming to false.
    }


	/// <summary>
	/// Tells our tank if it's allowed to move or not
	/// </summary>
	/// <param name="Enabled"></param>
	public void EnableTankMovement(bool Enabled)
    {
        enableMovement = Enabled;
    }

    /// <summary>
    ///     Tells our tank if its allowed to aim down sight or not 
    /// </summary>
    /// <param name="Enabled"></param>
    public void EnableTankAiming(bool Enabled)
	{
        enabledAiming = Enabled;
	}

    /// <summary>
    /// Handles the basic movement of our tank
    /// </summary>
    public void HandleMovement(float ForwardMovement, float RotationMovement)
    {
        // if we can't move don't
        if(enableMovement == false)
        {
            return;
        }
        Move(ForwardMovement);
        Turn(RotationMovement);
        tankSoundEffects.PlayEngineSound(ForwardMovement, RotationMovement); // update our audio based on our input 
    }
	
    /// <summary>
    ///     Handles mouse aim for the tank 
    /// </summary>
    /// <param name="mouseDirection"></param>
    public void HandleAiming(Vector3 mouseDirection)
	{
        // If we can't aim, dont.
        if (enabledAiming == false)
		{
            return;
		}


        // Aim the turret using the players mouse direction 
        AimTurret(mouseDirection);
    }
    #endregion

    #region Private Methods 
    /// <summary>
    ///     Moves the tank forwards and backwards
    /// </summary>
    private void Move(float ForwardMovement)
    {
        // create a vector based on the forward vector of our tank, move it forwad or backwards on nothing based on the key input, multiplied by the speed, multipled by the time between frames rendered to make it smooth
        Vector3 movementVector = m_tankReference.forward * ForwardMovement * speed * Time.deltaTime;
        //Debug.Log(movementVector);
        m_rigidbody.MovePosition(m_rigidbody.position + movementVector); // move our rigibody based on our current position + our movement vector
    }
    /// <summary>
    ///     Rotates the tank on the Y axis
    /// </summary>
    private void Turn(float RotationalAmount)
    {
        // get the key input value, multiply it by the turn speed, multiply it by the time between frames
        float turnAngle = RotationalAmount * turnSpeed * Time.deltaTime; // the angle in degrees we want to turn our tank
        Quaternion turnRotation = Quaternion.Euler(0f, turnAngle, 0); // essentially turn our angle into a quarternion for our rotation

        // update our rigidboy with this new rotation
        m_rigidbody.MoveRotation(m_rigidbody.rotation * turnRotation); // rotate our rigidbody based on our input.
    }
	
    /// <summary>
    ///     Aims the turret on the x & y axis 
    /// </summary>
    /// <param name="AimVertical"></param>
    /// <param name="AimHorizontal"></param>
    private void AimTurret(Vector3 AimPosition)
    {            
        string weaponAimingKey = "T90_PrimaryWeaponAiming";

        AudioSource s = AudioManager.Instance.GetAudioSource(weaponAimingKey);

        // If the aim position magnitude is greater than 0.1f 
         if (AimPosition.magnitude >= 0.1f)
		{

            // If the weapon aim source isnt playing 
            if (s.isPlaying == false)
            { 
                AudioManager.Instance.PlaySound(weaponAimingKey);
            }
            // Otherwise, if audio source is playing and s.volume is less than 1 
            else if (s.isPlaying && s.volume < 1.0f)
			{
                // Debug.Log("Fading sound in!");
                // Fade the audio sound effect in to 100% over 0.75 of a second.
                AudioManager.Instance.FadeSoundEffect(weaponAimingKey, 100f, 0.75f);
            }
     

             // Desired camera x axis rotation 
             desiredXAxisRotation -= AimPosition.y;
             // Clamp vertical look degrees angle 
             desiredXAxisRotation = Mathf.Clamp(desiredXAxisRotation, 0, 0); 

             m_cameraReference.localRotation = Quaternion.Euler(desiredXAxisRotation, 0, 0);

            // Rotate the main turret gun on the y axis 
             MainTurret.Rotate(Vector3.up, AimPosition.x);
        }
         else
		{
            AudioManager.Instance.FadeSoundEffect(weaponAimingKey, 0f, 1f);
            // AudioManager.Instance.StopPlaying(weaponAimingKey);
		}            
	}
    #endregion
}

