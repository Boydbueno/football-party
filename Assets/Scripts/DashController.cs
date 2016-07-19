using UnityEngine;
using System.Collections;
using System.Xml.Schema;

public class DashController : MonoBehaviour {

    //publics
    public float ForceMin, ForceMax;
    public float DashChargeMaxTime; //After how many frames since we started charging the dash, do we achieve max dashing charge.
    public float Cooldown;
    
    //privates
    private bool _onCooldown;
    private bool _dashButtonDown;
    private float _chargeTime;

    //variables we get from other components.
    private string _playerNumber;
    private Rigidbody _rb;

    void Start()
    {
        _playerNumber = GetComponent<PlayerController>().playerNumber;
        _rb = GetComponent<Rigidbody>();
        ResetCharge();
    }
	
	void Update()
	{
	    _dashButtonDown = Input.GetButton("Dash" + _playerNumber);
	}

    void FixedUpdate()
    {
        //on cooldown, we do nothing.
        if (_onCooldown) return;

        if (_dashButtonDown)
            UpdateChargeTime();
        else //we pressed last check, and have now released.
            Dash();
    }

    //returns true if the player is charging his dash.
    public bool IsCharging()
    {
        return _chargeTime > ForceMin;
    }

    //updates charge timer
    private void UpdateChargeTime()
    {
        _chargeTime += Time.deltaTime / DashChargeMaxTime;
        //clamp
        if (_chargeTime > 1)
            _chargeTime = 1;
    }

    //the actual dashing. 
    private void Dash()
    {
        float charge = Mathf.Lerp(ForceMin, ForceMax, _chargeTime);
        Vector3 chargeForce = _rb.transform.forward * charge;
        _rb.AddForce(_rb.transform.forward * charge);
        ResetCharge();
        StartCooldown();
    }

    //resets the charge.
    private void ResetCharge()
    {
        _chargeTime = 0;
    }
    #region Cooldown

    //starts the cooldown.
    private void StartCooldown()
    {
        _onCooldown = true;
        Invoke("ResetCooldown", Cooldown);
    }

    //resets the cooldown.
    private void ResetCooldown()
    {
        _onCooldown = false;
    }
    #endregion
}
