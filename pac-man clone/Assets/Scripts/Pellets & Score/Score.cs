using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    private int score;

    public TextMeshProUGUI scoreText;

    public void AddScore(int scoreValue)
    {
        score += scoreValue;

        UpdateScore();
    }

    private void UpdateScore()
    {
        scoreText.text = score.ToString();
    }
}
