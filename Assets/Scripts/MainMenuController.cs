using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text energyLeftText;
    [SerializeField] private int maxEnergy;
    [SerializeField] private int energyRechargeDuration;
    [SerializeField] private Button playButton;

    private const string EnergyKey = "Energy";
    private const string EnergyReadyKey = "EnergyReady";

    private int currentEnergy;
    private int highScoreAmount;
    private string fixedText = "High Score: ";
    private string energyReadyString;

    private void Awake() 
    {
        playButton.onClick.AddListener(OnPlayClicked);
    }

    private void Start() 
    {
        OnApplicationFocus(true);
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus) { return; }

        CancelInvoke(nameof(EnergyRecharged));

        highScoreAmount = PlayerPrefs.GetInt(ScoreSystem.HighScoreKey, 0);
        highScoreText.SetText(fixedText + highScoreAmount.ToString());

        currentEnergy = PlayerPrefs.GetInt(EnergyKey, maxEnergy);

        if (currentEnergy == 0)
        {
            energyReadyString = PlayerPrefs.GetString(EnergyReadyKey, string.Empty);

            if (energyReadyString == string.Empty) { return; }

            DateTime energyReadyTime = DateTime.Parse(energyReadyString);

            if (DateTime.Now > energyReadyTime)
            {
                currentEnergy = maxEnergy;
                PlayerPrefs.SetInt(EnergyKey, currentEnergy);
            }
            else
            {
                playButton.interactable = false;
                Invoke(nameof(EnergyRecharged), (energyReadyTime - DateTime.Now).Seconds);
            }

        }

        energyLeftText.SetText($"Play ({currentEnergy})");
    }

    private void EnergyRecharged()
    {
        playButton.interactable = true;
        currentEnergy = maxEnergy;
        PlayerPrefs.SetInt(EnergyKey, currentEnergy);
        energyLeftText.SetText($"Play ({currentEnergy})");

    }

    private void OnPlayClicked()
    {
        if (currentEnergy < 1) { return; }

        currentEnergy--;

        PlayerPrefs.SetInt(EnergyKey, currentEnergy);

        if (currentEnergy == 0)
        {
            DateTime energyReady = DateTime.Now.AddMinutes(energyRechargeDuration);
            PlayerPrefs.SetString(EnergyReadyKey, energyReady.ToString());
        }


        SceneManager.LoadScene(1);
    }
}
