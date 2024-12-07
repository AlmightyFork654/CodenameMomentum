using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{

    //[SerializeField] HumanoidLandInput input;
    HumanoidLandController controller;

    Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<HumanoidLandController>();
    }

    void FixedUpdate()
    {
        AnimatePlayer();
    }

    private void ClearAnimations()
    {
        animator.SetBool("isDashing", false);
        animator.SetBool("isBoosting", false);
        animator.SetBool("isJumping", false);
        animator.SetBool("isBoostFalling", false); //niet meer gebruiken?
        animator.SetBool("isFalling", false);
        animator.SetBool("isDoubleJumping", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isWalking", false);
    }

    private void ClearAnimationsExept(string BoolName)
    {
        ClearAnimations();
        animator.SetBool(BoolName, true);
    }

    private void AnimatePlayer()
    {
        if (controller.isBoosting)
        {
            ClearAnimationsExept("isBoosting");
        }
        else if (controller.isDashing)
        {
            ClearAnimationsExept("isDashing");
        }
        else if (!controller.playerIsGrounded && !controller.standingOnHand)
        {
            if (controller.isFalling)
            {
                ClearAnimationsExept("isFalling");
            }
            else if (controller.isDoubleJumping)
            {
                ClearAnimationsExept("isDoubleJumping");
            }
            else if (controller.playerIsJumping)
            {
                ClearAnimationsExept("isJumping");
            }
        }
        else if (controller.playerIsGrounded || controller.standingOnHand)
        {
            if (controller.isWalking)
            {
                ClearAnimationsExept("isWalking");
            }
            else if (controller.isRunning)
            {
                ClearAnimationsExept("isRunning");
            }
            else
            {
                ClearAnimations();
            }
        }
        else 
        {
            ClearAnimations();
        }
    }
}
