using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevelSelect()
    {
        SceneManager.LoadSceneAsync(1); //Load levelSelect
    }

    public void LvlOne()
    {
        SceneManager.LoadSceneAsync(2); 
    }

    public void LvlTwo()
    {
        SceneManager.LoadSceneAsync(3);
    }

    public void LvlThree()
    {
        SceneManager.LoadSceneAsync(4);
    }

    public void LvlFour()
    {
        SceneManager.LoadSceneAsync(5);
    }

    public void LvlFive()
    {
        SceneManager.LoadSceneAsync(6);
    }

    public void LvlSix()
    {
        SceneManager.LoadSceneAsync(7);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
