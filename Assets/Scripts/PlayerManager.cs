using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using XInputDotNetPure;

public class PlayerManager : MonoBehaviour {

    public List<PlayerData> PlayersData = new List<PlayerData>();

    public UnityEngine.Object PlayerPrefab;
    public float MaxInactivityTime;

    public int TeamsCount;

    public Vector3 BlueTeamSpawn, RedTeamSpawn;

    private Texture2D[] _textures;

    [System.Serializable]
    public class PlayerData {
        public GameObject Player;
        public int PlayerID;
        public float InactivityTimer;
        public int TeamID;
    }

    public class PlayerCreationData
    {
        public int playerID;
        public Vector3 Position = Vector3.zero;
    }

    public int maxPlayerCount;

    void Start()
    {
        _textures = Resources.LoadAll<Texture2D>("Textures");
    }

    void Update() {

        if(Input.GetKeyDown("backspace")) {
            ShuffleTeams();
        }

        // We constantly listen for start inputs
        for (int i = 1; i <= maxPlayerCount; i++) {
            if (Input.GetButtonDown("Start" + i)) {
                // we attempt to find the player
                PlayerData playerData = PlayersData.Find(item => item.PlayerID == i);

                if (playerData == null) {
                    {
                        PlayerCreationData data = new PlayerCreationData {playerID = i};
                        createPlayer(data);
                    }
                } else if (!playerData.Player.activeSelf) {
                    activatePlayer(playerData);
                }                
            }

            // Check horizontal input to reset 
            if (Input.GetAxis("Horizontal" + i) != 0) {
                PlayerData playerData = PlayersData.Find(item => item.PlayerID == i);
                if (playerData != null) playerData.InactivityTimer = 0;
            }
        }

        // We will update the inactivity timers
        foreach (PlayerData playerData in PlayersData) {
            playerData.InactivityTimer += Time.deltaTime;
            // If an inactivity timer hits the max inactivity deactive the player
            if (playerData.InactivityTimer >= MaxInactivityTime) {
                deactivatePlayer(playerData);
            }
        }
    }

    public void ShuffleTeams() {

        //shuffle the list to a random order.
        List<PlayerData> ActivePlayerList = PlayersData.FindAll(item => item.Player.activeSelf);
        PlayersData.Clear();    
        for (int i = 0; i < ActivePlayerList.Count; i++) {
            PlayerData temp = ActivePlayerList[i];
            int RandomIndex = Random.Range(0, ActivePlayerList.Count);
            ActivePlayerList[i] = ActivePlayerList[RandomIndex];
            ActivePlayerList[RandomIndex] = temp;
        }

        //loop through the list, destroying the players and creating the anew.
        foreach(PlayerData data in ActivePlayerList) { 
            //save reusable data.
            int playerNumber = data.PlayerID;
            Vector3 position = data.Player.transform.position;
            //KILL. DIE.
            Destroy(data.Player);
            PlayersData.Remove(data);
            //create a new player.
            PlayerCreationData createData = new PlayerCreationData {playerID = playerNumber, Position = position};
            createPlayer(createData);
        }
    }

    private void createPlayer(PlayerCreationData data)
    {
        int playerID = data.playerID;
        int teamID = GetSmallestTeamId();

        //get position
        Vector3 position;
        if (data.Position != Vector3.zero) //default value, if we haven't assigned a custom value.
            position = data.Position;
        else
            position = GetTeamPosition(teamID);
        

        // Create a new player with this id and give it an active state
        GameObject player = (GameObject)Instantiate(PlayerPrefab, position, Quaternion.identity);
        player.GetComponent<PlayerController>().PlayerNumber = playerID;
        
        //set the data.
        PlayerData playerData = new PlayerData {
            Player = player,
            PlayerID = playerID,
            TeamID = teamID
        };

        player.GetComponent<PlayerController>().TeamID = teamID;

        //set the right texture.
        Texture2D texture = GetTexture(playerID, teamID);
        SkinnedMeshRenderer rendererInChildren = player.GetComponentInChildren<SkinnedMeshRenderer>();
        rendererInChildren.material.mainTexture = texture;

        // And add it to the list
        PlayersData.Add(playerData);
    }

    //returns the spawning position for the team.
    private Vector3 GetTeamPosition(int teamID)
    {
        //crappy version, todo make better later
        return teamID == 1 ? BlueTeamSpawn : RedTeamSpawn;
    }

    private void deactivatePlayer(PlayerData playerData) {
        playerData.InactivityTimer = 0;
        GameManager.instance.RumbleStop((PlayerIndex)playerData.PlayerID - 1);
        playerData.Player.SetActive(false);
    }

    private void activatePlayer(PlayerData playerData) {
        playerData.Player.SetActive(true);
    }

    private int GetSmallestTeamId() {
        int smallestTeamSize = 100;
        int smallestTeamId = 0;

        for (int i = 1; i <= TeamsCount; i++) {
            List<PlayerData> playersData = PlayersData.FindAll(item => item.TeamID == i && item.Player.activeSelf);
            if (playersData.Count <= smallestTeamSize) {
                smallestTeamId = i;
                smallestTeamSize = playersData.Count;
            }
        }

        return smallestTeamId;
    }

    private Texture2D GetTexture(int playerID, int teamID)
    {
        string teamText = teamID == 1 ? "TeamBlue" : "TeamRed";
        return _textures.FirstOrDefault(t => t.name.Contains(teamText) && t.name.Contains("Color_" + playerID));
    }
}
