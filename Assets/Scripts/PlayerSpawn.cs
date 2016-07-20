using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
public class PlayerSpawn : MonoBehaviour
{

    public enum playingState { InActive, Active, InBetween }

    public playingState p1_State = playingState.InActive;
    public playingState p2_State = playingState.InActive;
    public playingState p3_State = playingState.InActive;
    public playingState p4_State = playingState.InActive;
    public playingState p5_State = playingState.InActive;
    public playingState p6_State = playingState.InActive;
    public playingState p7_State = playingState.InActive;
    public playingState p8_State = playingState.InActive;

    public float p1_inactivityTimer;
    public float p2_inactivityTimer;
    public float p3_inactivityTimer;
    public float p4_inactivityTimer;
    public float p5_inactivityTimer;
    public float p6_inactivityTimer;
    public float p7_inactivityTimer;
    public float p8_inactivityTimer;

    public Object playerObject;
    public int playerAmount;
    public float maxInactivitytime;

    List<GameObject> spawnedPlayers = new List<GameObject>();
    // Use this for initialization


    // Spawns a Player, with appropriate playerNumber and adds it to the spawnedPlayers list
    void SpawnPlayer(int playerNumber)
    {

        bool objectExists = false;
        for (int i = 0; i < spawnedPlayers.Count; i++)
        {
            PlayerController PLAYERscript = spawnedPlayers[i].GetComponent("PlayerController") as PlayerController;
            if (int.Parse(PLAYERscript.PlayerNumber) == playerNumber)
            {
                objectExists = true;
                break;
            }
        }

        if (!objectExists)
        {
            GameObject player = Instantiate(playerObject, new Vector3(1, 0, 1), Quaternion.identity) as GameObject;
            PlayerController playerScript = player.GetComponent("PlayerController") as PlayerController;
            playerScript.PlayerNumber = playerNumber.ToString();
            spawnedPlayers.Add(player);
        }
        else
        {
            foreach (GameObject player in spawnedPlayers)
            {
                PlayerController playerScript = player.GetComponent("PlayerController") as PlayerController;
                if (playerScript.PlayerNumber == playerNumber.ToString())
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
                if (int.Parse(playerScript.PlayerNumber) == playerNumber)
                {
                    player.SetActive(false);
                    player_State = playingState.InActive;
                    return;
                }
            }
        }


    }

    void Update()
    {
        FiniteStateMachine(ref p1_State, ref p1_inactivityTimer, 1);
        FiniteStateMachine(ref p2_State, ref p2_inactivityTimer, 2);
        FiniteStateMachine(ref p3_State, ref p3_inactivityTimer, 3);
        FiniteStateMachine(ref p4_State, ref p4_inactivityTimer, 4);
        FiniteStateMachine(ref p5_State, ref p5_inactivityTimer, 5);
        FiniteStateMachine(ref p6_State, ref p6_inactivityTimer, 6);
        FiniteStateMachine(ref p7_State, ref p7_inactivityTimer, 7);
        FiniteStateMachine(ref p8_State, ref p8_inactivityTimer, 8);
    }
}
