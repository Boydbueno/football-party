using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    void Awake() 
    {
        if (instance == null) 
            instance = this;

        if (this != instance) 
            Destroy(gameObject);
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
}
