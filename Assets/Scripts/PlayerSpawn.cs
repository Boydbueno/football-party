using UnityEngine;
using System.Collections;

public class PlayerSpawn : MonoBehaviour {

    public Object playerObject;
    // Use this for initialization
    void Start()
    {
        int activeControllers = GetActiveControllers();
        for (int i = 0; i < activeControllers; i++)
        {
            SpawnPlayer(i);
        }
    }

    // Returns amount of active controllers
    int GetActiveControllers()
    {
        string[] activeControllers = Input.GetJoystickNames();
        return activeControllers.Length;
    }

    // Spawns a Player, with appropriate parameters
    void SpawnPlayer(int number)
    {
        GameObject player = Instantiate(playerObject, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        PlayerController playerScript = player.GetComponent("PlayerController") as PlayerController;
        playerScript.Speed = 80;
        playerScript.RotationSpeed = 0.4f;
        playerScript.DashStrength = 100;
        playerScript.DashCooldown = 0.5f;
    }
	
	// Update is called once per frame
	void Update () {
        
    }
}
