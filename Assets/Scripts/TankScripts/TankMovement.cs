using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Handle everything to do with our tank movement
/// </summary>
[System.Serializable]
public class TankMovement 
{
    public float speed = 12f; // the speed our tank moves
    public float turnSpeed = 180f; // the speed that we can turn in degrees in seconds.

    private TankParticleEffects tankParticleEffects = new TankParticleEffects(); // creating a new instance of our tank particle effects class
    public TankSoundEffects tankSoundEffects = new TankSoundEffects(); // creating a new instance of our tank sound effects class

    private Rigidbody rigidbody;// a reference to the rigidbody on our tank
    private bool enableMovement = true; // if this is true we are allowed to accept input from the player

    private Transform tankReference; // a reference to the tank gameobject


    /// <summary>
    /// Handles the set up of our tank movement script
    /// </summary>
    /// <param name="Tank"></param>
    public void SetUp(Transform Tank)
    {
        tankReference = Tank;
        if (tankReference.GetComponent<Rigidbody>())
        {
            rigidbody = tankReference.GetComponent<Rigidbody>(); // grab a reference to our tanks rigidbody
        }
        else
        {
            Debug.LogError("No Rigidbody attached to the tank");
        }
        tankParticleEffects.SetUpEffects(tankReference); // set up our tank effects
        tankSoundEffects.SetUp(tankReference);
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
    /// Handles the movement of our tank
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
    /// Moves the tank forward and back
    /// </summary>
    private void Move(float ForwardMovement)
    {
        // create a vector based on the forward vector of our tank, move it forwad or backwards on nothing based on the key input, multiplied by the speed, multipled by the time between frames rendered to make it smooth
        Vector3 movementVector = tankReference.forward * ForwardMovement * speed * Time.deltaTime;
        //Debug.Log(movementVector);
        rigidbody.MovePosition(rigidbody.position + movementVector); // move our rigibody based on our current position + our movement vector
    }
    /// <summary>
    /// Rotates the tank on the Y axis
    /// </summary>
    private void Turn(float RotationalAmount)
    {
        // get the key input value, multiply it by the turn speed, multiply it by the time between frames
        float turnAngle = RotationalAmount * turnSpeed * Time.deltaTime; // the angle in degrees we want to turn our tank
        Quaternion turnRotation = Quaternion.Euler(0f, turnAngle, 0); // essentially turn our angle into a quarternion for our rotation

        // update our rigidboy with this new rotation
        rigidbody.MoveRotation(rigidbody.rotation * turnRotation); // rotate our rigidbody based on our input.
    }
}

