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
   
    [Header("Camera Settings")]
    public float cameraTurnSmoothingTime = 0.1f;
    [SerializeField] float cameraTurnSmoothVelocity;

    [Header("Special Effects")]
    private TankParticleEffects tankParticleEffects = new TankParticleEffects(); // creating a new instance of our tank particle effects class
    public TankSoundEffects tankSoundEffects = new TankSoundEffects(); // creating a new instance of our tank sound effects class

    private Rigidbody m_rigidbody;// a reference to the rigidbody on our tank
    private bool enableMovement = true; // if this is true we are allowed to accept input from the player
    private bool enabledAiming = true; // Allowed to accept input for weapon aim down sight 
    private Transform m_tankReference; // a reference to the tank gameobject
    public Transform m_turret; // reference to the tanks turret
 
    private Transform m_cameraReference; // a reference to the tank's camera 

    #region Public Methods 
    /// <summary>
    /// Handles the set up of our tank movement script
    /// </summary>
    /// <param name="Tank"></param>
    public void SetUp(Transform Tank)
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
        tankSoundEffects.SetUp(m_tankReference); // Set up the sounds effects for the tank
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
        tankSoundEffects.PlayTankEngine(ForwardMovement, RotationMovement); // update our audio based on our input 
    }
	
    /// <summary>
    ///     Handles mouse aim for the tank 
    /// </summary>
    /// <param name="mouseDirection"></param>
    public void HandleAiming(Vector2 mouseDirection)
	{
       float xAimRotation = mouseDirection.x;
       float yAimRotation = mouseDirection.y;

        // Debugging Purposes
       // Debug.Log("[TankMovement.HandleAiming]: " + "Mouse X: " + xAimRotation + " Mouse Y: " + yAimRotation);


        // If Aiming is not enabled return.
        if (enabledAiming == false)
		{
            return;
		}

        AimTurret(xAimRotation, yAimRotation);
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
    /// <param name="Horizontal"></param>
    /// <param name="Vertical"></param>
    private void AimTurret(float Horizontal, float Vertical)
	{

        if (m_turret == null)
		{
            Debug.LogError("[TankMovement.AimTurret]: " + "Main turret transform is null!");
		    return;
        }

        Vector3 direction = new Vector3(Horizontal, 0f, Vertical);


        // If the movement direction magnitude is greater than or equal to 1f 
        // We want to aim in this direction 
        if (direction.magnitude >= 0.1f)
		{

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + m_cameraReference.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(m_turret.transform.eulerAngles.y, targetAngle, ref cameraTurnSmoothVelocity, cameraTurnSmoothingTime);


            m_turret.rotation = Quaternion.Euler(0f, angle, 0f);

		}
	}
    #endregion
}

