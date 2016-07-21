using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallController : MonoBehaviour
{
    public int LastTeamTouchedID { get; private set; }

    private Rigidbody _rb;
    private Vector3 _startPos;
    private AudioController _ac;

	void Start()
	{
	    _startPos = transform.position;
        _rb = GetComponent<Rigidbody>();
        _ac = GetComponent<AudioController>();
        ResetLTT();
	}

    public void Reset()
    {
        transform.position = _startPos;
        _rb.velocity = Vector3.zero;

        ResetLTT();
    }

    //resets the LastTeamTouchedID.
    private void ResetLTT()
    { 
        LastTeamTouchedID = -1;
    }

    void OnCollisionEnter(Collision other)
    {
        GameObject obj = other.gameObject;
        if (obj.tag == "Player")
        {
            _ac.Play("BallKick");
            PlayerController pc = obj.GetComponent<PlayerController>();
            LastTeamTouchedID = pc.TeamID;
            Debug.Log("LastTouched: " + LastTeamTouchedID);
        }

    }

}
