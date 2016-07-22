using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class BombController : MonoBehaviour
{
    AudioController _ac;

    public float NonLethalExplosionRange = 30;
    public float LethalExplosionRange = 15;
    public float ExplosionForce;

    public float MinDetonationTime;
    public float MaxdetonationTime;

    public float MaxBlinkingInterval;
    public float MinBlinkingInterval;

    public bool BlinkOn;
    private bool bombExploding;

    public Renderer renderer;
    public GameObject RadiusSphere;
    public Material BlinkOnMaterial;
    public Material BlinkOffMaterial;

    public float _detonationTime;
    private float _countDownDuration;
    private float _curBlinkInterval;
    private bool toShake;
    private float switchTime;
    private bool _hasExploded = false;
    

    void Start()
    {
        if(NonLethalExplosionRange < LethalExplosionRange)
            throw new Exception("NonLethalExplosionRange can't be smaller than LethalExplosionRange");
        _ac = GetComponent<AudioController>();
        SetDetonationTime();
        Blink();
    }

    void Update()
    {
        _detonationTime -= Time.deltaTime;
        if (_detonationTime <= 0 && !_hasExploded)
        {
            Explode();
        }
    }

    //set the detonation time.
    public void SetDetonationTime() 
    {
        _detonationTime = Random.Range(MinDetonationTime, MaxdetonationTime);
        _countDownDuration = _detonationTime;
    }

    //bomb goes boooooom!
    public void Explode() 
    {

        //spawn a sweet smoke explosion.
        GameManager.instance.Explode(transform.position);

        // apply ExplosionForce to all players in range.
        List<PlayerManager.PlayerData> players = GameManager.instance.GetComponentInChildren<PlayerManager>().PlayersData;
        foreach (GameObject player in players.Select(t => t.Player))
        {
            RaycastHit hit;
            Physics.Linecast(transform.position, player.transform.position, out hit);
            if (hit.collider == player.GetComponent<Collider>())
            {
                Debug.Log("Linecast hit a player");
                //within lethal range, players shuld "die".
                if (hit.distance <= LethalExplosionRange)
                    hit.collider.gameObject.GetComponent<PlayerController>().Die();
                //otherwise we want to apply some force.
                else 
                    player.GetComponent<Rigidbody>().AddExplosionForce(ExplosionForce, transform.position, NonLethalExplosionRange);
            }
        }

        // Put back the normal ball
        GameManager.instance.GoToNormalMode();

        toShake = true;
        _hasExploded = true;
        _ac.Play("BOOM");
        GameManager.instance.ScreenshakeForTime(2f);
        Destroy(this.gameObject);
    }

    void Blink()
    {
        //actual blink  
        BlinkOn = !BlinkOn;
        if (BlinkOn) {
            _ac.Play("Tick");
            // Set one mesh
            renderer.material = BlinkOnMaterial;
            RadiusSphere.SetActive(false);
        } else {
            _ac.Play("Tock");
            // Set other mesh
            renderer.material = BlinkOffMaterial;
            RadiusSphere.SetActive(true);
        }

        float pointInCountdown = _detonationTime/_countDownDuration;
        //choose interval
        float interval = Mathf.Lerp(MinBlinkingInterval, MaxBlinkingInterval, pointInCountdown);
        //invoke next blink
        Invoke("Blink", interval);
    }
}
