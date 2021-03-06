﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasTime : MonoBehaviour {


    public int minSwitchTime;
    public int maxSwitchTime;
    public Text timeText;
    private float second;
    public float secondDuration;

    public float shakeDuration;
    public float switchTime;
    public int modeSwithTime;
    public bool toShake;

    AudioController _ac;
    // Use this for initialization
    void Start()
    {
        modeSwithTime = Random.Range(minSwitchTime, maxSwitchTime);
        _ac = GetComponent<AudioController>();
    }

    void Shake()
    {
        if (toShake)
        {
            switchTime += Time.deltaTime;
            GameManager.instance.ScreenShake();
            _ac.Play("Ding");
            if (switchTime > shakeDuration)
            {
               
                switchTime = 0;
                modeSwithTime += Random.Range(minSwitchTime, maxSwitchTime);

                //var value = Random.value;
                //if(value < 0.5)
                //{
                    GameManager.instance.StartBombMode();
                //}
                //else
                //{
                //    PlayerManager.instance.ShuffleTeams();
                //}
                toShake = false;
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        second += Time.deltaTime;
        if (second >= secondDuration)
        {
            //After second has passed update timeText by 1 second
            string[] curTime = timeText.text.Split(':');
            string min = curTime[0];
            string sec = curTime[1];
            int newTime = int.Parse(sec) + 1;
            string NewTime = newTime.ToString();
            int minutes = int.Parse(min);

            //Handles minutes
            if (newTime >= 59)
            {
                string extra = "";
                if (minutes < 10)
                {
                    extra = "0";
                }
                minutes += 1;
                min = extra + minutes.ToString();
                newTime = 0;
            }

            //Handles 0's for digits below 10.
            if (newTime < 10)
            {
                NewTime = '0' + newTime.ToString();
            }

            timeText.text = min + ':' + NewTime;

            //Reset second, for next second
            second = 0;

            //Switches Modes
            int time = newTime + minutes * 60;
            if (time >= modeSwithTime)
            {
                toShake = true;

            }
        }

        Shake();
    }
}
