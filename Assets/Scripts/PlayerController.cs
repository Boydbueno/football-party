using System;
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEditor;

public class PlayerController : MonoBehaviour
{
    public float Speed;
    public float RotationSpeed;
    public string playerNumber;
    public bool DashChargeStopsMovement;
    public Animator Animator;

    private Rigidbody _rb;
    private float _moveHor, _moveVert;
    private DashController _dash;

    void OnCollisionEnter(Collision collision) 
    {
        if (collision.gameObject.tag == "Ball") 
        {
            // If we ever want to add a force to the ball on collision, we can do that here.
        }
    }
    
	void Start()
	{
	    _rb = GetComponent<Rigidbody>();
	    _dash = GetComponent<DashController>();
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
        float movementSpeed = Math.Abs(_moveHor) > Math.Abs(_moveVert) ? Math.Abs(_moveHor) : Math.Abs(_moveVert);

        ApplyMovement(movementSpeed);

        //Modify animation speed
        Animator.SetFloat("Speed", movementSpeed);

        //apply rotation
        Vector3 targetRotation = new Vector3(_moveHor, 0.0f, _moveVert);
        Rotate(targetRotation);
    }

    private void ApplyMovement(float movementSpeed)
    {
        //if charging our dash stops our movement, and dash is charging, we don't move. :O :O
        if (DashChargeStopsMovement && _dash.IsCharging()) return;

        //add forward movement only when input is given.
        if (_moveHor != 0 || _moveVert != 0)
            _rb.AddForce(_rb.transform.forward * movementSpeed * Speed);
    }

    //Rotate the player to the target.
    void Rotate(Vector3 targetRotation) 
    {
        Vector3 newRotation = Vector3.RotateTowards(_rb.transform.forward, targetRotation, RotationSpeed, 0.0f);
        _rb.transform.rotation = Quaternion.LookRotation(newRotation);
    }
    #endregion
}
