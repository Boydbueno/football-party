using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class GameManager : MonoBehaviour 
{

    public static GameManager instance;
    public float destroyDelay;

    public GameObject Ball;
    public Object BombPrefab;

    void Awake() 
    {
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
        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
        cam.transform.localPosition += new Vector3(1, 1, 1);

    }

    public void Update() 
    {
        if (Input.GetKeyDown("space")) 
        {
            StartBombMode();
        }
    }

    /// <summary>
    /// Given a GameObject, spawns Smoke on the players position.
    /// </summary>
    /// <param name="position">Position to spawn the smoke at</param>
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
        GamePad.SetVibration(playerIndex, left, right);
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

    public void StartBombMode() 
    {
        // Do the smoke effect
        Smoke(Ball.transform.position);

        // We deactivate the ball
        Ball.SetActive(false);

        // We instantiate the bomb
        GameObject bomb = (GameObject)Instantiate(BombPrefab, Ball.transform.position, Quaternion.identity);

        // We give the bomb an detonation time
        bomb.GetComponent<BombController>().SetDetonationTime();
    }
}
