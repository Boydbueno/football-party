using UnityEngine;
using System.Collections;

public class BombController : MonoBehaviour {

    public float MinDetonationTime;
    public float MaxdetonationTime;

    public float MinBlinkingSpeed;
    public float MaxBlinkingSpeed;
    public float TimeTillMaxBlinkingSpeed;

    public float _detonationTime;

    void Update() 
    {
        _detonationTime -= Time.deltaTime;

        // We have to make the object blink..
        StartCoroutine("Blinking");
        // The blinking should have a default duration, so that it doesn't give away the point it explodes

        // Max blinking speed

        // When we reach zero, explode!
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
        
        // Remove this game object
        Destroy(this.gameObject);
    }

    public IEnumerator Blinking() {
        while (_detonationTime > 0) {
            Mathf.Lerp()
            yield return new WaitForSeconds();
        }
        yield return null;
    }

}
