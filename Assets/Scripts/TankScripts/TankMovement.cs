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
    public float turnSpeed = 180f; // the speed that we can turn in degrees in seconds.
    
    [Header("Camera Settings")]
    public Transform cameraSlot;
    public Transform mainTurret;
    [SerializeField] float cameraClampMaximum = 90f;
    [SerializeField] float cameraSensitivity = 2.5f;
    

    [Header("Special Effects")]
    private TankParticleEffects tankParticleEffects = new TankParticleEffects(); // creating a new instance of our tank particle effects class
    public TankSoundEffects tankSoundEffects = new TankSoundEffects(); // creating a new instance of our tank sound effects class

    private Rigidbody m_rigidbody;// a reference to the rigidbody on our tank
    private bool enableMovement = true; // if this is true we are allowed to accept input from the player
    private Transform m_tankReference; // a reference to the tank gameobject
    
    float m_desiredCameraRotationX;
    float m_mainTurretRotationY;
    TankCameraControls m_tankCameraControls;
   

    /// <summary>
    /// Handles the set up of our tank movement script
    /// </summary>
    /// <param name="Tank"></param>
    public void SetUp(Transform Tank)
    {
        m_tankReference = Tank;
        if (m_tankReference.GetComponent<Rigidbody>())
        {
            m_rigidbody = m_tankReference.GetComponent<Rigidbody>(); // grab a reference to our tanks rigidbody
        }
        else
        {
            Debug.LogError("No Rigidbody attached to the tank");
        }


        // Check the tank reference for camera controls component 

        if (m_tankReference.GetComponent<TankCameraControls>())
		{
            m_tankCameraControls = m_tankReference.GetComponent<TankCameraControls>(); // reference to tanks camera controls component 
		}
        else
		{
            Debug.LogError("No Tank Camera Controls Attached to the Tank!");
		}



        tankParticleEffects.SetUpEffects(m_tankReference); // set up our tank effects
        tankSoundEffects.SetUp(m_tankReference);
        tankParticleEffects.PlayDustTrails(true);// start playing tank particle effects
        EnableTankMovement(false);
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
    ///     Handles the tanks aiming for firing projectiles
    /// </summary>
   public void HandleCameraLook()
    {
       // Desired Look Camera Rotation X 

       m_desiredCameraRotationX = Mathf.Clamp(m_desiredCameraRotationX - (m_tankCameraControls.mouseControls().y), -cameraClampMaximum, cameraClampMaximum);
       
        cameraSlot.localRotation = Quaternion.Lerp(cameraSlot.localRotation, 
                                  Quaternion.Euler(m_desiredCameraRotationX, 0f, 0f),
                                  cameraSensitivity * Time.fixedDeltaTime);
        
        // Get the Main turrets rotation on the Y Axis 
       m_mainTurretRotationY = Mathf.Lerp(
           m_mainTurretRotationY,
           m_tankCameraControls.mouseControls().x, 
           cameraSensitivity * Time.fixedDeltaTime
        );

        
      // Rotate the turrent on the Y Axis
        Vector3 forward = mainTurret.transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(mainTurret.transform.position, forward, Color.Lerp(Color.red, Color.green, Time.deltaTime));
      mainTurret.transform.Rotate(Vector3.up, m_mainTurretRotationY);
    }

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

}

