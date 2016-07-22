using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public ScoreController score;
    public float destroyDelay;
    public float increment;

    public float BetweenModeTime = 2;

    /**Variables for shakes**/
    private GameObject cam;
    public bool screenShakeOn;
    private bool reShake;
    public float shakx;
    public float shaky;
    public float shakz;
    public bool shakeXOn;
    public bool shakeYOn;
    public bool shakeZOn;
    private float _n = 0;
    public float shakeXIntensifier = 1.0f;
    public float shakeYIntensifier = 1.0f;
    public float shakeZIntensifier = 1.0f;

    public GameObject Ball;
    public Object BombPrefab;

    private PlayerManager _playerManager;

    void Awake()
    {
        _playerManager = GetComponentInChildren<PlayerManager>();
        cam = GameObject.FindGameObjectWithTag("MainCamera");

        if (instance == null)
            instance = this;

        if (this != instance)
            Destroy(gameObject);
    }

    /// <summary>
    /// Moves the Camera Magickly, using parametrized Perline Noise Waves
    /// </summary>
    public void ScreenShake()
    {
        if(screenShakeOn)
        {
            if (reShake)
            {
                shakx = -shakx;
                shaky = -shaky;
                shakz = -shakz;
                reShake = false;
            }
            else
            {
                shakx = shakeXIntensifier * Mathf.PerlinNoise(Time.time * _n, 0.0f);
                shaky = shakeYIntensifier * Mathf.PerlinNoise(Time.time * _n + 1, 0.0f);
                shakz = shakeZIntensifier * Mathf.PerlinNoise(Time.time * _n + 2, 0.0f);
                reShake = true;
            }

            //Booleans shake to 0
            if (!shakeXOn)
            {
                shakx = 0;
            }
            if (!shakeYOn)
            {
                shaky = 0;
            }
            if (!shakeZOn)
            {
                shakz = 0;
            }

            Vector3 camPos = cam.transform.localEulerAngles;
            float camX = camPos.x;
            float camY = camPos.y;
            float camZ = camPos.z;
            cam.transform.localEulerAngles = new Vector3(
                camX + shakx,
                camY + shaky,
                camZ + shakz);
        }

    }

    void Update()
    {
        if (screenShakeOn)
        {
            _n += increment;
        }

        if (Input.GetKeyDown("space")) {
            StartBombMode();
        }
    }

    /// <summary>
    /// Given a GameObject, spawns Smoke on the players position.
    /// </summary>
    /// <param name="position">The position to spawn the smoke</param>
    public void Smoke(Vector3 position)
    {
        GameObject smokePrefab = (GameObject)Resources.Load("Smoke_PS");
        GameObject smoke = Instantiate(smokePrefab);
        smoke.transform.position = position;
        Destroy(smoke, destroyDelay);
    }

    /// <summary>
    /// Rumble the controller for a certain amount of time
    /// </summary>
    /// <param name="left">Rumble for left motor, float between 1 and 0</param>
    /// <param name="right">Rumble for right motor, float between 1 and 0</param>
    /// <param name="time">The time it should rumble</param>
    public void Rumble(PlayerIndex playerIndex, float left, float right, float time) 
    {
        GamePad.SetVibration(playerIndex, 0, 0);
        GamePad.SetVibration(playerIndex, left, right);
        StartCoroutine(StopRumbleAfterTime(playerIndex, time));
    }

    /// <summary>
    /// Start rumble. Don't forget to stop it.
    /// </summary>
    /// <param name="left">Rumble for left motor, float between 1 and 0</param>
    /// <param name="right">Rumble for right motor, float between 1 and 0</param>
    public void RumbleStart(PlayerIndex playerIndex, float left, float right) 
    {
        //GamePad.SetVibration(playerIndex, left, right);
    }

    /// <summary>
    /// Stop the rumble
    /// </summary>
    public void RumbleStop(PlayerIndex playerIndex) 
    {
        GamePad.SetVibration(playerIndex, 0, 0);
    }

    /// <summary>
    /// IEnumerator to start as coroutine to stop the rumble after a certain amount of time.
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator StopRumbleAfterTime(PlayerIndex playerIndex, float time) 
    {
        yield return new WaitForSeconds(time);
        RumbleStop(playerIndex);
    }

    /// <summary>
    /// Given a GameObject, spawns Explosion on the gameObjects position.
    /// </summary>
    /// <param name="position">The position to spawn the smoke</param>
    public void Explode(Vector3 position)
    {
        GameObject explosionPrefab = (GameObject)Resources.Load("Explosion_PS");
        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = position;
        Destroy(explosion, 1.0f);
    }

    public void StartBombMode() {
        // Do the smoke effect
        Smoke(Ball.transform.position);

        // We deactivate the ball
        Ball.SetActive(false);

        // We instantiate the bomb
        GameObject bomb = (GameObject)Instantiate(BombPrefab, Ball.transform.position, Quaternion.identity);

        // We give the bomb an detonation time
        bomb.GetComponent<BombController>().SetDetonationTime();
    }

    public void GoToNormalMode()
    {
        Invoke("StartNormalMode", BetweenModeTime);
    }

    private void StartNormalMode()
    {
        SpawnBall();
        UpdateScoreAfterBomb();
        _playerManager.ShuffleTeams();
    }

    //updates the score
    private void UpdateScoreAfterBomb()
    {
        int blueDead = 0, redDead = 0;
        List<PlayerManager.PlayerData> deadPlayers = _playerManager.PlayersData.FindAll(p => p.Player.GetComponent<PlayerController>().IsDead);
        foreach (PlayerManager.PlayerData p in deadPlayers)
        {
            if (p.TeamID == 1)
                blueDead++;
            else
                redDead++;
        }
        score.UpdateScore(1, blueDead);
        score.UpdateScore(2, redDead);
    }

    private void SpawnBall()
    {
        Ball.SetActive(true);

        BallController ballControl = Ball.GetComponent<BallController>();
        ballControl.Respawn();
    }

}
