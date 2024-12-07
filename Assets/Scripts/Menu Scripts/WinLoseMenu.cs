using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class WinLoseMenu : MonoBehaviour
{
    public static WinLoseMenu Instance { get; private set; }

    int currentScene;

    public TMP_Text hs, cs, hs2;

    [SerializeField] public GameObject WinScreen;
    [SerializeField] public GameObject LoseScreen;

    void Awake()
    {
        Instance = this;
    }

    public void Win()
    {
        if (HumanoidLandController.Instance.isSlomo)
        {
            HumanoidLandController.Instance.FinalizePlacement();
        }
        WinScreen.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SetHighscore();
        SetCurrentScore();
    }

    public void Lose()
    {
        if (HumanoidLandController.Instance.isSlomo)
        {
            HumanoidLandController.Instance.FinalizePlacement();
        }
        LoseScreen.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SetHighscore();
    }

    public void Redo()
    {
        WinScreen.SetActive(false);
        LoseScreen.SetActive(false);
        Time.timeScale = 1.0f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    public void Next()
    {
        WinScreen.SetActive(false);
        LoseScreen.SetActive(false);
        Time.timeScale = 1.0f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int NextScene = currentScene + 1;
        SceneManager.LoadSceneAsync(NextScene);
    }

    public void ReturnToMenu()
    {
        WinScreen.SetActive(false);
        LoseScreen.SetActive(false);
        Time.timeScale = 1.0f;
        SceneManager.LoadSceneAsync(1);
    }

    public void SetHighscore()
    {
        TimeSpan timePlaying = TimeSpan.FromSeconds(PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name));
        string timePlayingStr = timePlaying.ToString("mm':'ss'.'ff");
        hs.text = "Highscore: " + timePlayingStr;
        hs2.text = "Highscore: " + timePlayingStr;
    }

    public void SetCurrentScore()
    {
        TimeSpan timePlaying = TimeSpan.FromSeconds(PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "_last"));
        string timePlayingStr = timePlaying.ToString("mm':'ss'.'ff");
        cs.text = "Time: " + timePlayingStr;
    }
}
