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

        StartCoroutine("Blinking");

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

    public IEnumerator Blinking() 
    {
        while (_detonationTime > 0) {
            //Mathf.Lerp(MinBlinkingSpeed, MaxBlinkingSpeed, )
            yield return new WaitForSeconds(1);
        }
        yield return null;
    }

}
