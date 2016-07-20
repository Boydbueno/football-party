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
    private ParticleSystem _ps;

    public float minEmmisionRate;
    public float maxEmmisionRate;
    public float minGravityForce;
    public float maxGravityForce;
    public float minPlaySpeed;
    public float maxPlaySpeed;

    void Start()
    {
        _playerNumber = GetComponent<PlayerController>().PlayerNumber;
        _rb = GetComponent<Rigidbody>();
        _ps = GetComponentInChildren<ParticleSystem>();
        ResetCharge();

    }
	
	void Update()
	{   
        //get input.
	    _dashButtonDown = Input.GetButton("Dash" + _playerNumber);
	}


    void FixedUpdate()
    {
        //on cooldown, we do nothing.
        if (_onCooldown) return;

        if (_dashButtonDown)
            UpdateCharge();
        else //we pressed last check, and have now released.
            Dash();
    }

    //returns true if the player is charging his dash.
    public bool IsCharging()
    {
        return _chargeTime > ForceMin;
    }

    //updates charge timer
    private void UpdateCharge()
    {
        _chargeTime += Time.deltaTime / DashChargeMaxTime;
        //clamp
        if (_chargeTime > 1)
            _chargeTime = 1;

        if (_ps.isPlaying)
        {
            _ps.gravityModifier = Mathf.Lerp(minGravityForce, maxGravityForce, _chargeTime);
            _ps.emissionRate = Mathf.Lerp(minEmmisionRate, maxEmmisionRate, _chargeTime);
            _ps.playbackSpeed = Mathf.Lerp(minPlaySpeed, maxPlaySpeed, _chargeTime);
        }

        else
        {
            ResetParticles();
            _ps.Play();
        }
    }

    //the actual dashing. 
    private void Dash()
    {
        float charge = Mathf.Lerp(ForceMin, ForceMax, _chargeTime);
        Vector3 chargeForce = _rb.transform.forward * charge;
        _rb.AddForce(_rb.transform.forward * charge);
        ResetCharge();
        ResetParticles();
        StartCooldown();
    }

    //resets the charge.
    private void ResetCharge()
    {
        _chargeTime = 0;
       
    }

    private void ResetParticles()
    {
        _ps.emissionRate = 0;
        _ps.playbackSpeed = 10;
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

