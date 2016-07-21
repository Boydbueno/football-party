using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StadionCollision : MonoBehaviour {

    private AudioController _ac;

    void Start()
    {
        _ac = GetComponent<AudioController>();
    }

    void OnCollisionEnter(Collision other)
    {
        GameObject obj = other.gameObject;
        if (obj.tag == "Ball")
        {
            _ac.Play("BallBounce");
        }

    }
}
