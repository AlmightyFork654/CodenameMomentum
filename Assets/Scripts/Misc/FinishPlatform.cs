using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class FinishPlatform : MonoBehaviour
{

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        TimerController.instance.EndTimer();
        string sceneName = SceneManager.GetActiveScene().name;
        float timeInFloat = TimerController.instance.elapsedTime;
        SaveHighScore(sceneName, timeInFloat);
        SaveLastScore(sceneName, timeInFloat);
        WinLoseMenu.Instance.Win();
    }

    private void SaveHighScore(string sceneName, float timeInFloat)
    {
        if (PlayerPrefs.HasKey(sceneName)) {
            if (PlayerPrefs.GetFloat(sceneName) > timeInFloat)
            {
                PlayerPrefs.SetFloat(sceneName, timeInFloat);
            }
        }
        else
        {
            PlayerPrefs.SetFloat(sceneName, timeInFloat);
        }
    }

    private void SaveLastScore(string sceneName, float timeInFloat)
    {
        PlayerPrefs.SetFloat(sceneName + "_last", timeInFloat);
    }
}