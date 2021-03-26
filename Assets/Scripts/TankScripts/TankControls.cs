using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class is just hold the data for our player controls.
/// </summary>
[System.Serializable]
public class TankControls
{
    public enum KeyType { Movement, Rotation, Fire, Aim };

    public KeyCode forward = KeyCode.W; // the forward button to use
    public KeyCode backwards = KeyCode.S; // the backwards button
    public KeyCode left = KeyCode.A; // the left button
    public KeyCode right = KeyCode.D; // the right button
    public KeyCode fireButton = KeyCode.Mouse0; // the button to fire
    public KeyCode adsButton = KeyCode.Mouse1; // the aim down sight button (ads)
    public KeyCode changeCamera = KeyCode.C;
    private bool fireButtonWasPressed = false; // has the fire button been pressed?
    private bool adsButtonWasPressed = false; // the aim down sight button was pressed 
 
    
    /// <summary>
    /// If the value returned is postive then the postive axis has been pressed for that key.
    /// if the value returned is negative then the negative axis as been pressed
    /// if the value is 0 then no button has been pressed
    /// </summary>
    /// <param name="codeToCheck"></param>
    /// <returns></returns>
 
    public float ReturnKeyValue(KeyType codeToCheck)
    {
        float currentValue = 0; // the current input value of the code to check

        switch (codeToCheck)
        {
            case KeyType.Movement:
                {
                    if (Input.GetKey(forward)) // if we are pressing the forward button
                    {
                        currentValue = 1; // we moving positively.
                    }
                    else if (Input.GetKey(backwards)) // if pressing backwards button
                    {
                        currentValue = -1; // we are moving negatively
                    }
                    break;
                }
            case KeyType.Rotation:
                {
                  if (Input.GetKey(right)) // if we are pressing the right button
                    {
                        currentValue = 1; // we are rotating positively.
                    }
                    else if (Input.GetKey(left))// if we are pressing the left button
                    {
                        currentValue = -1; // we are rotating negatively
                    }
                    break;
                }
            case KeyType.Fire:
                {
                    if (Input.GetKey(fireButton)) // if we are pressing the fire button
                    {
                        currentValue = 1; // the fire button is pressed
                        fireButtonWasPressed = true;
                        //Debug.Log("Fire Pressed");
                    }
                    else if (Input.GetKeyUp(fireButton) && fireButtonWasPressed == true)
                    {
                        fireButtonWasPressed = false;
                        currentValue = -1;
                        //Debug.Log("Fire Released");
                    }
                    break;
                }

            case KeyType.Aim:
				{
                    if (Input.GetKey(adsButton)) // if we are pressing the aim down sight button 
					{
                        currentValue = 1; // the aim down sigt button was pressed 
					    adsButtonWasPressed = true;
                    }
                    else if (Input.GetKeyUp(adsButton) && adsButtonWasPressed == true)
					{
                        adsButtonWasPressed = false;
                        currentValue = -1;
					}
                   break;
				}
        }

        return currentValue;
    }



    /// <summary>
    ///     Returns a Vector 2 (x, y) for mouse input 
    /// </summary>
    /// <param name="MouseSensitivity">The desired sensitivity for mouse movement</param>
    /// <returns></returns>
    public Vector2 ReturnMouseInput(float MouseSensitivity)
	{
        float x = Input.GetAxis("Mouse X") * MouseSensitivity * Time.fixedDeltaTime;
        float y = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.fixedDeltaTime;

        // Debugging Mouse Input 
        // Debug.Log("[TankControls.ReturnMouseInput]: " + "Mouse Input: (" + x + ", " + y + ")");
        return new Vector2(x, y).normalized;

    }
}
  