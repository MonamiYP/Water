using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public event System.EventHandler OnJump;

    private PlayerControls playerControls;

    private void Awake() {
        playerControls = new PlayerControls();
        playerControls.Player.Enable();

        playerControls.Player.Jump.performed += Jump_performed;
    }

    public Vector2 GetMovementVector() {
        Vector2 inputvector = playerControls.Player.Movement.ReadValue<Vector2>();
        inputvector.Normalize();
        return inputvector;
    }

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnJump?.Invoke(this, System.EventArgs.Empty);
    }
}
