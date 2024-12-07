using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance { get; private set; }
    public bool GamePaused = false;

    public TMP_Text lvlName, hs, cs, ls;

    [SerializeField] public GameObject PauseScreen;
    [SerializeField] public GameObject MenuScreen;

    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !HumanoidLandController.Instance.handIsActive && !WinLoseMenu.Instance.WinScreen.active && !WinLoseMenu.Instance.LoseScreen.active)
        {
            if (GamePaused && !MenuScreen.active)
            {
                Resume();
            }   
            else
            {
                Pause();
            }
        }
            
    }

    public void Pause()
    {
        GamePaused = true;
        PauseScreen.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        GamePaused = false;
        PauseScreen.SetActive(false);
        Time.timeScale = 1.0f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ReturnToMenu()
    {
        GamePaused = false;
        PauseScreen.SetActive(false);
        Time.timeScale = 1.0f;
        SceneManager.LoadSceneAsync(0);
    }

    public void SetData(string TimeSpanStr)
    {
        string name = SceneManager.GetActiveScene().name;
        if (name == SceneManager.GetSceneByBuildIndex(2).name)
        {
            lvlName.text = "Level 1";
        }
        else if (name == SceneManager.GetSceneByBuildIndex(3).name)
        {
            lvlName.text = "Level 2";
        }
        else if (name == SceneManager.GetSceneByBuildIndex(4).name)
        {
            lvlName.text = "Level 3";
        }
        else if (name == SceneManager.GetSceneByBuildIndex(5).name)
        {
            lvlName.text = "Level 4";
        }
        else if (name == SceneManager.GetSceneByBuildIndex(6).name)
        {
            lvlName.text = "Level 5";
        }
        else if (name == SceneManager.GetSceneByBuildIndex(7).name)
        {
            lvlName.text = "Level 6";
        }

        TimeSpan timePlaying = TimeSpan.FromSeconds(PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name));
        string timePlayingStr = timePlaying.ToString("mm':'ss'.'ff");
        hs.text = "Highscore: " + timePlayingStr;

        TimeSpan timePlayingLast = TimeSpan.FromSeconds(PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "_last"));
        string timePlayingStrLast = timePlayingLast.ToString("mm':'ss'.'ff");
        ls.text = "Previous Time: " + timePlayingStrLast;

        cs.text = "Current Time: " + TimeSpanStr;
    }
}
