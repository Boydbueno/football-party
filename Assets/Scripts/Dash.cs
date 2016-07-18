using UnityEngine;
using System.Collections;

public class Dash : MonoBehaviour {


    public float strength;
    private Rigidbody rigidBody;
	// Use this for initialization
	void Start () {

        rigidBody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space"))
        {
            //Dash
            rigidBody.AddForce(rigidBody.velocity * strength);
        }
    }
}
