using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float Speed;
    public float DashStrength;

    private Rigidbody _rb;

    //dash variables
    public float DashCooldown;
    private bool _dashOnCooldown;
    private bool _isDashing;

	// Use this for initialization
	void Start ()
	{
	    _rb = GetComponent<Rigidbody>();
	}


    void Update()
    {
        //should be checked in update.
        if (Input.GetKeyDown("space"))
            _isDashing = true;

    }
	void FixedUpdate ()
	{
        MoveCheck();
        DashCheck();
	}

    private void MoveCheck()
    {
        //get movement input.
        float hor = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        //apply the movement.
        Vector3 movement = new Vector3(hor, 0.0f, vert);
        _rb.AddForce(movement * Speed);
    }

    private void DashCheck()
    {
        if (!_dashOnCooldown && _isDashing)
        {
            //Dash, and start the cooldown.
            _rb.AddForce(_rb.velocity * DashStrength);
            _dashOnCooldown = true;
            Invoke("ResetDashCoolDown", DashCooldown);
        }
    }

    private void ResetDashCoolDown()
    {
        _dashOnCooldown = false;
    }
}
