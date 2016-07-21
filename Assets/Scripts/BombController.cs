using UnityEngine;
using System.Collections;

public class BombController : MonoBehaviour {

    public float MinDetonationTime;
    public float MaxdetonationTime;

    public float MaxBlinkingInterval;
    public float MinBlinkingInterval;
    public float TimeTillMaxBlinkingSpeed;

    public float _detonationTime;

    void Update() 
    {
        _detonationTime -= Time.deltaTime;

        // Todo: Start Blinking!

        if (_detonationTime <= 0) 
        {
            Explode();
        }
    }

    public void SetDetonationTime() 
    {
        _detonationTime = Random.Range(MinDetonationTime, MaxdetonationTime);
    }

    public void Explode() 
    {
        // Todo: Spawn explosion
        GameManager.instance.Smoke(transform.position);
        
        Destroy(this.gameObject);

        // Put back the normal ball or start new game mode, whatever
    }
}
