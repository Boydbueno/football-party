using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Text;

public class ScoreController : MonoBehaviour
{
    public int TeamCount = 1;

    public int[] _scores;
    private Text _textComponent;

    private const int NotATeam = -1;
	
	void Start()
	{
	    _textComponent = GetComponent<Text>();
	    _scores = new int[TeamCount];
        ResetScore();
	}

    //increments the score for the given team.
    public void UpdateScore(int teamNumber, int increment = 1)
    {
        if (teamNumber == NotATeam)
        {
            Debug.Log("No player touched the ball leading up to this goal.");
            return;
        }
        _scores[teamNumber-1] += increment;
        UpdateScoreText();
    }

    //resets all the scores.
    public void ResetScore()
    {
        for(int i = 0; i < _scores.Length; i++)
            _scores[i] = 0;
        UpdateScoreText();
    }

    //updates the score text with the current values.
    private void UpdateScoreText()
    {
        string newText = _scores[0].ToString();
        for (int i = 1; i < _scores.Length; i++)
            newText += " - " + _scores[i];

        _textComponent.text = newText;
    }
}
