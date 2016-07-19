using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour
{

    private Vector3 _startPos;
	// Use this for initialization
	void Start()
	{
	    _startPos = transform.position;
	}

    public void ResetPosition()
    {
        transform.position = _startPos;
    }
}
