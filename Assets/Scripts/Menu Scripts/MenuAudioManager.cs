using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudioManager : MonoBehaviour
{
    [SerializeField] AudioSource ThemeSource;

    public AudioClip Theme;

    private void Awake()
    {
        ThemeSource.Play();
    }

    private void FixedUpdate()
    {
        int state = PlayerPrefs.GetInt("ThemeToggle");

        if (state == 0)
        {
            ThemeSource.mute = true;
        }
        else
        {
            ThemeSource.mute = false;
        }
    }
}
