using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private float scoreMultiplier = 2f;

    private float score;
    private float currentHighScore;
    public const string HighScoreKey = "HighScore";

    // Update is called once per frame
    void Update()
    {
        score += scoreMultiplier * Time.deltaTime;

        scoreText.SetText(Mathf.FloorToInt(score).ToString());

        currentHighScore = PlayerPrefs.GetInt(HighScoreKey, 0);

        if (score > currentHighScore)
        {
            PlayerPrefs.SetInt(HighScoreKey, Mathf.FloorToInt(score));
        }
    }
}
