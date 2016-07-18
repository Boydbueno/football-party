using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float Speed;
    public float DashStrength;
    public float RotationSpeed;

    private Rigidbody _rb;
    private bool _isDashing;

	// Use this for initialization
	void Start()
	{
	    _rb = GetComponent<Rigidbody>();
	}
    
	// Update is called once per frame
	void FixedUpdate()
	{
        MoveCheck();
        DashCheck();
	}

    void MoveCheck()
    {
        //get movement input.
        float hor = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        Vector3 targetRotation = new Vector3(hor, 0.0f, vert);

        if (hor != 0 || vert != 0) 
        {
            _rb.AddForce(_rb.transform.forward * Speed);
        }

        Rotate(targetRotation);
    }

    void DashCheck()
    {
        if (_isDashing)
        {
            //Dash
            _rb.AddForce(_rb.velocity * DashStrength);
        }
    }

    void Rotate(Vector3 targetRotation) 
    {
        Vector3 newRotation = Vector3.RotateTowards(_rb.transform.forward, targetRotation, RotationSpeed, 0.0f);
        _rb.transform.rotation = Quaternion.LookRotation(newRotation);
    }
}
