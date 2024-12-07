using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAudioManager : MonoBehaviour
{
    HumanoidLandController controller;

    [SerializeField] public AudioSource WalkSFXSource;
    [SerializeField] public AudioSource JumpSFXSource;
    [SerializeField] public AudioSource FallSFXSource;
    [SerializeField] public AudioSource AbilitySFXSource;
    [SerializeField] public AudioSource ThemeSource;
    [SerializeField] public AudioSource SFXui;

    public AudioClip Walk;
    public AudioClip Jump;
    public AudioClip DJump;
    public AudioClip Land;
    public AudioClip Fall;
    public AudioClip Dash;
    public AudioClip Boost;
    public AudioClip Theme;

    public AudioClip AbilityActive;
    public AudioClip canBoost;
    public AudioClip canDash;
    public AudioClip canPlaceHand;

    public AudioClip NowPlaying;

    private int[] pentatonicSemitones = new[] { 0, 2, 4, 7, 9 };

    private int playCount = 0;
    private int currentSemitone = 0;
    private const int playLimit = 12;

    private float walkCooldown = 0.6f;
    private float nextWalkTime = 0f;

    private bool playedDashFull = true;
    private bool playedBoostFull = true;
    private bool playedHandFull = true;
    private bool playedDashAvailable = true;
    private bool playedBoostAvailable = true;
    private bool playedHandAvailable = true;


    private void Awake()
    {
        ThemeSource.Play();
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<HumanoidLandController>();
    }

    private void FixedUpdate()
    {
        playerAudioPlayer();
        PlayUISounds();

        Toggles();
    }

    public void PlayUISFX(AudioClip clip)
    {
        SFXui.PlayOneShot(clip);
    }

    public void playSFX(AudioSource source, AudioClip clip)
    {
        if (source.isPlaying && source != AbilitySFXSource) return;


        FallSFXSource.Stop();
        playCount++;

        if (playCount > playLimit)
        {
            currentSemitone = pentatonicSemitones[Random.Range(0, pentatonicSemitones.Length)];
            playCount = 1;
        }

        float pitch = GetPitchShift(currentSemitone);
        source.pitch = pitch;

        source.PlayOneShot(clip);
    }

    private float GetPitchShift(int semitones)
    {
        float pitch = 1.0f;
        for (int i = 0; i < semitones; i++)
        {
            pitch *= 1.059463f;
        }
        return pitch;
    }

    private void playerAudioPlayer()
    {
        if (controller.input.DashIsPressed && controller.canDash && !controller.isBoosting)
        {
            playSFX(AbilitySFXSource, Dash);
        }
        if (controller.input.BoostIsPressed && controller.canBoost && !controller.isDashing)
        {
            playSFX(AbilitySFXSource, Boost);
        }


        if (controller.playerIsGrounded || controller.standingOnHand)
        {
            if (!controller.playerIsJumping)
            {
                if (controller.input.JumpIsPressed)
                {
                    playSFX(JumpSFXSource, Jump);
                }
            }
        }

        if (controller.wasAirborne && NowPlaying != Land && !controller.isDashing && !controller.isBoosting && !controller.isFalling)
        {
            playSFX(JumpSFXSource, Land);
        }

        if (!controller.playerIsGrounded && !controller.standingOnHand)
        {
            if (controller.isFalling && !controller.isDashing && !controller.isBoosting)
            {
                playSFX(FallSFXSource, Fall);
            }

            if (controller.playerIsJumping && !controller.isDoubleJumping)
            {
                if (controller.input.JumpIsPressed && controller.canDoubleJump)
                {
                    playSFX(JumpSFXSource, DJump);
                }
            }
        }

        if ((controller.playerIsGrounded || controller.standingOnHand) && (controller.isWalking || controller.isRunning) && NowPlaying != Walk)
        {
            if (Time.time >= nextWalkTime)
            {
                playSFX(WalkSFXSource, Walk);
                nextWalkTime = Time.time + walkCooldown;
            }
        }

    }

    private void PlayUISounds()
    {
        if (controller.DashSlider.value == 1.5f && !playedDashFull)
        {
            PlayUISFX(AbilityActive);
            playedDashFull = true;
        }
        else if (controller.DashSlider.value != 1.5f)
        {
            playedDashFull = false;
        }

        if (controller.BoostSlider.value == 1.5f && !playedBoostFull)
        {
            PlayUISFX(AbilityActive);
            playedBoostFull = true;
        }
        else if (controller.BoostSlider.value != 1.5f)
        {
            playedBoostFull = false;
        }

        if (controller.HandSlider.value == 5.0f && !playedHandFull)
        {
            PlayUISFX(AbilityActive);
            playedHandFull = true;
        }
        else if (controller.HandSlider.value != 5.0f)
        {
            playedHandFull = false;
        }

        if (controller.canDash && !playedDashAvailable)
        {
            PlayUISFX(canDash);
            playedDashAvailable = true;
        }
        else if (!controller.canDash)
        {
            playedDashAvailable = false;
        }

        if (controller.canBoost && !playedBoostAvailable)
        {
            PlayUISFX(canBoost);
            playedBoostAvailable = true;
        }
        else if (!controller.canBoost)
        {
            playedBoostAvailable = false;
        }

        if (controller.canPlaceHand && !playedHandAvailable)
        {
            PlayUISFX(canPlaceHand);
            playedHandAvailable = true;
        }
        else if (!controller.canPlaceHand)
        {
            playedHandAvailable = false;
        }
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
            ThemeSource.mute = true;
        }
        else
        {
            ThemeSource.mute = false;
        }

        if (walkState == 0)
        {
            WalkSFXSource.mute = true;
        }
        else
        {
            WalkSFXSource.mute = false;
        }

        if (JumpState == 0)
        {
            JumpSFXSource.mute = true;
        }
        else
        {
            JumpSFXSource.mute = false;
        }

        if (fallState == 0)
        {
            FallSFXSource.mute = true;
        }
        else
        {
            FallSFXSource.mute = false;
        }

        if (abilityState == 0)
        {
            AbilitySFXSource.mute = true;
        }
        else
        {
            AbilitySFXSource.mute = false;
        }

        if (SFXState == 0)
        {
            SFXui.mute = true;
        }
        else
        {
            SFXui.mute = false;
        }
    }
}
