using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;

public enum playingState { InActive, Active, InBetween }

public class PlayerData
{
    public GameObject Player
    {
        get
        {
            return _player;
        }
        set
        {
            _player = value;
            PC = _player.GetComponent<PlayerController>();
            IsCreated = true;
        }
    }

    public PlayerController PC;

    private GameObject _player;

    public playingState State;
    public float InactivityTimer;
    public int PlayerID;
    public bool IsCreated;
}
public class TeamManager : MonoBehaviour
{
    public PlayerData[] Players;

    public Object PlayerPrefab;

    public int PlayerCount;
    public int TeamCount;

    public float MaxInactivitySeconds;

    private List<PlayerData> _activePlayers;
    private int[] _teamSizes;

    void Start()
    {
        Players = new PlayerData[PlayerCount];
        for(int i = 0; i < Players.Length; i++)
            Players[i] = new PlayerData {State = playingState.InActive, PlayerID = i + 1, IsCreated = false};
        
        _activePlayers = new List<PlayerData>();

        //init teamsizes list.
        _teamSizes = new int[TeamCount];
        for (int i = 0; i < TeamCount; i++)
            _teamSizes[i] = 0;
    }

    void Update() 
    {
        foreach (PlayerData data in Players)
            FiniteStateMachine(data);
    }

    //spawns teams.
    void SpawnTeams()
    {
        List<PlayerData> newActivePlayers = new List<PlayerData>();
        int team = 0;
        while (_activePlayers.Count > 0)
        {
            //pick a random player.
            int choice = Random.Range(0, _activePlayers.Count);
            PlayerData choiceData = _activePlayers[choice];

            //keep track in the lists.
            _activePlayers.Remove(choiceData);
            newActivePlayers.Add(choiceData);

            //spawn in team.
            SpawnPlayer(choiceData); //TODO make variable on team.

            //set team for next player.
            team++;
            if (team >= TeamCount)
                team = 0;
        }

        _activePlayers = newActivePlayers;
    }



    // Spawns a Player, with appropriate playerNumber
    void SpawnPlayer(PlayerData data)
    {
        Vector3 position = GeneratePosition(data);
        //create the player if it doesn't exist yet.
        if (!data.IsCreated)
        {
            GameObject player = Instantiate(PlayerPrefab, position, Quaternion.identity) as GameObject;
            PlayerController playerScript = player.GetComponent("PlayerController") as PlayerController;
            playerScript.PlayerNumber = data.PlayerID;

            data.Player = player;
        }
        //if it already exists, set it to active.
        else
            data.Player.SetActive(true);

        //add to smallest team.
        int smallestTeam = GetMinIndex(_teamSizes);
        data.PC.TeamID = _teamSizes[smallestTeam];
        _teamSizes[smallestTeam]++;

        _activePlayers.Add(data);
    }

    //TODO Make variable on team, teamsize, etc.
    private Vector3 GeneratePosition(PlayerData data)
    {
        return new Vector3(1, 0, 1);
    }

    //gets the index of the smallest value.
    private int GetMinIndex(IList<int> arr)
    {
        int i = 0;
        int val = int.MaxValue;
        for (int j = 0; j < arr.Count; j++)
        {
            if (arr[j] < val)
            {
                val = arr[j];
                i = j;
            }
        }

        return i;
    }

    //Handles spawning Finite State machine for all players
    void FiniteStateMachine(PlayerData data)
    {
        playingState playerState = data.State;
        float inactivityTimer = data.InactivityTimer;
        int playerNumber = data.PlayerID;

        //Handles InActive state => Started Pressed, goes to Active
        if (data.State == playingState.InActive)
        {
            bool playerStartPressed = Input.GetButton("Start" + playerNumber);
            if (playerStartPressed)
            {
                if(!data.IsCreated)
                    SpawnPlayer(data);
                data.State = playingState.Active;
            }
        }

        //Handles Active state => If x time inActivity go to InBetween
        if (playerState == playingState.Active)
        {
            float playerActivity = Input.GetAxis("Horizontal" + playerNumber);

            //reset if there's activity.
            if (playerActivity != 0)
                data.InactivityTimer = 0;
            //else keep counting
            else
                data.InactivityTimer += Time.deltaTime;

            //go to inbetween if we're past our limit.
            if (inactivityTimer >= MaxInactivitySeconds)
            {
                data.InactivityTimer = 0;
                data.State = playingState.InBetween;
            }
        }

        //Handles InBetween state => Cleans up unused player
        if (playerState == playingState.InBetween)
        {
            data.Player.SetActive(false);
            data.State = playingState.InActive;
            _activePlayers.Remove(data);
            int team = data.PC.TeamID;
            _teamSizes[team]--;
        }
    }
}
