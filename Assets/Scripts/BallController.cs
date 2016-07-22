using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallController : MonoBehaviour
{
    public int LastTeamTouchedID { get; private set; }
    public float RespawnTime = 0.5f;

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

    public void Respawn()
    {
        GameManager.instance.Smoke(transform.position);
        
        transform.position = new Vector3(1000,1000,1000); //best way ever to remove the ball!
        Invoke("Reset", RespawnTime);
    }

    private void Reset()
    {
        //reset the position and movement.
        transform.position = _startPos;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        ResetLTT();
        GameManager.instance.Smoke(_startPos);
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
