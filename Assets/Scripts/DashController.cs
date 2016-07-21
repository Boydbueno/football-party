using UnityEngine;
using System.Collections;
using System.Xml.Schema;
using XInputDotNetPure;

public class DashController : MonoBehaviour {

    //publics
    public float ForceMin, ForceMax;
    public float DashChargeMaxTime; //After how many frames since we started charging the dash, do we achieve max dashing charge.
    public float Cooldown;
    
    //privates
    private bool _onCooldown;
    private bool _dashButtonDown;
    private float _chargeTime;

    private GameManager _gameManager;
    private AudioController _audioController;

    //variables we get from other components.
    private int _playerNumber;
    private Rigidbody _rb;
    private ParticleSystem _ps;
    private TrailRenderer _tr;

    public float minEmmisionRate;
    public float maxEmmisionRate;
    public float minGravityForce;
    public float maxGravityForce;
    public float minPlaySpeed;
    public float maxPlaySpeed;

    public float minRumbleLeft;
    public float maxRumbleLeft;
    public float minRumbleRight;
    public float maxRumbleRight;

    void Start()
    {
        _playerNumber = GetComponent<PlayerController>().PlayerNumber;
        _rb = GetComponent<Rigidbody>();
        _ps = GetComponentInChildren<ParticleSystem>();
        _ps.Stop();
        _tr = GetComponent<TrailRenderer>();
        _tr.enabled = false;
        _gameManager = GameManager.instance;
        _audioController = GetComponent<AudioController>();
        ResetCharge();
    }
	
	void Update()
	{
        if (Input.GetKeyDown(KeyCode.L))
        {
            _gameManager.ScreenShake();
        }
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
        {
            _tr.enabled = false;
            Dash();
        }
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

        // Lerp the rumble!
        float rumbleForceLeft = Mathf.Lerp(minRumbleLeft, maxRumbleLeft, _chargeTime);
        float rumbleForceRight = Mathf.Lerp(minRumbleRight, maxRumbleRight, _chargeTime);
        _gameManager.RumbleStart((PlayerIndex)_playerNumber - 1, rumbleForceLeft, rumbleForceRight);

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
        if (_chargeTime == 0) return;

        float charge = Mathf.Lerp(ForceMin, ForceMax, _chargeTime);
        Vector3 chargeForce = _rb.transform.forward * charge;
        _rb.AddForce(_rb.transform.forward * charge);
        _gameManager.RumbleStop((PlayerIndex)_playerNumber - 1);
        //_audioController.Play("DashRelease");
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
        _tr.enabled = true;
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

