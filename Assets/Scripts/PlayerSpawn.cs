using UnityEngine;
using System.Collections;

public class PlayerSpawn : MonoBehaviour {

    public Object playerObject;

    public int playerAmount;
    // Use this for initialization
    void Awake()
    {
        for (int i = 0; i < playerAmount; i++)
        {
            SpawnPlayer(i+1);
        }
    }

    // Returns amount of active controllers
    int GetActiveControllers()
    {
        string[] activeControllers = Input.GetJoystickNames();
        return activeControllers.Length;
    }

    // Spawns a Player, with appropriate playerNumber
    void SpawnPlayer(int playerNumber)
    {
        GameObject player = Instantiate(playerObject, new Vector3(1, 0, 1), Quaternion.identity) as GameObject;
        PlayerController playerScript = player.GetComponent("PlayerController") as PlayerController;
        playerScript.playerNumber = playerNumber.ToString();
    }
}
