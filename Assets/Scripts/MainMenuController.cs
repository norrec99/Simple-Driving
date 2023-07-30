using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private Button playButton;

    private int highScoreAmount;
    private string fixedText = "High Score: ";

    private void Awake() 
    {
        playButton.onClick.AddListener(OnPlayClicked);
    }

    // Start is called before the first frame update
    void Start()
    {
        highScoreAmount = PlayerPrefs.GetInt(ScoreSystem.HighScoreKey, 0);
        highScoreText.SetText(fixedText + highScoreAmount.ToString());
    }

    private void OnPlayClicked()
    {
        SceneManager.LoadScene(1);
    }
}
