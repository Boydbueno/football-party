using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class PlayerController : MonoBehaviour
{
    public float Speed;
    public float DashStrength;
    public float DashToBallForce; // Doesn't work yet //TODO
    public float DashCooldown;

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
        _isDashing = Input.GetKeyDown("space") && !_dashOnCooldown;
        _moveHor = Input.GetAxis("Horizontal");
        _moveVert = Input.GetAxis("Vertical");
    }

    #region FixedUpdate
    // Update is called once per frame
    void FixedUpdate()
	{
        ApplyMovement();
        DashCheck();
	}

    private void ApplyMovement()
    {
        //apply the movement.
        Vector3 movement = new Vector3(_moveHor, 0.0f, _moveVert);
        _rb.AddForce(movement * Speed);
    }

    private void DashCheck()
    {
        if (_isDashing)
        {
            //Dash
            _rb.AddForce(_rb.velocity * DashStrength);

            //start dash cooldown
            _dashOnCooldown = true;
            Invoke("ResetDashCoolDown", DashCooldown);
        }
    }

    //called after a the cooldown expires.
    private void ResetDashCoolDown()
    {
        _dashOnCooldown = false;
    }
    #endregion

    void OncolOnCollisionEnter(Collision other)
    {
        //apply extra force when dashing.
        if (_isDashing && other.gameObject.tag == "Ball")
        {
            Vector3 direction = other.transform.position - transform.position;
            other.rigidbody.AddForce(direction * DashToBallForce);
        }
    }
}
