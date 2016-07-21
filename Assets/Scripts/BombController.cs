using UnityEngine;
using System.Collections;

public class BombController : MonoBehaviour {

    public float MinDetonationTime;
    public float MaxdetonationTime;

    public float MaxBlinkingInterval;
    public float MinBlinkingInterval;

    public bool BlinkOn = false;

    public Renderer renderer;
    public Material BlinkOnMaterial;
    public Material BlinkOffMaterial;

    public float _detonationTime;
    private float _countDownDuration;
    private float _curBlinkInterval;

    void Start()
    {
        SetDetonationTime();
        Blink();
    }

    void Update() 
    {
        _detonationTime -= Time.deltaTime;
        
        if (_detonationTime <= 0) 
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
        // Todo: Spawn explosion
        GameManager.instance.Smoke(transform.position);
        
        Destroy(this.gameObject);

        // Put back the normal ball or start new game mode, whatever
    }

    void Blink()
    {
        //actual blink  

        BlinkOn = !BlinkOn;
        if (BlinkOn) {
            // Set one mesh
            renderer.material = BlinkOnMaterial;
        } else {
            // Set other mesh
            renderer.material = BlinkOffMaterial;
        }

        float pointInCountdown = _detonationTime/_countDownDuration;
        //choose interval
        float interval = Mathf.Lerp(MinBlinkingInterval, MaxBlinkingInterval, pointInCountdown);
        //invoke next blink
        Invoke("Blink", interval);
    }
}
