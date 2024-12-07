using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    HumanoidLandController controller;
    PlayerAudioManager audioManager;

    Animator animator;

    private bool canEmit = false;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<HumanoidLandController>();
    }

    void FixedUpdate()
    {
        PlayParticles();
    }

    private void Counter()
    {
        int currentTick = System.Environment.TickCount;
        if (currentTick % 3 == 0) { 
            canEmit = true;
        }
        else
        {
            canEmit = false;
        }
    }

    private void PlayParticles()
    {
        if (controller.startedBoost)
        {
            Counter();
            if (canEmit)
            {
                controller.Boost.Emit(1);
            }
        }
        if (controller.startedDash)
        {
            Counter();
            if (canEmit)
            {
                controller.Dash.Emit(1);
            }
        }
        if (!controller.playerIsGrounded && !controller.standingOnHand)
        {
            if (controller.playerIsJumping && !controller.isDoubleJumping)
            {
                if (controller.input.JumpIsPressed && controller.canDoubleJump && !controller.isSlomo)
                {
                    controller.Poof.Emit(18);
                }
            }
            if (controller.isFalling)
            {
                controller.Falling.Emit(1);
            }
        }
        if (controller.playerIsGrounded || controller.standingOnHand)
        {
            if (!controller.playerIsJumping)
            {
                if (controller.input.JumpIsPressed && !controller.isSlomo)
                {
                    controller.Poof.Emit(18);
                }
            }

            if ((controller.isWalking || controller.isRunning) &&!controller.isSlomo)
            {
                
                controller.Trail.Emit(1);
            }
        }
    }
}
