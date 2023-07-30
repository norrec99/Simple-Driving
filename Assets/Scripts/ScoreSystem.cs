using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private float scoreMultiplier = 2f;

    private float score;

    // Update is called once per frame
    void Update()
    {
        score += scoreMultiplier * Time.deltaTime;

        scoreText.SetText(Mathf.FloorToInt(score).ToString());
    }
}
