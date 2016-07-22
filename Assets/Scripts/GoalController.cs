using System;
using UnityEngine;
using System.Collections;

public class GoalController : MonoBehaviour
{
    public int TeamID;
    public ScoreController Score;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(string.Format("Entered Goal: {0}", other.gameObject.tag));
        if (other.gameObject != null && other.gameObject.tag == "Ball" )
        {
            BallController ball = other.GetComponent<BallController>();
            GoalScored(ball);
        }
    }

    private void GoalScored(BallController ball)
    {
        Debug.Log(TeamID);
        //Increases the Scores
        Score.UpdateScore(1- TeamID);
        //reset the ball.
        ball.Respawn();
    }
}
