using System;
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class PlayerController : MonoBehaviour
{
    public float Speed;
    public float RotationSpeed;
    public string playerNumber; 
    public Animator Animator;

    private Rigidbody _rb;
    private float _moveHor, _moveVert;

	// Use this for initialization
	void Start()
	{
	    _rb = GetComponent<Rigidbody>();
	}

    void Update()
    {
        //get input.
        _moveHor = Input.GetAxis("Horizontal"+ playerNumber);
        _moveVert = Input.GetAxis("Vertical"+ playerNumber);
    }

    #region FixedUpdate
    void FixedUpdate()
	{
        //add forward movement only when input is given. 
        if (_moveHor != 0 || _moveVert != 0)
            _rb.AddForce(_rb.transform.forward * Speed);

        //Modify animation speed
        Animator.SetFloat("Speed", (Math.Abs(_moveHor) + Math.Abs(_moveVert)) / 2);

        //apply rotation
        Vector3 targetRotation = new Vector3(_moveHor, 0.0f, _moveVert);
        Rotate(targetRotation);
    }

    //Rotate the player to the target.
    void Rotate(Vector3 targetRotation) 
    {
        Vector3 newRotation = Vector3.RotateTowards(_rb.transform.forward, targetRotation, RotationSpeed, 0.0f);
        _rb.transform.rotation = Quaternion.LookRotation(newRotation);
    }
    #endregion
}
