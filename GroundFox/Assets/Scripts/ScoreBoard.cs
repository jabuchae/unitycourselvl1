using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    public static ScoreBoard instance;
    private int currentScore = 0;
    private Text scoreText;
    
    // Start is called before the first frame update
    void Start()
    {
        if (ScoreBoard.instance == null)
        {
            ScoreBoard.instance = this;
            scoreText = GetComponent<Text>();
            UpdateScore();
        }
    }

    public void ScorePoints(int points)
    {
        currentScore += points;
        UpdateScore();
    }

    private void UpdateScore()
    {
        string score = currentScore.ToString();
        while (score.Length < 6)
        {
            score = "0" + score;
        }

        scoreText.text = score;
    }
}
