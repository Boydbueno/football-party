using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public enum playingState { InActive, Active, InBetween }

public struct PlayerData
{
    public PlayerController pc;
    public playingState State;
    public float InactivityTimer;
}
public class PlayerSpawn2 : MonoBehaviour
{
    public PlayerData[] players;

    public Object playerObject;
    public int playerAmount;
    public float maxInactivitytime;

    List<GameObject> spawnedPlayers = new List<GameObject>();

    void Start()
    {
        players = new PlayerData[playerAmount];
    }

    void Update() 
    {
        //FSM for every player.
        for(int i = 0; i < players.Length; i++)
        {
            PlayerData p = players[i];
            FiniteStateMachine(ref p.State, ref p.InactivityTimer, i);
        }
    }

    // Spawns a Player, with appropriate playerNumber and adds it to the spawnedPlayers list
    void SpawnPlayer(int playerNumber)
    {

        //check if a player already exists.
        bool objectExists = spawnedPlayers.Select(t => t.GetComponent("PlayerController") as PlayerController).Any(playerScript => playerScript.PlayerNumber == playerNumber);

        //create the player if it doesn't exist yet.
        if (!objectExists)
        {
            GameObject player = Instantiate(playerObject, new Vector3(1, 0, 1), Quaternion.identity) as GameObject;
            PlayerController playerScript = player.GetComponent("PlayerController") as PlayerController;
            playerScript.PlayerNumber = playerNumber;
            spawnedPlayers.Add(player);
        }
        //if it already exists, set it to active.
        else
        {
            foreach (GameObject player in spawnedPlayers)
            {
                PlayerController playerScript = player.GetComponent("PlayerController") as PlayerController;
                if (playerScript.PlayerNumber == playerNumber)
                {
                    player.SetActive(true);
                    break;
                }
            }

        }
    }
   
    //Handles spawning Finite State machine for all players
    void FiniteStateMachine(ref playingState player_State, ref float inactivity_timer, int playerNumber)
    {
        //Handles InActive state => Started Pressed, goes to Active
        if (player_State == playingState.InActive)
        {
            bool player_StartPressed = Input.GetButton("Start" + playerNumber);
            if (player_StartPressed)
            {
                SpawnPlayer(playerNumber);
                player_State = playingState.Active;
            }
        }

        //Handles Active state => If x time inActivity go to InBetween
        if (player_State == playingState.Active)
        {
            float player_activity = Input.GetAxis("Horizontal" + playerNumber);
            if (player_activity != 0)
            {
                inactivity_timer = 0;
            }
            else
            {
                inactivity_timer += Time.deltaTime;
            }

            if (inactivity_timer >= maxInactivitytime)
            {
                inactivity_timer = 0;
                player_State = playingState.InBetween;
            }
        }

        //Handles InBetween state => Cleans up unused player
        if (player_State == playingState.InBetween)
        {
            foreach (GameObject player in spawnedPlayers)
            {
                PlayerController playerScript = player.GetComponent("PlayerController") as PlayerController;
                if (playerScript.PlayerNumber == playerNumber)
                {
                    player.SetActive(false);
                    player_State = playingState.InActive;
                    return;
                }
            }
        }
    }
}
