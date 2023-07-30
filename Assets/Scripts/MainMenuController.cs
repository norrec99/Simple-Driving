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
    [SerializeField] private TMP_Text refreshTimeText;
    [SerializeField] private int maxEnergy;
    [SerializeField] private int energyRechargeDuration;
    [SerializeField] private Button playButton;
    [Header("Notfication")]
    [SerializeField] private AndroidNotificationController androidNotificationController;

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

        CancelInvoke();

        highScoreAmount = PlayerPrefs.GetInt(ScoreSystem.HighScoreKey, 0);
        highScoreText.SetText(fixedText + highScoreAmount.ToString());

        currentEnergy = PlayerPrefs.GetInt(EnergyKey, maxEnergy);
        refreshTimeText.gameObject.SetActive(false);

        if (currentEnergy == 0)
        {
            refreshTimeText.gameObject.SetActive(true);
            InvokeRepeating(nameof(UpdateRefreshTime), 0f, 1f);
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
        refreshTimeText.gameObject.SetActive(false);
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
            InvokeRepeating(nameof(UpdateRefreshTime), 0f, 1f);
#if UNITY_ANDROID
            androidNotificationController.ScheduleNotification(energyReady);
#endif
        }
        SceneManager.LoadScene(1);
    }

    private void UpdateRefreshTime()
    {
        energyReadyString = PlayerPrefs.GetString(EnergyReadyKey, string.Empty);
        DateTime energyReadyTime = DateTime.Parse(energyReadyString);

        TimeSpan refreshTime = energyReadyTime - DateTime.Now;

        Debug.Log("refreshTime " + refreshTime);

        refreshTimeText.SetText($"Refreshing in: {refreshTime.Minutes}h {refreshTime.Seconds}s");

        if (refreshTime.TotalSeconds < 0)
        {
            CancelInvoke(nameof(UpdateRefreshTime));
        }
    }
}
