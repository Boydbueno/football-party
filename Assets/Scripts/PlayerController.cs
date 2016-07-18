using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float Speed;
    public float DashStrength;
    public float RotationSpeed;

    private Rigidbody _rb;

    //dash variables
    public float DashCooldown;
    private bool _dashOnCooldown;
    private bool _isDashing;

	// Use this for initialization
	void Start()
	{
	    _rb = GetComponent<Rigidbody>();
	}

    void Update()
    {
        //should be checked in update.
        if (Input.GetButton("Dash"))
            _isDashing = true;
    }
 
	// Update is called once per frame
	void FixedUpdate()
	{
        MoveCheck();
        DashCheck();
	}

    private void MoveCheck()
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


    void Rotate(Vector3 targetRotation) 
    {
        Vector3 newRotation = Vector3.RotateTowards(_rb.transform.forward, targetRotation, RotationSpeed, 0.0f);
        _rb.transform.rotation = Quaternion.LookRotation(newRotation);
    }

    private void ResetDashCoolDown()
    {
        _dashOnCooldown = false;
    }
}
