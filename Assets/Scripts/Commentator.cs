using UnityEngine;
using System.Collections;

public class Commentator : MonoBehaviour {

    private AudioController _ac;
    public float countDownMin;
    public float countDownMax;
    private float countDownTimer;
    private float countDownBoundary;

    // Use this for initialization
    void Start ()
    {
        _ac = GetComponent<AudioController>();
        countDownBoundary = Random.Range(countDownMin, countDownMax);
    }
	
	// Update is called once per frame
	void Update ()
    {
        countDownTimer += Time.deltaTime;
        if(countDownTimer >= countDownBoundary)
        {
            int randomNum = Random.Range(0, 4);
            _ac.Play("comment" + randomNum);

            countDownTimer = 0;
            countDownBoundary = Random.Range(countDownMin, countDownMax);
        }
    }
}
