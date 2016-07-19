﻿using System;
using UnityEngine;
using System.Collections;

public class GoalController : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(string.Format("Entered Goal: {0}", other.gameObject.tag));
        if(other.gameObject.tag == "Ball")
            GoalScored();
    }

    private void GoalScored()
    {
        Debug.Log("Goal Scored");
    }
}
