using System;
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class PlayerController : MonoBehaviour
{
    public float Speed;
    public float RotationSpeed;

    public float DashStrength;
    public float DashToBallForce; // Doesn't work yet //TODO
    public float DashCooldown;
    public string playerNumber; 

    public Animator Animator;

    private Rigidbody _rb;

    //dash variables
    private bool _dashOnCooldown;
    private bool _isDashing;

    private float _moveHor, _moveVert;

	// Use this for initialization
	void Start()
	{
	    _rb = GetComponent<Rigidbody>();
	}

    void Update()
    {
        //get input.
        _isDashing = Input.GetButtonDown("Dash" + playerNumber) && !_dashOnCooldown;
        _moveHor = Input.GetAxis("Horizontal" + playerNumber);
        _moveVert = Input.GetAxis("Vertical" + playerNumber);
    }

    #region FixedUpdate
    void FixedUpdate()
	{
        ApplyMovement();
        DashCheck();
	}

    private void ApplyMovement()
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

    private void DashCheck()
    {
        if (_isDashing)
        {
            //Dash
            _rb.AddForce(_rb.velocity*DashStrength);

            //start dash cooldown
            _dashOnCooldown = true;
            Invoke("ResetDashCoolDown", DashCooldown);
        }
    }

    void Rotate(Vector3 targetRotation) 
    {
        Vector3 newRotation = Vector3.RotateTowards(_rb.transform.forward, targetRotation, RotationSpeed, 0.0f);
        _rb.transform.rotation = Quaternion.LookRotation(newRotation);
    }
    
    //called after a the cooldown expires.
    private void ResetDashCoolDown()
    {
        _dashOnCooldown = false;
    }
    #endregion

    void OnTriggerEnter(Collider other)
    {
        //apply extra force when dashing.
        if (_isDashing && other.gameObject.tag == "Ball")
        {
            Vector3 direction = other.transform.position - transform.position;
            other.GetComponent<Rigidbody>().AddForce(direction * DashToBallForce);
            Console.WriteLine("Should add extra force to ball");
        }
    }
}
