using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BombController : MonoBehaviour
{
    AudioController _ac;

    public float LethalExplosionRange;
    public float ExplosionForce;

    public float MinDetonationTime;
    public float MaxdetonationTime;

    public float MaxBlinkingInterval;
    public float MinBlinkingInterval;

    public bool BlinkOn;

    public Renderer renderer;
    public GameObject RadiusSphere;
    public Material BlinkOnMaterial;
    public Material BlinkOffMaterial;

    public float _detonationTime;
    private float _countDownDuration;
    private float _curBlinkInterval;

    private bool _hasExploded = false;
    

    void Start()
    {
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
        _ac.Play("BOOM");

        // apply ExplosionForce to all players in range.
        List<PlayerManager.PlayerData> players = GameManager.instance.GetComponentInChildren<PlayerManager>().PlayersData;
        foreach (GameObject player in players.Select(t => t.Player))
        {
            RaycastHit hit;
            Physics.Linecast(transform.position, player.transform.position, out hit);
            if (hit.collider == player.GetComponent<Collider>())
            {
                Debug.Log("Linecast hit a player");
                player.GetComponent<Rigidbody>().AddExplosionForce(ExplosionForce, transform.position, LethalExplosionRange);
            }
        }

        // Put back the normal ball
        GameManager.instance.GoToNormalMode();

        _hasExploded = true;
        //detroy the bomb.
        Destroy(this.gameObject, 3f);
    }

    void Blink()
    {
        //actual blink  
        BlinkOn = !BlinkOn;
        if (BlinkOn) {
            // Set one mesh
            renderer.material = BlinkOnMaterial;
            RadiusSphere.SetActive(false);
        } else {
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
