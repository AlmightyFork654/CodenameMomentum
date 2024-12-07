using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HumanoidLandInput : MonoBehaviour
{
    public Vector2 MoveInput {  get; private set; } = Vector2.zero;
    public bool MoveIsPressed { get; private set; } = false;

    public Vector2 LookInput { get; private set; } = Vector2.zero;
    public bool invertMouseY { get; private set; } = true;

    public bool RunIsPressed { get; private set; } = false;

    public bool JumpIsPressed { get; private set; } = false;

    public bool DashIsPressed { get; private set; } = false;

    public bool BoostIsPressed { get; private set; } = false;

    public bool PlaceHandIsPressed { get; private set; } = false;

    public bool ConfirmPlaceIsPressed { get; private set; } = false;

    public  InputActions input;

    

    private void OnEnable()
    {
        input = new InputActions();
        input.HumanoidLand.Enable();

        input.HumanoidLand.Move.performed += SetMove;
        input.HumanoidLand.Move.canceled += SetMove;

        input.HumanoidLand.Look.performed += SetLook;
        input.HumanoidLand.Look.canceled += SetLook;

        input.HumanoidLand.Run.started += SetRun;
        input.HumanoidLand.Run.canceled += SetRun;

        input.HumanoidLand.Jump.started += SetJump;
        input.HumanoidLand.Jump.performed += SetJump;
        input.HumanoidLand.Jump.canceled += SetJump;

        input.HumanoidLand.Dash.started += SetDash;
        input.HumanoidLand.Dash.performed += SetDash;
        input.HumanoidLand.Dash.canceled += SetDash;

        input.HumanoidLand.Boost.started += SetBoost;
        input.HumanoidLand.Boost.performed += SetBoost;
        input.HumanoidLand.Boost.canceled += SetBoost;

        input.HumanoidLand.PlaceHand.started += SetPlaceHand;
        input.HumanoidLand.PlaceHand.performed += SetPlaceHand;
        input.HumanoidLand.PlaceHand.canceled += SetPlaceHand;

        input.HumanoidLand.ConfirmPlace.started += ConfirmPlaceHand;
        input.HumanoidLand.ConfirmPlace.performed += ConfirmPlaceHand;
        input.HumanoidLand.ConfirmPlace.canceled += ConfirmPlaceHand;

    }

    private void OnDisable()
    {

        input.HumanoidLand.Move.performed -= SetMove;
        input.HumanoidLand.Move.canceled -= SetMove;

        input.HumanoidLand.Look.performed -= SetLook;
        input.HumanoidLand.Look.canceled -= SetLook;

        input.HumanoidLand.Run.started -= SetRun;
        input.HumanoidLand.Run.canceled -= SetRun;

        input.HumanoidLand.Jump.started -= SetJump;
        input.HumanoidLand.Jump.performed -= SetJump;
        input.HumanoidLand.Jump.canceled -= SetJump;

        input.HumanoidLand.Dash.started -= SetDash;
        input.HumanoidLand.Dash.performed -= SetDash;
        input.HumanoidLand.Dash.canceled -= SetDash;

        input.HumanoidLand.Boost.started -= SetBoost;
        input.HumanoidLand.Boost.performed -= SetBoost;
        input.HumanoidLand.Boost.canceled -= SetBoost;

        input.HumanoidLand.PlaceHand.started -= SetPlaceHand;
        input.HumanoidLand.PlaceHand.performed -= SetPlaceHand;
        input.HumanoidLand.PlaceHand.canceled -= SetPlaceHand;

        input.HumanoidLand.ConfirmPlace.started -= ConfirmPlaceHand;
        input.HumanoidLand.ConfirmPlace.performed -= ConfirmPlaceHand;
        input.HumanoidLand.ConfirmPlace.canceled -= ConfirmPlaceHand;


        input.HumanoidLand.Disable();
    }

    private void SetMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
        MoveIsPressed = !(MoveInput == Vector2.zero);
    }

    private void SetLook(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
    }

    private void SetRun(InputAction.CallbackContext context)
    {
        RunIsPressed = context.started;
    }

    private void SetJump(InputAction.CallbackContext context)
    {
        JumpIsPressed = context.started;
    }

    private void SetDash(InputAction.CallbackContext context)
    {
        DashIsPressed = context.started;
    }

    private void SetBoost(InputAction.CallbackContext context)
    {
        BoostIsPressed = context.started;
    }

    private void SetPlaceHand(InputAction.CallbackContext context)
    {
        PlaceHandIsPressed = context.started;
    }

    private void ConfirmPlaceHand(InputAction.CallbackContext context)
    {
        ConfirmPlaceIsPressed = context.started;
    }

}
