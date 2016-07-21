using System;
using UnityEngine;
using System.Collections;
using UnityEditor;

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
        //int scoringTeamID = ball.LastTeamTouchedID;
        //Increases the Scores
            Score.UpdateScore(1- TeamID); 
        ball.Reset();
    }
}
