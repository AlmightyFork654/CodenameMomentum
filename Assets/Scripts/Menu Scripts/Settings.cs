using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour{

    public static Settings Instance { get; private set; }

    [SerializeField] private AudioMixer mixer;

    [SerializeField] private Slider ThemeSlider, WalkSlider, JumpSlider, FallSlider, AbilitySlider, SFXSlider;

    [SerializeField] public Button ThemeB1, ThemeB2, WalkB1, WalkB2, JumpB1, JumpB2, FallB1, FallB2, AbilityB1, AbilityB2, SFXB1, SFXB2;

    [SerializeField] private Slider XSlider, YSlider;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("ThemeVol") || PlayerPrefs.HasKey("WalkVol") || PlayerPrefs.HasKey("JumpVol") || PlayerPrefs.HasKey("FallVol") || PlayerPrefs.HasKey("AbilityVol") || PlayerPrefs.HasKey("SFXVol")) {
            LoadVolumes();
        }
        else { 
            SetThemeVolume();
            SetWalkVolume();
            SetJumpVolume();    
            SetFallVolume();
            SetAbilityVolume();
            SetSFXVolume();
        }

        if (PlayerPrefs.HasKey("XMulti") || PlayerPrefs.HasKey("YMulti"))
        {
            LoadSens();
        }
        else
        {
            SetXMulti();
            SetYMulti();
        }

        Toggles();
    }

    public void SetThemeVolume()
    {
        float volume = ThemeSlider.value;
        mixer.SetFloat("Theme", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("ThemeVol", volume);
    }

    public void SetWalkVolume()
    {
        float volume = WalkSlider.value;
        mixer.SetFloat("Walk", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("WalkVol", volume);
    }

    public void SetJumpVolume()
    {
        float volume = JumpSlider.value;
        mixer.SetFloat("Jump", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("JumpVol", volume);
    }

    public void SetFallVolume()
    {
        float volume = FallSlider.value;
        mixer.SetFloat("Fall", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("FallVol", volume);
    }

    public void SetAbilityVolume()
    {
        float volume = AbilitySlider.value;
        mixer.SetFloat("Ability", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("AbilityVol", volume);
    }
    public void SetSFXVolume()
    {
        float volume = SFXSlider.value;
        mixer.SetFloat("Ui", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVol", volume);
    }

    public void Toggles(string Name)
    {
        int state;

        if (PlayerPrefs.HasKey(Name)) {
            if (PlayerPrefs.GetInt(Name) == 0)
            {
                state = 1;
            }
            else
            {
                state = 0;
            }
            PlayerPrefs.SetInt(Name, state);
        }
        else
        {
            PlayerPrefs.SetInt(Name, 0);
        }
        
    }

    public void SetXMulti()
    {
        float sens = XSlider.value;
        PlayerPrefs.SetFloat("XMulti", sens);
    }

    public void SetYMulti()
    {
        float sens = YSlider.value;
        PlayerPrefs.SetFloat("YMulti", sens);
    }

    private void LoadVolumes()
    {
        ThemeSlider.value = PlayerPrefs.GetFloat("ThemeVol");
        WalkSlider.value = PlayerPrefs.GetFloat("WalkVol");
        JumpSlider.value = PlayerPrefs.GetFloat("JumpVol");
        FallSlider.value = PlayerPrefs.GetFloat("FallVol");
        AbilitySlider.value = PlayerPrefs.GetFloat("AbilityVol");
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVol");
        

        SetThemeVolume();
        SetWalkVolume();
        SetJumpVolume();
        SetFallVolume();
        SetAbilityVolume();
        SetSFXVolume();
    }

    private void LoadSens()
    {
        XSlider.value = PlayerPrefs.GetFloat("XMulti");
        YSlider.value = PlayerPrefs.GetFloat("YMulti");

        SetXMulti();
        SetYMulti();
    }

    private void Toggles()
    {
        int themeState = PlayerPrefs.GetInt("ThemeToggle");
        int walkState = PlayerPrefs.GetInt("WalkToggle");
        int JumpState = PlayerPrefs.GetInt("JumpToggle");
        int fallState = PlayerPrefs.GetInt("FallToggle");
        int abilityState = PlayerPrefs.GetInt("AbilityToggle");
        int SFXState = PlayerPrefs.GetInt("SFXToggle");

        if (themeState == 0)
        {
            ThemeB1.gameObject.SetActive(false);
            ThemeB2.gameObject.SetActive(true);
        }
        else
        {
            ThemeB1.gameObject.SetActive(true);
            ThemeB2.gameObject.SetActive(false);
        }

        if (walkState == 0)
        {
            WalkB1.gameObject.SetActive(false);
            WalkB2.gameObject.SetActive(true);
        }
        else
        {
            WalkB1.gameObject.SetActive(true);
            WalkB2.gameObject.SetActive(false);
        }

        if (JumpState == 0)
        {
            JumpB1.gameObject.SetActive(false);
            JumpB2.gameObject.SetActive(true);
        }
        else
        {
            JumpB1.gameObject.SetActive(true);
            JumpB2.gameObject.SetActive(false);
        }

        if (fallState == 0)
        {
            FallB1.gameObject.SetActive(false);
            FallB2.gameObject.SetActive(true);
        }
        else
        {
            FallB1.gameObject.SetActive(true);
            FallB2.gameObject.SetActive(false);
        }

        if (abilityState == 0)
        {
            AbilityB1.gameObject.SetActive(false);
            AbilityB2.gameObject.SetActive(true);
        }
        else
        {
            AbilityB1.gameObject.SetActive(true);
            AbilityB2.gameObject.SetActive(false);
        }

        if (SFXState == 0)
        {
            SFXB1.gameObject.SetActive(false);
            SFXB2.gameObject.SetActive(true);
        }
        else
        {
            SFXB1.gameObject.SetActive(true);
            SFXB2.gameObject.SetActive(false);
        }
    }
}
