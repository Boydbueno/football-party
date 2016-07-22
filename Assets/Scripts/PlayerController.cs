using System;
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class PlayerController : MonoBehaviour
{
    public float Speed;
    public float RotationSpeed;
    public float RespawnTime;
    public int PlayerNumber;
    public int TeamID;
    public bool DashChargeStopsMovement;
    public bool IsDead;
    public Animator Animator;

    private Rigidbody _rb;
    private DashController _dash;
    private Vector3 _startPosition;
    private float _moveHor, _moveVert;

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
	    _startPosition = transform.position;
	}

    void Update()
    {
        //get input.
        _moveHor = Input.GetAxis("Horizontal"+ PlayerNumber);
        _moveVert = Input.GetAxis("Vertical"+ PlayerNumber);
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

    public void Die()
    {
        IsDead = true;
        //spawn smoke
        GameManager.instance.Smoke(transform.position);
        //"remove" player.
        transform.position = new Vector3(1000,-1000,1000);
    }

    public void Resurrect()
    {
        if (IsDead)
            transform.position = _startPosition;
    }
}
