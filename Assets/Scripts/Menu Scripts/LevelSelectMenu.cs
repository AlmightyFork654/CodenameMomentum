using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelSelectMenu : MonoBehaviour
{
    [SerializeField] public TMP_Text lvlOne, lvlTwo, lvlThree, lvlFour, lvlFive, lvlSix;

    [SerializeField] public Button lvlOneB, lvlTwoB, lvlThreeB, lvlFourB, lvlFiveB, lvlSixB;


    void Awake()
    {
        lvlOne.text = "00:00.00";
        lvlTwo.text = "00:00.00";
        lvlTwoB.interactable = false;
        lvlThree.text = "00:00.00";
        lvlThreeB.interactable = false;
        lvlFour.text = "00:00.00";
        lvlFourB.interactable = false;
        lvlFive.text = "00:00.00";
        lvlFiveB.interactable = false;
        lvlSix.text = "00:00.00";
        lvlSixB.interactable = false;

        SetScores();
    }

    private void SetScores()
    {
        if (PlayerPrefs.HasKey("LevelOneBlockOut"))
        {
            TimeSpan timePlaying = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("LevelOneBlockOut"));
            string timePlayingStr = timePlaying.ToString("mm':'ss'.'ff");
            lvlOne.text = timePlayingStr;
            lvlTwoB.interactable = true;
        }
        if (PlayerPrefs.HasKey("LevelTwoBlockOut"))
        {
            TimeSpan timePlaying = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("LevelTwoBlockOut"));
            string timePlayingStr = timePlaying.ToString("mm':'ss'.'ff");
            lvlTwo.text = timePlayingStr;
            lvlThreeB.interactable = true;
        }
        if (PlayerPrefs.HasKey("LevelThreeBlockOut"))
        {
            TimeSpan timePlaying = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("LevelThreeBlockOut"));
            string timePlayingStr = timePlaying.ToString("mm':'ss'.'ff");
            lvlThree.text = timePlayingStr;
            lvlFourB.interactable = true;
        }
        if (PlayerPrefs.HasKey("LevelFourBlockOut"))
        {
            TimeSpan timePlaying = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("LevelFourBlockOut"));
            string timePlayingStr = timePlaying.ToString("mm':'ss'.'ff");
            lvlFour.text = timePlayingStr;
            lvlFiveB.interactable = true;
        }
        if (PlayerPrefs.HasKey("LevelFiveBlockOut"))
        {
            TimeSpan timePlaying = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("LevelFiveBlockOut"));
            string timePlayingStr = timePlaying.ToString("mm':'ss'.'ff");
            lvlFive.text = timePlayingStr;
            lvlSixB.interactable = true;
        }
        if (PlayerPrefs.HasKey("LevelSixBlockOut"))
        {
            TimeSpan timePlaying = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("LevelSixBlockOut"));
            string timePlayingStr = timePlaying.ToString("mm':'ss'.'ff");
            lvlSix.text = timePlayingStr;
        }
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
