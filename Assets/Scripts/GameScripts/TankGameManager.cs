using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankGameManager : MonoBehaviour
{

    public float preGameWaitTime = 3f;
    private List<Tank> allTanksSpawnedIn = new List<Tank>(); // a list of all the tanks that we spawned in

    private int playerOneScore;
    private int playerTwoScore;
    private int playerThreeScore;
    private int playerFourscore;

    private void OnEnable()
    {
        TankGameEvents.OnTanksSpawnedEvent += TanksSpawned; //add our tanks spawned function
        TankGameEvents.OnObjectDestroyedEvent += TankDespawned;
    }

    private void OnDisable()
    {
        TankGameEvents.OnTanksSpawnedEvent -= TanksSpawned; //add our tanks spawned function
        TankGameEvents.OnObjectDestroyedEvent -= TankDespawned;
    }

    /// <summary>
    /// Is called when the tanks are spawned in
    /// </summary>
    /// <param name="tanksSpawnedIn"></param>
    private void TanksSpawned(List<GameObject> tanksSpawnedIn)
    {
        allTanksSpawnedIn.Clear();
        for(int i=0; i<tanksSpawnedIn.Count; i++)
        {
            if(!tanksSpawnedIn[i].GetComponent<Tank>())
            {
                continue; // checks to see if it has a tank script, if it does then add it to the list, otherwise continue to the next element
            }
            Tank tempTank = tanksSpawnedIn[i].GetComponent<Tank>(); // store a reference to the tank script
            allTanksSpawnedIn.Add(tempTank);

            UpdateScores();
        }
    }

    private void UpdateScores()
    {
        for(int i=0; i<allTanksSpawnedIn.Count; i++)
        {
            switch (allTanksSpawnedIn[i].playerNumber)
            {
                case PlayerNumber.One:
                    {
                        TankGameEvents.OnScoreUpdatedEvent?.Invoke(allTanksSpawnedIn[i].playerNumber, playerOneScore);
                        break;
                    }
                case PlayerNumber.Two:
                    {
                        TankGameEvents.OnScoreUpdatedEvent?.Invoke(allTanksSpawnedIn[i].playerNumber, playerTwoScore);
                        break;
                    }
                case PlayerNumber.Three:
                    {
                        TankGameEvents.OnScoreUpdatedEvent?.Invoke(allTanksSpawnedIn[i].playerNumber, playerThreeScore);
                        break;
                    }
                case PlayerNumber.Four:
                    {
                        TankGameEvents.OnScoreUpdatedEvent?.Invoke(allTanksSpawnedIn[i].playerNumber, playerFourscore);
                        break;
                    }
            }       
        }
    }

    /// <summary>
    /// Called when a tank is despawned
    /// </summary>
    /// <param name="tankDespawned"></param>
    private void TankDespawned(Transform tankDespawned)
    {
        if(tankDespawned.GetComponent<Tank>() == null)
        {
            return; // jump out of here if the object coming in doesn't tank script
        }

        allTanksSpawnedIn.Remove(tankDespawned.GetComponent<Tank>()); // remove the tank despawned

        //check to see how tanks are left, if there is one, then declare it the winner
        if(allTanksSpawnedIn.Count <=1)
        {
            // we have one tank left in theory.
            //delclare that player the winner
            Debug.Log("Winner is" + allTanksSpawnedIn[0].playerNumber.ToString());
            // check to see which player is left and give them a point
            if ((int)allTanksSpawnedIn[0].playerNumber == 1)
            {
                playerOneScore++;
            }
            else if ((int)allTanksSpawnedIn[0].playerNumber == 2)
            {
                playerTwoScore++;
            }
            else if ((int)allTanksSpawnedIn[0].playerNumber == 3)
            {
                playerThreeScore++;
            }
            else if ((int)allTanksSpawnedIn[0].playerNumber == 4)
            {
                playerFourscore++;
            }

            // update our score
            UpdateScores();
            TankGameEvents.OnRoundEnededEvent?.Invoke(allTanksSpawnedIn[0].playerNumber);

            // reset our round
            Invoke("ResetRound", 3f); // after 3 seconds reset our round
            
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GameLogic()); // start running our game logic 
    }

    private void ResetRound()
    {
        TankGameEvents.OnRoundResetEvent?.Invoke();
        TankGameEvents.SpawnTanksEvent(2); // might want to do different things between tank spawed and game started
        Invoke("BeginRound", 2);
    }

    private void BeginRound()
    {
        TankGameEvents.OnGameStartedEvent?.Invoke(); // start our game up
    }


    /// <summary>
    /// this is a custom update function, where I can tell it when/where to update
    /// </summary>
    /// <returns></returns>
    private IEnumerator GameLogic()
    {
        TankGameEvents.OnResetGameEvent?.Invoke(); // invoke our resetGameEvent
        TankGameEvents.OnPreGameEvent?.Invoke(); // call our pregame event
        TankGameEvents.SpawnTanksEvent(2); // might want to do different things between tank spawed and game started
        yield return new WaitForSeconds(preGameWaitTime);      
        TankGameEvents.OnGameStartedEvent?.Invoke(); // start our game up

        // do something else in too

        yield return null; // this tells our coroutine when the next "frame/update" should occur
    }
}
