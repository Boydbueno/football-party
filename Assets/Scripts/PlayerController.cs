using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float Speed;
    public float DashStrength;

    private Rigidbody _rb;
    private bool _isDashing;

	// Use this for initialization
	void Start ()
	{
	    _rb = GetComponent<Rigidbody>();
	}

    void FixedUpdate ()
	{
        MoveCheck();
        DashCheck();
	}

    void MoveCheck()
    {
        //get movement input.
        float hor = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(hor, 0.0f, vert);

        _rb.AddForce(movement * Speed);
    }

    void DashCheck()
    {
        if (_isDashing)
        {
            //Dash
            _rb.AddForce(_rb.velocity * DashStrength);
        }
    }
}
