using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerObject : MonoBehaviour
{
    public KRB_CharacterController Character;

    private Vector2 _movement = Vector2.zero;
    private Vector2 _aim = Vector2.zero;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        HandleCharacterInput();

        //Debug.Log(_attackButtonPressed);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        _movement = context.ReadValue<Vector2>();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        _aim = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Character.RequestJump();
        }     
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Character.RequestDash();
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Character.RequestAttack();
        }
    }

    private void HandleCharacterInput()
    {
        PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

        // Build the CharacterInputs struct
        characterInputs.MoveAxisForward = _movement.y;
        characterInputs.MoveAxisRight = _movement.x;
        characterInputs.CameraRotation = Camera.main.transform.rotation;
        characterInputs.AimAxisForward = _aim.y;
        characterInputs.AimAxisRight = _aim.x;

        // Apply inputs to character
        Character.SetInputs(ref characterInputs);
    }
}
