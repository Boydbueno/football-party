using UnityEngine;
using System.Collections;

public class DashController : MonoBehaviour {

    //publics
    public float ForceMin, ForceMax;
    public int DashChargeMaxTime; //After how many frames since we started charging the dash, do we achieve max dashing charge.
    public float Cooldown;
    

    //privates
    private bool _onCooldown;
    private bool _dashButtonDown;
    public float _charge;
    private float _chargeIncrement;

    //variables we get from other components.
    private string _playerNumber;
    private Rigidbody _rb;

    void Start()
    {
        _playerNumber = GetComponent<PlayerController>().playerNumber;
        _rb = GetComponent<Rigidbody>();

        _chargeIncrement = (ForceMax - ForceMin) / DashChargeMaxTime;
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
            AddCharge();
        else if(_charge >= ForceMin) //we pressed last check, and have now released.
            Dash();
    }

    //returns true if the player is charging his dash.
    public bool IsCharging()
    {
        return _charge >= ForceMin;
    }

    //adds charge(clamps to forceMax).
    private void AddCharge()
    {
        _charge += _chargeIncrement*Time.deltaTime;
        //clamp
        if (_charge > ForceMax)
            _charge = ForceMax;
    }

    //the actual dashing. 
    private void Dash()
    {
        _rb.AddForce(_rb.transform.forward * _charge);
        ResetCharge();
        StartCooldown();
    }

    //resets the charge.
    private void ResetCharge()
    {
        _charge = ForceMin;
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
