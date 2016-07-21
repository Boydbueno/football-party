using UnityEngine;
using System.Collections;

public class BombController : MonoBehaviour {

    public float MinDetonationTime;
    public float MaxdetonationTime;

    public float MaxBlinkingInterval;
    public float MinBlinkingInterval;

    public bool BlinkOn = false;

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

    public void SetDetonationTime() 
    {
        _detonationTime = Random.Range(MinDetonationTime, MaxdetonationTime);
        _countDownDuration = _detonationTime;
    }

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
        Debug.Log("Should blink(switch the mesh) now");
        BlinkOn = !BlinkOn;

        float pointInCountdown = _detonationTime/_countDownDuration;
        //choose interval
        float interval = Mathf.Lerp(MinBlinkingInterval, MaxBlinkingInterval, pointInCountdown);
        //invoke next blink
        Invoke("Blink", interval);
    }
}
