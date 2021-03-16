using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankUIManager : MonoBehaviour
{
    public GameObject startScreen;
    public GameObject inGameUI;
    public GameObject roundOverText;

    public List<InGamePlayerUI> allPlayerUI = new List<InGamePlayerUI>(); // a list of all the player ui

    private void OnEnable()
    {
        TankGameEvents.OnPreGameEvent += PreGame;
        TankGameEvents.OnGameStartedEvent += InGame;
        TankGameEvents.OnRoundEnededEvent += PostRound;
        TankGameEvents.OnScoreUpdatedEvent += UpdateScoreText;
        TankGameEvents.OnTanksSpawnedEvent += DisplayScore;
    }

    private void OnDisable()
    {
        TankGameEvents.OnPreGameEvent -= PreGame;
        TankGameEvents.OnGameStartedEvent -= InGame;
        TankGameEvents.OnRoundEnededEvent -= PostRound;
        TankGameEvents.OnScoreUpdatedEvent -= UpdateScoreText;
        TankGameEvents.OnTanksSpawnedEvent -= DisplayScore;
    }

    /// <summary>
    /// Called to display the pregame ui
    /// </summary>
    private void PreGame()
    {
        startScreen.SetActive(true);
        inGameUI.SetActive(false);
        roundOverText.SetActive(false);
    }

    /// <summary>
    /// Called to display the ingame ui
    /// </summary>
    private void InGame()
    {
        startScreen.SetActive(false);
        inGameUI.SetActive(true);
        roundOverText.SetActive(false);
    }

    /// <summary>
    /// Display the score text if the tank is spawned in
    /// </summary>
    /// <param name="allTanks"></param>
    private void DisplayScore(List<GameObject> allTanks)
    {
        for (int i = 0; i < allPlayerUI.Count; i++)
        {
            allPlayerUI[i].DisableText(); // disables all the text
        }

        for (int i = 0; i < allTanks.Count; i++)
        {

            if(!allTanks[i].GetComponent<Tank>())
            {
                continue;
            }

            Tank tank = allTanks[i].GetComponent<Tank>(); // grab a reference to the tank script

            for(int j=0; j<allPlayerUI.Count; j++)
            {
                allPlayerUI[j].EnableText(tank.playerNumber);
            }
        }
    }

    private void UpdateScoreText(PlayerNumber playerNumber, int Amount)
    {
        for(int i=0; i<allPlayerUI.Count;i++)
        {
            allPlayerUI[i].SetPlayerText(playerNumber, Amount);
        }
     
    }

    /// <summary>
    /// Called when the round is over
    /// </summary>
    private void PostRound(PlayerNumber playerNumber)
    {
        startScreen.SetActive(false);
        inGameUI.SetActive(false);
        roundOverText.SetActive(true);
        roundOverText.GetComponentInChildren<Text>().text = "Player " + playerNumber.ToString() + " Wins!";
    }
}

[System.Serializable]
public class InGamePlayerUI
{
    public PlayerNumber playerReferenceNumber;
    public Text playerText;

    /// <summary>
    /// Disables the player text
    /// </summary>
    public void DisableText()
    {
        playerText.gameObject.SetActive(false);
    }

    /// <summary>
    /// Enables the player Text
    /// </summary>
    /// <param name="PlayerNumberToCheck"></param>
    public void EnableText(PlayerNumber PlayerNumberToCheck)
    {
        if(playerReferenceNumber == PlayerNumberToCheck)
        {
            playerText.gameObject.SetActive(true); // turn the text on
        }
    }

    public void SetPlayerText(PlayerNumber playerNumberCheck, int Amount)
    {
        if(playerNumberCheck == playerReferenceNumber)
        {
            playerText.text = "Player " + playerReferenceNumber.ToString()+": " +Amount;
        }
    }
}
