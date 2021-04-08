using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCameraController : MonoBehaviour
{
    public float dampTime = 0.2f; // Approximate time it should take for our camera to focus on our tanks
    public float screenEdgeBuffer = 4; // space between the top and the bottom of targets
    public float minCameraSize = 6.5f; // the smallest othorgraphic camera size
    private List<GameObject> listOfTanks = new List<GameObject>(); // a reference to all the tanks in our scene

    private Camera cam; // a reference to our main camera 
    private float zoomSpeed; // the speed we be zooming in/out at
    private Vector3 moveVelocity; // reference velocity for smooth damping position - how fast we moving towards our desired position
    private Vector3 desiredPosition; // the position our camera should be moving towards

    public bool enableCameraOnStart = false;

    private void OnEnable()
    {
        cam = GetComponentInChildren<Camera>();
        TankGameEvents.OnTanksSpawnedEvent += Initalise; // add our initialise function
    }

    private void OnDisable()
    {
        TankGameEvents.OnTanksSpawnedEvent -= Initalise; // add our initialise function
    }

    private void Start()
    {
        if(enableCameraOnStart)
        {
            Tank[] allTanks = FindObjectsOfType<Tank>();
            List<GameObject> allTanksList = new List<GameObject>(); //list of all tanks

            for(int i=0; i<allTanks.Length; i++)
            {
                allTanksList.Add(allTanks[i].gameObject); // store the game object of the tank
            }

            Initalise(allTanksList);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move(); // move our camera
        Zoom(); // zoom it in to show all tanks
    }

    /// <summary>
    /// Called when the tanks have been spawned in and not before hand
    /// </summary>
    private void Initalise(List<GameObject> allTanks)
    {
        listOfTanks = allTanks; // set our reference to all the tanks list

        // find the average position
        FindAveragePosition();
        // set our transform to our immediate desired position
        transform.position = desiredPosition;
        // find the required size camera/ zoom in immediately
        cam.orthographicSize = CalculateRequiredSize();
    }


    /// <summary>
    /// Moves our camera to the desire position
    /// </summary>
    private void Move()
    {
        FindAveragePosition(); // find all the tanks and get the position average position between them all.

        // moving our position to the desired position at the velocity rate, over the damp time.
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref moveVelocity, dampTime);
    }

    /// <summary>
    /// Handles the zoomin in and out of our camera
    /// </summary>
    private void Zoom()
    {
        // find the required size based on the desired position and smoothly transition to that size
        float requiredSize = CalculateRequiredSize(); // calculate the required size of our camera
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, requiredSize, ref zoomSpeed, dampTime); // smoothly move towards our target size of our camera
    }

    /// <summary>
    /// This is going to loop through our tanks, and fid an optimal position so that they all be viewed by the camera
    /// </summary>
    /// <returns></returns>
    private float CalculateRequiredSize()
    {
        // finds the position the camera is moving towards in local space
        Vector3 desiredLocalPosition = transform.InverseTransformPoint(desiredPosition);

        float size = 0; // start the startin size to 0;

        // loop through all the tanks
        for(int i=0; i< listOfTanks.Count; i++)
        {
            if (listOfTanks[i].activeSelf == false)
            {
                // check to see if the tanks is enabled
                continue; // skip to the next element
            }
            else
            {
                Vector3 targetLocalPos = transform.InverseTransformPoint(listOfTanks[i].transform.position); // grab the local position of the tank relative to the camera

                // find the position of the desired target position of the camera's local space
                Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPosition; // grabbing the direction between 

                size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y)); // choose the largest out of our current sieze and the distance of the tank up or down from the camera

                // choose the largest out of the the current size and the calculated size based on the tank being to the left or the right of the camera
                size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / cam.aspect);
            }
        }
        // add the edge buffer to the maximum size required
        size += screenEdgeBuffer;

        // make sure the camera size isn't below the minimum size
        size = Mathf.Max(size, minCameraSize);

        return size;
    }

    /// <summary>
    /// This loops throughs our tanks and finds the tanks positions, and more or less finds the average beteween
    /// </summary>
    private void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3(); // creating a blank vector 3 i.e. 0,0,0
        int numTargets = 0; // the number of tanks we are trying to average

        for(int i=0; i<listOfTanks.Count; i++)
        {
            // loop through all our tanks
            if(listOfTanks[i].activeSelf == false)
            {
                // check to see if the tank is active if not jump to the next element in the list
                continue;
            }
            else
            {
                averagePos += listOfTanks[i].transform.position; // add the current tank to our position
                numTargets++; // increase the number of tank targets by one
            }
        }

        // do a check to make sure we are not dividing by 0
        if(numTargets >0)
        {
            averagePos /= numTargets; // get the average position from our targets
        }

        // keep the y position the same, so our camera doesn't move up and down
        averagePos.y = transform.position.y; // set the average pos y to our camera's y

        desiredPosition = averagePos; // set our desired position to our average position;
    }
}
